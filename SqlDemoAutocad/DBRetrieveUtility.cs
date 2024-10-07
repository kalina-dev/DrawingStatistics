using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using DataTable = System.Data.DataTable;
using System.Data;
using Microsoft.Data.SqlClient;
using AutoCAD.SQL.Plugin;

namespace AutocadSQLPlugin
{
    public class DBRetrieveUtility
    {
        private readonly string _connectionString = SettingsDb.Default.connectionString;
        public string RetrieveAndDrawLines()
        {
            string result = "";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                try
                {
                    string sql = "SELECT Id, StartPtx, StartPtY, EndPtX, EndPtY, Layer, Color, Linetype FROM dbo.Lines WHERE IsDeleted IS NULL";
                    SqlDataAdapter adapter = new(sql, conn);
                    DataTable dt = new();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Document activeDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
                        Database database = activeDocument.Database;
                        Editor editor = activeDocument.Editor;

                        activeDocument.LockDocument();
                        using Transaction transaction = database.TransactionManager.StartTransaction();
                        editor.WriteMessage("Drawing Lines!");
                        BlockTable blockTable = (BlockTable)transaction.GetObject(database.BlockTableId, OpenMode.ForRead);
                        BlockTableRecord? record = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        int id;
                        string layer = "", color = "", linetype = "";
                        int i = 0;

                        double[] coords = new double[4];
                        foreach (DataRow dr in dt.Rows)
                        {
                            id = Convert.ToInt32(dr["Id"]);
                            coords[i] = Convert.ToDouble(dr["StartPtX"]);
                            coords[i + 1] = Convert.ToDouble(dr["StartPtY"]);
                            coords[i + 2] = Convert.ToDouble(dr["EndPtX"]);
                            coords[i + 3] = Convert.ToDouble(dr["EndPtY"]);
                            layer = dr["Layer"].ToString() ?? "";
                            color = dr["Color"].ToString() ?? "";
                            linetype = dr["Linetype"].ToString() ?? "";

                            Point3d pt1 = new(coords[0], coords[1], 0);
                            Point3d pt2 = new(coords[2], coords[3], 0);

                            Line ln = new(pt1, pt2)
                            {
                                Layer = layer,
                                Linetype = linetype,
                                ColorIndex = CommonUtility.GetColorIndex(color)
                            };
                            ObjectId objectId = record?.AppendEntity(ln) ?? new();
                            transaction.AddNewlyCreatedDBObject(ln, true);
                            CommonUtility.AddXDataToEntity("AUTOCADDB", ln, id);
                        }
                        transaction.Commit();
                    }
                    result = "Data selected successfully!";
                }
                catch (Exception ex)
                {
                    result = "Error encountered: " + ex.Message;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return result;
            };
        }
    }
}

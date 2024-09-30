using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using DataTable = System.Data.DataTable;
using System.Data;
using System.Data.SqlClient;

namespace DbAutocadDemoNemetschek
{
    public class DBRetrieveUtility
    {        
        public string RetrieveAndDrawLines()
        {
            string result = "";
            SqlConnection conn = new SqlConnection();
            try
            {
                conn = DBUtility.GetConnection();
                string sql = "SELECT Id, StartPtx, StartPtY, EndPtX, EndPtY, Layer, Color, Linetype FROM dbo.Lines WHERE IsDeleted IS NULL";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);                
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {                    
                    Document activeDocument = Application.DocumentManager.MdiActiveDocument;
                    Database database = activeDocument.Database;
                    Editor editor = activeDocument.Editor;

                    activeDocument.LockDocument();
                    using (Transaction transaction = database.TransactionManager.StartTransaction())
                    {
                        editor.WriteMessage("Drawing Lines!");
                        BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord record = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;                        

                        int id;
                        string layer="", color="", linetype="";
                        int i = 0;
                                                
                        double[] coords = new double[4];
                        foreach (DataRow dr in dt.Rows)
                        {
                            id = Convert.ToInt32(dr["Id"]);
                            coords[i] = Convert.ToDouble(dr["StartPtX"]);
                            coords[i + 1] = Convert.ToDouble(dr["StartPtY"]);
                            coords[i + 2] = Convert.ToDouble(dr["EndPtX"]);
                            coords[i + 3] = Convert.ToDouble(dr["EndPtY"]);
                            layer = dr["Layer"].ToString();
                            color = dr["Color"].ToString();
                            linetype = dr["Linetype"].ToString();                            
                            
                            Point3d pt1 = new Point3d(coords[0], coords[1], 0);
                            Point3d pt2 = new Point3d(coords[2], coords[3], 0);

                            Line ln = new Line(pt1, pt2)
                            {
                                Layer = layer,
                                Linetype = linetype,
                                ColorIndex = CommonUtility.GetColorIndex(color)
                            };
                            record.AppendEntity(ln);
                            transaction.AddNewlyCreatedDBObject(ln, true);
                            CommonUtility.AddXDataToEntity("AUTOCADDB", ln, id);
                        }
                        transaction.Commit();
                    }
                }
                result = "Completed successfully!";
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
        }
    }
}

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.Data;
using Microsoft.Data.SqlClient;
using AutoCAD.SQL.Plugin;

namespace AutocadSQLPlugin
{
    public class DBLoadUtility
    {
        private readonly string _connectionString = SettingsDb.Default.connectionString;
        public string LoadLines()
        {
            var result = "";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                try
                {

                    Document activeDocument = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
                    Editor editor = activeDocument.Editor;

                    using Transaction transaction = activeDocument.TransactionManager.StartTransaction();
                    TypedValue[] tv = new TypedValue[1];
                    tv.SetValue(new TypedValue((int)DxfCode.Start, "LINE"), 0);
                    SelectionFilter filter = new(tv);

                    PromptSelectionResult ssPrompt = editor.SelectAll(filter);
                    // Check if there is object selected
                    if (ssPrompt.Status == PromptStatus.OK)
                    {
                        double startPtX = 0.0, startPtY = 0.0, endPtX = 0.0, endPtY = 0.0;
                        string layer = "", ltype = "", color = "";
                        double len = 0.0;
                        Line line = new();
                        SelectionSet ss = ssPrompt.Value;
                        string sql = @"INSERT INTO dbo.Lines (StartPtX, StartPtY, EndPtX, EndPtY, Layer, Color, Linetype, Length, Created) 
                                       VALUES(@StartPtX, @StartPtY, @EndPtX, @EndPtY, @Layer, @Color, @Linetype, @Length, @Created)";

                        // Loop through the selection set and insert into database one line object at a time
                        foreach (SelectedObject sObj in ss)
                        {
                            line = (Line)transaction.GetObject(sObj.ObjectId, OpenMode.ForRead);
                            startPtX = line.StartPoint.X;
                            startPtY = line.StartPoint.Y;
                            endPtX = line.EndPoint.X;
                            endPtY = line.EndPoint.Y;
                            layer = line.Layer;
                            ltype = line.Linetype;
                            color = line.Color.ToString();
                            len = line.Length;

                            SqlCommand cmd = new(sql, conn);
                            cmd.Parameters.AddWithValue("@StartPtX", startPtX);
                            cmd.Parameters.AddWithValue("@StartPtY", startPtY);
                            cmd.Parameters.AddWithValue("@EndPtX", endPtX);
                            cmd.Parameters.AddWithValue("@EndPtY", endPtY);
                            cmd.Parameters.AddWithValue("@Layer", layer);
                            cmd.Parameters.AddWithValue("@Color", color);
                            cmd.Parameters.AddWithValue("@Linetype", ltype);
                            cmd.Parameters.AddWithValue("@Length", len);
                            cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        editor.WriteMessage("No object selected.");
                    }
                    result = "Data inserted successfully!";
                }
                catch (Exception ex)
                {
                    result = ex.Message;
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

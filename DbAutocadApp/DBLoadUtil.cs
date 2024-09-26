﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.Data;
using System.Data.SqlClient;
using System;

namespace DbAutocadDemoNemetschek
{
    public class DBLoadUtil
    {
        // Load all the Line Objects into Database
        public string LoadLines()
        {
            string result = "";
            SqlConnection conn = DBUtil.GetConnection();

            try
            {
                // Get the Document and Editor object
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;

                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {   
                    TypedValue[] tv = new TypedValue[1];
                    tv.SetValue(new TypedValue((int)DxfCode.Start, "LINE"), 0);
                    SelectionFilter filter = new SelectionFilter(tv);

                    PromptSelectionResult ssPrompt = ed.SelectAll(filter);
                    // Check if there is object selected
                    if (ssPrompt.Status == PromptStatus.OK)
                    {
                        double startPtX = 0.0, startPtY = 0.0, endPtX = 0.0, endPtY = 0.0;
                        string layer = "", ltype = "", color = "";
                        double len = 0.0;
                        Line line = new Line();                        
                        SelectionSet ss = ssPrompt.Value;
                        String sql = @"INSERT INTO dbo.Lines (StartPtX, StartPtY, EndPtX, EndPtY, Layer, Color, Linetype, Length, Created) 
                                       VALUES(@StartPtX, @StartPtY, @EndPtX, @EndPtY, @Layer, @Color, @Linetype, @Length, @Created)";
                        conn.Open();

                        // Loop through the selection set and insert into database one line object at a time
                        foreach (SelectedObject sObj in ss)
                        {
                            line = trans.GetObject(sObj.ObjectId, OpenMode.ForRead) as Line;
                            startPtX = line.StartPoint.X;
                            startPtY = line.StartPoint.Y;
                            endPtX = line.EndPoint.X;
                            endPtY = line.EndPoint.Y;
                            layer = line.Layer;
                            ltype = line.Linetype;
                            color = line.Color.ToString();
                            len = line.Length;
                                                        
                            SqlCommand cmd = new SqlCommand(sql, conn);
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
                        ed.WriteMessage("No object selected.");
                    }
                    result = "Completed successfully!";
                }
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
        }
    }
}

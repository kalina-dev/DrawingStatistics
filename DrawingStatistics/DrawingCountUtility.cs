using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using System.IO;

namespace DrawingStatistics
{
    internal class DrawingCountUtility
    {
        private const string keywordScreen = "Screen";
        private const string keywordTXT = "TXT";
        private const string keywordCSV = "CSV";
        private const string keywordHTML = "HTML";
        private readonly string error = string.Empty;
        Editor edt;
        public DrawingCountUtility()
        {
            error = "Error encountered: ";
            edt = Application.DocumentManager.MdiActiveDocument.Editor;
        }

        [CommandMethod("DrawingStatistics")]
        public void CountDrawingOptions()
        {
            PromptKeywordOptions pko = new PromptKeywordOptions("Select Display Mode: ");
            pko.Keywords.Add(keywordScreen);
            pko.Keywords.Add(keywordTXT);
            pko.Keywords.Add(keywordCSV);
            pko.Keywords.Add(keywordHTML);
            pko.AllowNone = true;

            PromptResult res = edt.GetKeywords(pko);
            string answer = res.StringResult;

            switch (answer) {
                case keywordScreen:
                    DisplayDrawingCountOnScreen();
                    break;
                case keywordTXT:
                    WriteDrawingCountToTextFile();
                    break;
                case keywordCSV:
                    WriteDrawingCountToCSVFile();
                    break;
                case keywordHTML:
                    WriteDrawingCountToHTMLFile();
                    break;
            }
        }

        private void WriteDrawingCountToHTMLFile()
        {
            try
            {
                PromptStringOptions pso = new PromptStringOptions("Enter HTML filename and location: ");
                PromptResult pr = edt.GetString(pso);
                string filename = pr.StringResult;

                if (filename != "")
                {
                    // Now, write the information to a file
                    int lineCount, mtxCount, plCount, arcCount, blkCount;
                    lineCount = GetEntityCount("LINE");
                    mtxCount = GetEntityCount("MTEXT");
                    plCount = GetEntityCount("LWPOLYLINE");
                    arcCount = GetEntityCount("ARC");
                    blkCount = GetEntityCount("INSERT");

                    int totalCount = lineCount + mtxCount + plCount + arcCount + blkCount;

                    using (StreamWriter file = new StreamWriter(filename))
                    {
                        // Write the Header
                        file.WriteLine("<html>");
                        file.WriteLine("<head></head>");
                        file.WriteLine("<body>");
                        file.WriteLine("<h2 style='background-color:red'>List of Objects found in the drawing:</h2>");
                        file.WriteLine("<table border=1>");
                        file.WriteLine("<tr>");
                        file.WriteLine("<td style='color:blue'>Lines</td>");
                        file.WriteLine("<td style='color:blue'>MTexts</td>");
                        file.WriteLine("<td style='color:blue'>Polylines</td>");
                        file.WriteLine("<td style='color:blue'>Arcs</td>");
                        file.WriteLine("<td style='color:blue'>Blocks</td>");
                        file.WriteLine("<td style='color:blue'>Total</td></tr>");
                        file.WriteLine("</tr>");
                        file.WriteLine("<tr>");
                        file.WriteLine("<td>" + lineCount.ToString() + "</td>");
                        file.WriteLine("<td>" + mtxCount.ToString() + "</td>");
                        file.WriteLine("<td>" + plCount.ToString() + "</td>");
                        file.WriteLine("<td>" + arcCount.ToString() + "</td>");
                        file.WriteLine("<td>" + blkCount.ToString() + "</td>");
                        file.WriteLine("<td>" + totalCount.ToString() + "</td>");
                        file.WriteLine("</tr>");
                        file.WriteLine("</table>");
                        file.WriteLine("</body>");
                        file.WriteLine("</html>");
                    }
                }
            }
            catch (System.Exception ex)
            {
                edt.WriteMessage(error + ex.Message);
            }
        }

        private void WriteDrawingCountToCSVFile()
        {
            try
            {
                PromptStringOptions pso = new PromptStringOptions("Enter CSV filename and location: ");
                PromptResult pr = edt.GetString(pso);
                string filename = pr.StringResult;

                if (filename != "")
                {
                    // Now, write the information to a file
                    int lineCount, mtxCount, plCount, arcCount, blkCount;
                    lineCount = GetEntityCount("LINE");
                    mtxCount = GetEntityCount("MTEXT");
                    plCount = GetEntityCount("LWPOLYLINE");
                    arcCount = GetEntityCount("ARC");
                    blkCount = GetEntityCount("INSERT");

                    int totalCount = lineCount + mtxCount + plCount + arcCount + blkCount;

                    using (StreamWriter file = new StreamWriter(filename))
                    {
                        file.WriteLine("List of Objects found in the drawing: ");
                        file.WriteLine("Lines, MTexts, Polylines, Arcs, Blocks, Total");
                        file.WriteLine(lineCount.ToString() + "," + mtxCount.ToString() + "," + plCount.ToString() + "," + arcCount.ToString() + "," + blkCount.ToString() + "," + totalCount.ToString());
                    }
                }
                else
                {
                    edt.WriteMessage("File not found");
                }
            }
            catch (System.Exception ex)
            {
                edt.WriteMessage(error + ex.Message);
            }
        }

        private void WriteDrawingCountToTextFile()
        {
            try
            {
                PromptStringOptions pso = new PromptStringOptions("Enter TXT filename and location: ");
                PromptResult pr = edt.GetString(pso);
                string filename = pr.StringResult;

                if (filename != "")
                {
                    // Now, write the information to a file
                    int lineCount, mtxCount, plCount, arcCount, blkCount;
                    lineCount = GetEntityCount("LINE");
                    mtxCount = GetEntityCount("MTEXT");
                    plCount = GetEntityCount("LWPOLYLINE");
                    arcCount = GetEntityCount("ARC");
                    blkCount = GetEntityCount("INSERT");

                    int totalCount = lineCount + mtxCount + plCount + arcCount + blkCount;

                    using (StreamWriter file = new StreamWriter(filename))
                    {
                        file.WriteLine("\nList of Objects found in the drawing: ");
                        file.WriteLine("\nLines: " + lineCount.ToString());
                        file.WriteLine("\nMTexts: " + mtxCount.ToString());
                        file.WriteLine("\nPoylines: " + plCount.ToString());
                        file.WriteLine("\nArcs: " + arcCount.ToString());
                        file.WriteLine("\nBlocks: " + blkCount.ToString());
                        file.WriteLine("\nTotal Objects Count: " + totalCount.ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                edt.WriteMessage(error + ex.Message);
            }
        }

        public void DisplayDrawingCountOnScreen()
        {
            try
            {
                int lineCount, mtxCount, plCount, arcCount, blkCount;
                lineCount = GetEntityCount("LINE");
                mtxCount = GetEntityCount("MTEXT");
                plCount = GetEntityCount("LWPOLYLINE");
                arcCount = GetEntityCount("ARC");
                blkCount = GetEntityCount("INSERT");

                int totalCount = lineCount + mtxCount + plCount + arcCount + blkCount;

                // Now, display the results
                edt.WriteMessage("\nList of Objects found in the drawing: ");
                edt.WriteMessage("\nLines: " + lineCount.ToString());
                edt.WriteMessage("\nMTexts: " + mtxCount.ToString());
                edt.WriteMessage("\nPoylines: " + plCount.ToString());
                edt.WriteMessage("\nArcs: " + arcCount.ToString());
                edt.WriteMessage("\nBlocks: " + blkCount.ToString());
                edt.WriteMessage("\nTotal Objects Count: " + totalCount.ToString());
            }
            catch (System.Exception ex)
            {
                edt.WriteMessage(error + ex.Message);
            }
        }

        private int GetEntityCount(string entityType)
        {
            // Get all the Objects in the specified entityType
            TypedValue[] tv = new TypedValue[1];
            tv.SetValue(new TypedValue((int)DxfCode.Start, entityType), 0);
            SelectionFilter filter = new SelectionFilter(tv);
            PromptSelectionResult ssPrompt = edt.SelectAll(filter);
            int objCount = 0;
            if (ssPrompt.Status == PromptStatus.OK)
            {
                SelectionSet ss = ssPrompt.Value;
                objCount = ss.Count;
            }
            return objCount;
        }
    }
}

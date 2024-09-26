using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using System.IO;

namespace DrawingStatistics
{
    public class DrawingCountUtility
    {
        const string keywordScreen = @"Screen";
        const string keywordTXT = @"TXT";
        const string keywordCSV = @"CSV";
        const string keywordHTML = @"HTML";
        const string error = @"Error encountered: ";
        const string objectLine = @"LINE";
        const string objectMtext = @"MTEXT";
        const string objectLwPolyline = @"LWPOLYLINE";
        const string objectArc = @"ARC";
        const string objectBlock = @"INSERT";
        const string titleMessage = @"Number of objects found in the drawing: ";
        const string promptMessage = @"Select Object Statistics Display Mode: ";
        readonly Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;

        [CommandMethod("CountObjectAndShowStatistics")]
        public void CountObjectAndShowStatistics()
        {
            PromptKeywordOptions promptOptions = new PromptKeywordOptions(promptMessage);
            promptOptions.Keywords.Add(keywordScreen);
            promptOptions.Keywords.Add(keywordTXT);
            promptOptions.Keywords.Add(keywordCSV);
            promptOptions.Keywords.Add(keywordHTML);
            promptOptions.AllowNone = false;

            PromptResult result = editor.GetKeywords(promptOptions);
            string answer = result.StringResult;
            editor.WriteMessage(@"Selected answer is " + answer);

            if (answer != null)
            {
                DisplayOrWriteObjectStatistics(answer);
            }
            else
            {
                editor.WriteMessage(@"No response.");
            }
        }

        private void DisplayOrWriteObjectStatistics(string answer)
        {
            try
            {
                string filename = string.Empty;
                int lineCount, mtxCount, plCount, arcCount, blkCount;

                lineCount = GetEntityCount(objectLine);
                mtxCount = GetEntityCount(objectMtext);
                plCount = GetEntityCount(objectLwPolyline);
                arcCount = GetEntityCount(objectArc);
                blkCount = GetEntityCount(objectBlock);

                int totalCount = lineCount + mtxCount + plCount + arcCount + blkCount;

                if (answer != keywordScreen && answer != null)
                {
                    PromptStringOptions promptOptions = new PromptStringOptions(@"Enter " + answer + " filename and its location (path) in the format C:\\Autodesk\\example.### where ### is the file extension. ");
                    PromptResult promptResult = editor.GetString(promptOptions);
                    
                    filename = promptResult.StringResult;
                    string[] paramKeyword = new string[] { filename, keywordCSV, keywordTXT, keywordHTML };
                    if (!HelperUtility.CheckFile(editor, paramKeyword))
                    {
                        editor.WriteMessage(error);
                    }
                }

                if (!string.IsNullOrEmpty(filename) && answer != keywordScreen)
                {
                    using (StreamWriter file = new StreamWriter(filename))
                    {
                        switch (answer)
                        {
                            case keywordTXT:
                                file.WriteLine(titleMessage);
                                file.WriteLine(@"\nLines: " + lineCount.ToString());
                                file.WriteLine("@\nMTexts: " + mtxCount.ToString());
                                file.WriteLine(@"\nPoylines: " + plCount.ToString());
                                file.WriteLine(@"\nArcs: " + arcCount.ToString());
                                file.WriteLine(@"\nBlocks: " + blkCount.ToString());
                                file.WriteLine(@"\nTotal Objects Count: " + totalCount.ToString());
                                break;
                            case keywordCSV:
                                file.WriteLine(titleMessage);
                                file.WriteLine(@"Lines, MTexts, Polylines, Arcs, Blocks, Total");
                                file.WriteLine(lineCount.ToString() + "," + mtxCount.ToString() + "," + plCount.ToString() + "," + arcCount.ToString() + "," + blkCount.ToString() + "," + totalCount.ToString());
                                break;
                            case keywordHTML:
                                file.WriteLine(@"<html>");
                                file.WriteLine(@"<head></head>");
                                file.WriteLine(@"<body>");
                                file.WriteLine(@"<h2 style='background-color:yellow'>List of Objects found in the drawing:</h2>");
                                file.WriteLine(@"<table border=1>");
                                file.WriteLine(@"<tr>");
                                file.WriteLine(@"<td style='color:green'>Lines</td>");
                                file.WriteLine(@"<td style='color:green'>MTexts</td>");
                                file.WriteLine(@"<td style='color:green'>Polylines</td>");
                                file.WriteLine(@"<td style='color:green'>Arcs</td>");
                                file.WriteLine(@"<td style='color:green'>Blocks</td>");
                                file.WriteLine(@"<td style='color:green'>Total</td></tr>");
                                file.WriteLine(@"</tr>");
                                file.WriteLine(@"<tr>");
                                file.WriteLine(@"<td>" + lineCount.ToString() + "</td>");
                                file.WriteLine(@"<td>" + mtxCount.ToString() + "</td>");
                                file.WriteLine(@"<td>" + plCount.ToString() + "</td>");
                                file.WriteLine(@"<td>" + arcCount.ToString() + "</td>");
                                file.WriteLine(@"<td>" + blkCount.ToString() + "</td>");
                                file.WriteLine(@"<td>" + totalCount.ToString() + "</td>");
                                file.WriteLine(@"</tr>");
                                file.WriteLine(@"</table>");
                                file.WriteLine(@"</body>");
                                file.WriteLine(@"</html>");
                                break;
                        }
                    }
                }
                else
                {
                    editor.WriteMessage(@"\n" + titleMessage);
                    editor.WriteMessage(@"\nLines: " + lineCount.ToString());
                    editor.WriteMessage(@"\nMTexts: " + mtxCount.ToString());
                    editor.WriteMessage(@"\nPoylines: " + plCount.ToString());
                    editor.WriteMessage(@"\nArcs: " + arcCount.ToString());
                    editor.WriteMessage(@"\nBlocks: " + blkCount.ToString());
                    editor.WriteMessage(@"\nTotal Objects Count: " + totalCount.ToString());
                }
            }
            catch (System.Exception ex)
            {
                editor.WriteMessage(error + ex.Message);
            }
        }

        private int GetEntityCount(string entityType)
        {
            TypedValue[] tv = new TypedValue[1];
            tv.SetValue(new TypedValue((int)DxfCode.Start, entityType), 0);
            SelectionFilter filter = new SelectionFilter(tv);
            PromptSelectionResult ssPrompt = editor.SelectAll(filter);
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

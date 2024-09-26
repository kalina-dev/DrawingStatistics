using System.IO;
using System.Collections;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;

namespace DrawingStatistics
{
    public class BlockCountUtility
    {
        private const string keywordScreen = @"Screen";
        private const string keywordTXT = @"TXT";
        private const string keywordCSV = @"CSV";
        private const string keywordHTML = @"HTML";
        private const string objectBlock = @"INSERT";
        private const string error = @"Error encountered: ";
        private const string titleMessage = @"Number of Blocks found in the drawing: ";
        const string promptMessage = @"Select Block Statistics Display Mode: ";
        readonly Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
        readonly Document activeDocument = Application.DocumentManager.MdiActiveDocument;

        [CommandMethod("CountBlockAndShowStatistics")]
        public void CountBlockAndShowStatistics()
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
                DisplayOrWriteBlockStatistics(answer);
            }
            else
            {
                editor.WriteMessage(@"No response.");
            }
        }

        private void DisplayOrWriteBlockStatistics(string answer)
        {
            try
            {
                ArrayList result = GatherBlocksAndCounts();
                string filename = string.Empty;
                if (result != null)
                {
                    ArrayList arBlocks = new ArrayList();
                    int[] arCounts = null;
                    int i = 0;

                    arBlocks = (ArrayList)result[0];
                    arCounts = (int[])result[1];

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
                                    // Write the results to the text file
                                    file.WriteLine(titleMessage);

                                    foreach (string blockname in arBlocks)
                                    {
                                        file.WriteLine(@"\nBlock: " + blockname + " = " + arCounts[i]);
                                        i += 1;
                                    }
                                    break;
                                case keywordCSV:
                                    // Write the results to the scv file
                                    file.WriteLine(titleMessage);
                                    file.WriteLine(@"Block Name, Count");
                                    foreach (string blockname in arBlocks)
                                    {
                                        file.Write(@"\nBlock: " + blockname + "," + arCounts[i]);
                                        i += 1;
                                    }
                                    break;
                                case keywordHTML:
                                    // Write the results to the HTML file
                                    file.WriteLine(@"<html>");
                                    file.WriteLine(@"<head></head>");
                                    file.WriteLine(@"<body>");
                                    file.WriteLine(@"<h2 style='background-color:yellow'>Number of Blocks found in the drawing: </h2>");
                                    file.WriteLine(@"<table border=1>");
                                    file.WriteLine(@"<tr><td>Block Name</td><td>Count</td></tr>");
                                    foreach (string blockname in arBlocks)
                                    {
                                        file.Write(@"<tr><td>Block:" + blockname + " </td><td style='color:blue'>" + arCounts[i] + "</td></tr>");
                                        i += 1;
                                    }
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
                        foreach (string blockname in arBlocks)
                        {
                            editor.WriteMessage(@"\nBlock: " + blockname + " = " + arCounts[i]);
                            i += 1;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                editor.WriteMessage(error + ex.Message);
            }
        }

        private ArrayList GatherBlocksAndCounts()
        {
            ArrayList result = new ArrayList();
            Database database = activeDocument.Database;

            try
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    string blockName = "";
                    TypedValue[] tv = new TypedValue[1];
                    tv.SetValue(new TypedValue((int)DxfCode.Start, objectBlock), 0);
                    PromptSelectionResult selecttionResult = editor.SelectAll(new SelectionFilter(tv));

                    var blks = selecttionResult.Value.GetObjectIds();
                    ArrayList blkCol = new ArrayList();
                    int iCount = 0;
                    foreach (ObjectId brId in blks)
                    {
                        BlockReference br = (BlockReference)transaction.GetObject(brId, OpenMode.ForRead);
                        blockName = br.Name.ToString();

                        if (blkCol.IndexOf(blockName) == -1)
                        {
                            blkCol.Add(blockName);
                            iCount += 1;
                        }
                    }
                    blkCol.Sort();

                    int[] blkCount = new int[iCount];
                    int i;
                    foreach (ObjectId brId in blks)
                    {
                        BlockReference reference = (BlockReference)transaction.GetObject(brId, OpenMode.ForRead);
                        blockName = reference.Name.ToString();

                        for (i = 0; i <= iCount - 1; i++)
                        {
                            if (blkCol[i].ToString() == blockName)
                            {
                                blkCount[i] += 1;
                                break;
                            }
                        }
                    }

                    result.Add(blkCol);
                    result.Add(blkCount);
                }
            }
            catch (System.Exception ex)
            {
                editor.WriteMessage(error + ex.Message);
                return null;
            }
            return result;
        }
    }
}

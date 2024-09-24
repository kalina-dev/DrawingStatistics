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
        private const string keywordScreen = "Screen";
        private const string keywordTXT = "TXT";
        private const string keywordCSV = "CSV";
        private const string keywordHTML = "HTML";
        const string objectBlock = "INSERT";
        readonly string error = "Error encountered: ";
        readonly Editor edt = Application.DocumentManager.MdiActiveDocument.Editor;
        readonly Document doc = Application.DocumentManager.MdiActiveDocument;

        [CommandMethod("CountBlockAndShowStatistics")]
        public void CountBlockAndShowStatistics()
        {
            PromptKeywordOptions pko = new PromptKeywordOptions("Select Block Statisticts Display Mode: ");
            pko.Keywords.Add(keywordScreen);
            pko.Keywords.Add(keywordTXT);
            pko.Keywords.Add(keywordCSV);
            pko.Keywords.Add(keywordHTML);
            pko.AllowNone = false;

            PromptResult res = edt.GetKeywords(pko);
            string answer = res.StringResult;
            edt.WriteMessage("Your choice is " + answer);

            if (answer != null)
            {
                DisplayOrWriteBlockStatistics(answer);
            }
            else
            {
                edt.WriteMessage("No response.");
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
                        PromptStringOptions pso = new PromptStringOptions("Enter " + answer + " filename and its location (path) in the format C:\\Autodesk\\example.### where ### is the file extension. ");
                        PromptResult pr = edt.GetString(pso);
                        filename = pr.StringResult;

                        if (!HelperMethods.CheckFile(filename, edt, keywordCSV, keywordTXT, keywordHTML))
                        {
                            edt.WriteMessage(error);
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
                                    file.WriteLine("Number of Blocks found in the drawing: ");

                                    foreach (string blockname in arBlocks)
                                    {
                                        file.WriteLine("\nBlock: " + blockname + " = " + arCounts[i]);
                                        i += 1;
                                    }
                                    break;
                                case keywordCSV:
                                    // Write the results to the text file
                                    file.WriteLine("Number of Blocks found in the drawing: ");
                                    file.WriteLine("Block Name, Count");
                                    foreach (string blockname in arBlocks)
                                    {
                                        file.Write("\nBlock: " + blockname + "," + arCounts[i]);
                                        i += 1;
                                    }
                                    break;
                                case keywordHTML:
                                    // Write the results to the HTML file
                                    file.WriteLine("<html>");
                                    file.WriteLine("<head></head>");
                                    file.WriteLine("<body>");
                                    file.WriteLine("<h2 style='background-color:yellow'>List of Blocks found in the drawing: </h2>");
                                    file.WriteLine("<table border=1>");
                                    file.WriteLine("<tr><td>Block Name</td><td>Count</td></tr>");
                                    foreach (string blockname in arBlocks)
                                    {
                                        file.Write("<tr><td>Block:" + blockname + " </td><td style='color:blue'>" + arCounts[i] + "</td></tr>");
                                        i += 1;
                                    }
                                    file.WriteLine("</table>");
                                    file.WriteLine("</body>");
                                    file.WriteLine("</html>");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        edt.WriteMessage("\nNumber of Blocks found in the drawing: ");
                        foreach (string blockname in arBlocks)
                        {
                            edt.WriteMessage("\nBlock: " + blockname + " = " + arCounts[i]);
                            i += 1;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                edt.WriteMessage(error + ex.Message);
            }
        }

        private ArrayList GatherBlocksAndCounts()
        {
            ArrayList result = new ArrayList();
            Database db = doc.Database;

            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    string blockName = "";
                    // Get all the Blocks
                    TypedValue[] tv = new TypedValue[1];
                    tv.SetValue(new TypedValue((int)DxfCode.Start, objectBlock), 0);
                    PromptSelectionResult psr = edt.SelectAll(new SelectionFilter(tv));

                    var blks = psr.Value.GetObjectIds();
                    ArrayList blkCol = new ArrayList();
                    int iCount = 0;
                    foreach (ObjectId brId in blks)
                    {
                        BlockReference br = (BlockReference)trans.GetObject(brId, OpenMode.ForRead);
                        blockName = br.Name.ToString();

                        if (blkCol.IndexOf(blockName) == -1)
                        {
                            blkCol.Add(blockName);
                            iCount += 1;
                        }
                    }
                    blkCol.Sort();

                    // Loop through the Block Collection and count the number of each block and store in the array
                    int[] blkCount = new int[iCount];
                    int i;
                    foreach (ObjectId brId in blks)
                    {
                        BlockReference br = (BlockReference)trans.GetObject(brId, OpenMode.ForRead);
                        blockName = br.Name.ToString();

                        for (i = 0; i <= iCount - 1; i++)
                        {
                            if (blkCol[i].ToString() == blockName)
                            {
                                blkCount[i] += 1;
                                break;
                            }
                        }
                    }

                    // Gather the results and return
                    result.Add(blkCol);
                    result.Add(blkCount);
                }
            }
            catch (System.Exception ex)
            {
                edt.WriteMessage(error + ex.Message);
                return null;
            }
            return result;
        }
    }
}

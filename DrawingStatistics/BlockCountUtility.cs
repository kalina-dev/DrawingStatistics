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
        private readonly string error = string.Empty;
        Editor edt;
        Document doc;
        Database db;
        public BlockCountUtility()
        {
            error = "Error encountered: ";
            doc = Application.DocumentManager.MdiActiveDocument;
            db = doc.Database;
            edt = Application.DocumentManager.MdiActiveDocument.Editor;
        }

        [CommandMethod("CountBlock")]
        public void CountBlock()
        {
            PromptKeywordOptions pko = new PromptKeywordOptions("Select Display Mode: ");
            pko.Keywords.Add("Screen");
            pko.Keywords.Add("TXT");
            pko.Keywords.Add("CSV");
            pko.Keywords.Add("HTML");
            pko.AllowNone = true;

            PromptResult res = edt.GetKeywords(pko);
            string answer = res.StringResult;

            switch (answer)
            {
                case "Screen":
                    DisplayBlockCountOnScreen();
                    break;
                case "TXT":
                    WriteBlockCountToTextFile();
                    break;
                case "CSV":
                    WriteBlockCountToCSVFile();
                    break;
                case "HTML":
                    WriteBlockCountToHTMLFile();
                    break;
            }
        }

        private void DisplayBlockCountOnScreen()
        {
            try
            {
                // Get all the Blocks
                var result = GatherBlocksAndCounts();
                if (result != null)
                {                    
                    ArrayList arBlocks = new ArrayList();
                    int[] arCounts = null;
                    int i = 0;

                    arBlocks = (ArrayList) result[0];
                    arCounts = (int[]) result[1];

                    // Display the results
                    edt.WriteMessage("\nList of Blocks found in the drawing: ");
                    foreach (string blockname in arBlocks)
                    {
                        edt.WriteMessage("\nBlock: " + blockname + " = " + arCounts[i]);
                        i += 1;
                    }
                }
            }
            catch (System.Exception ex)
            {
                edt.WriteMessage("Error encountered: " + ex.Message);
            }
        }

        private void WriteBlockCountToTextFile()
        {
            try
            {
                ArrayList result = GatherBlocksAndCounts();
                if (result != null)
                {
                    ArrayList arBlocks = new ArrayList();
                    int[] arCounts = null;
                    int i = 0;

                    arBlocks = (ArrayList)result[0];
                    arCounts = (int[])result[1];

                    PromptStringOptions pso = new PromptStringOptions("Enter TXT filename and location: ");
                    PromptResult pr = edt.GetString(pso);
                    string filename = pr.StringResult;

                    if (filename != "")
                    {
                        using (StreamWriter file = new StreamWriter(filename))
                        {
                            // Write the results to the text file
                            file.WriteLine("\nList of Blocks found in the drawing: ");

                            foreach (string blockname in arBlocks)
                            {
                                file.WriteLine("\nBlock: " + blockname + " = " + arCounts[i]);
                                i += 1;
                            }                            
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                edt.WriteMessage("Error encountered: " + ex.Message);
            }
        }

        private void WriteBlockCountToCSVFile()
        {
            try
            {
                ArrayList result = GatherBlocksAndCounts();
                if (result != null)
                {
                    ArrayList arBlocks = new ArrayList();
                    int[] arCounts = null;
                    int i = 0;

                    arBlocks = (ArrayList)result[0];
                    arCounts = (int[])result[1];

                    PromptStringOptions pso = new PromptStringOptions("Enter CSV filename and location: ");
                    PromptResult pr = edt.GetString(pso);
                    string filename = pr.StringResult;

                    if (filename != "")
                    {
                        using (StreamWriter file = new StreamWriter(filename))
                        {
                            // Write the results to the text file
                            file.WriteLine("List of Blocks found in the drawing: ");
                            file.WriteLine("Block Name, Count");
                            foreach (string blockname in arBlocks)
                            {
                                file.Write("\nBlock: " + blockname + "," + arCounts[i]);
                                i += 1;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                edt.WriteMessage("Error encountered: " + ex.Message);
            }
        }

        private void WriteBlockCountToHTMLFile()
        {
            try
            {
                ArrayList result = GatherBlocksAndCounts();
                if (result != null)
                {
                    ArrayList arBlocks = new ArrayList();
                    int[] arCounts = null;
                    int i = 0;

                    arBlocks = (ArrayList)result[0];
                    arCounts = (int[])result[1];

                    PromptStringOptions pso = new PromptStringOptions("Enter HTML filename and location: ");
                    PromptResult pr = edt.GetString(pso);
                    string filename = pr.StringResult;

                    if (filename != "")
                    {
                        using (StreamWriter file = new StreamWriter(filename))
                        {
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
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                edt.WriteMessage("Error encountered: " + ex.Message);
            }
        }

        private ArrayList GatherBlocksAndCounts()
        {
            ArrayList result = new ArrayList();

            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    // Get all the Blocks
                    TypedValue[] tv = new TypedValue[1];
                    tv.SetValue(new TypedValue((int)DxfCode.Start, "INSERT"), 0);
                    PromptSelectionResult psr = edt.SelectAll(new SelectionFilter(tv));

                    // Get all the unique Blocks and store in an ArrayList collection then sort the array
                    string blockName = "";
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
                edt.WriteMessage("Error encountered: " + ex.Message);
                return null;
            }
            return result;
        }
    }
}

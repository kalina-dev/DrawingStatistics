using Autodesk.AutoCAD.EditorInput;
using System.IO;

namespace DrawingStatistics
{
    public static class HelperMethods
    {
        public static bool CheckFile(string filename, Editor edt, string keywordCSV, string keywordTXT, string keywordHTML)
        {
            string path = @"C:\Autodesk";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string[] fileDetails = filename.Split('\\');
            if ((fileDetails[0] + fileDetails[1]) != path && fileDetails.Length > 3)
            {
                edt.WriteMessage("Incorrect file location");
                return false;
            }

            if (!fileDetails[3].Contains("."))
            {
                edt.WriteMessage("Incorrect windows file");
                return false;
            }

            string[] fileFormat = fileDetails[3].Split('.');
            if (fileFormat[1].Trim() != keywordCSV || fileFormat[1].Trim() != keywordTXT || fileFormat[1].Trim() != keywordHTML)
            {

                edt.WriteMessage("Incorrect file extension");
                return false;
            }

            return true;
        }
    }
}

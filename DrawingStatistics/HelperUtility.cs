using Autodesk.AutoCAD.EditorInput;
using System;
using System.IO;

namespace DrawingStatistics
{
    internal static class HelperUtility
    {
        const string path = @"C:\Autodesk";
        const string messageInvalidLocation = @"Incorrect file location";
        const string messageInvalidFile = @"Incorrect windows file";
        const string messageFileExtension = @"Incorrect file extension";
        public static bool CheckFile(Editor editor, params string[] keywordArray)
        {    
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string[] fileDetails = keywordArray[0].Split('\\');
            if ((keywordArray[0] + "\\" + keywordArray[1]) != path && fileDetails.Length > 3)
            {
                editor.WriteMessage(messageInvalidLocation);
                return false;
            }

            if (!fileDetails[2].Contains("."))
            {
                editor.WriteMessage(messageInvalidFile);
                return false;
            }

            string[] fileFormat = fileDetails[2].Split('.');
            if (!Array.Exists(keywordArray, element => element == fileFormat[1].Trim().ToUpper()))
            {
                editor.WriteMessage(messageFileExtension);
                return false;
            }

            return true;
        }
    }
}

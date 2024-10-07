using AutoCAD.SQL.Plugin;
using AutocadSQLPlugin;
using Autodesk.AutoCAD.Runtime;
using Autocad = Autodesk.AutoCAD.ApplicationServices;
using AutocadApp = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(EntryCommand))]

namespace AutocadSQLPlugin
{
    public class EntryCommand
    {
        [CommandMethod("ConnectDb")]
        public static void ConnectDb()
        {
            Autocad.Document? activeDocument = AutocadApp.DocumentManager.MdiActiveDocument;
            if (activeDocument is null)
            {
                return;
            }
            var editor = activeDocument.Editor;
            using var data = new DatabaseManager();
            try
            {

                editor.WriteMessage("\nConnected to SQL Server database successfully!");
                if (data.TestSqlServerConnection())
                {
                    var form = new Main();
                    form.ShowDialog();
                }

            }
            catch (System.Exception ex)
            {
                editor.WriteMessage($"\nConnecting to SQL Server database failed!\n{ex.Message}");
            }
        }
    }
}

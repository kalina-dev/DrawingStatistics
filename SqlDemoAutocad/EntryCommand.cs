using AutoCAD.SQL.Plugin;
using AutocadSQLPlugin;
using Autodesk.AutoCAD.Runtime;

[assembly: CommandClass(typeof(EntryCommand))]

namespace AutocadSQLPlugin
{
    public class EntryCommand
    {
        [CommandMethod("ConnectDb")]
        public static void ConnectDb()
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            if (doc is null)
            {
                return;
            }
            var ed = doc.Editor;
            using var data = new DatabaseManager();
            try
            {
                data.TestSqlServerConnection();
                ed.WriteMessage("\nConnected to SQL Server database successfully!");
                var form = new Main();
                form.ShowDialog();

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage($"\nConnecting to SQL Server database failed!\n{ex.Message}");
            }
        }
    }
}

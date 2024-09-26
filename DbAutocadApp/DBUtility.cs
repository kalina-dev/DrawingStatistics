using Autodesk.AutoCAD.Runtime;
using DbAutocadApp;
using System.Data.SqlClient;

namespace DbAutocadDemoNemetschek
{
    internal static class DBUtility
    {
        [CommandMethod("DBRun")]
        public static void DBRun()
        {
            Main main = new Main();
            main.ShowDialog();
        }
        public static SqlConnection GetConnection()
        {
            string connStr = Settings.Default.connstr;
            SqlConnection conn = new SqlConnection(connStr);
            return conn;
        }        
    }
}

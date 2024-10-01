using Microsoft.Data.SqlClient;

namespace AutoCAD.SQL.Plugin
{
    public class DatabaseManager : IDisposable
    {
        private readonly string? _connectionString;
        private bool _disposedValue;

        public DatabaseManager()
        {
            _connectionString = "Server=STZ-LAP-ETM001\\SQLEXPRESS;Database=AUTOCADDB;Integrated Security=True;TrustServerCertificate=Yes";
        }

        public bool TestSqlServerConnection()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Cannot connect to Database server: {ex.Message}");
                return false;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    //nothing to dispose, connection is disposed in using block
                }
                _disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

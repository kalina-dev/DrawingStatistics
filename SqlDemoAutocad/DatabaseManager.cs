﻿using AutoCAD.SQL.Plugin;
using Microsoft.Data.SqlClient;

namespace AutocadSQLPlugin
{
    public class DatabaseManager : IDisposable
    {
        private readonly string? _connectionString;
        private bool _disposedValue;

        public DatabaseManager()
        {
            _connectionString = SettingsDb.Default.connectionString;
        }

        public bool TestSqlServerConnection()
        {
            try
            {
                using SqlConnection connection = new(_connectionString);
                connection.Open();
                return true;
            }
            catch (Exception ex)
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

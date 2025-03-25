using System.Data.SqlClient;
using Dapper;
using Domain.Interfaces;
using DotNetEnv;

namespace Infrastructure.DatabaseInitializer
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly IDataProcessor _dataProcessor;

        public DatabaseInitializer(IDataProcessor dataProcessor)
        {
            Env.Load();
            _connectionString = GetEnvVar("CONNECTION_STRING");

            var builder = new SqlConnectionStringBuilder(_connectionString);
            _databaseName = builder.InitialCatalog ??
                            throw new Exception("Database name is missing in the connection string.");

            _dataProcessor = dataProcessor;
        }

        public void EnsureDatabaseExists()
        {
            try
            {
                if (DatabaseExists()) return;

                CreateDatabase();
                ExecuteSqlScripts();
                _dataProcessor.ProcessCsv();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database initialization: {ex.Message}");
                throw;
            }
        }

        private bool DatabaseExists()
        {
            try
            {
                using var connection = new SqlConnection(GetMasterConnectionString());
                connection.Open();

                string checkDbQuery = "SELECT 1 FROM sys.databases WHERE name = @dbName";
                return connection.ExecuteScalar<int?>(checkDbQuery, new { dbName = _databaseName }) != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if database exists: {ex.Message}");
                throw;
            }
        }

        private void CreateDatabase()
        {
            try
            {
                using var connection = new SqlConnection(GetMasterConnectionString());
                connection.Open();

                string createDbQuery = $"CREATE DATABASE [{_databaseName}]";
                connection.Execute(createDbQuery);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating database: {ex.Message}");
                throw;
            }
        }

        private void ExecuteSqlScripts()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                string scriptsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Infrastructure",
                    "DatabaseInitializer", "DatabaseScripts");
                ExecuteScriptIfExists(connection, Path.Combine(scriptsPath, "TableCreation.sql"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing SQL scripts: {ex.Message}");
                throw;
            }
        }

        private void ExecuteScriptIfExists(SqlConnection connection, string scriptPath,
            DynamicParameters? parameters = null)
        {
            try
            {
                if (!File.Exists(scriptPath))
                    throw new InvalidOperationException("Can't find file at " + scriptPath);

                string scriptContent = File.ReadAllText(scriptPath);
                connection.Execute(scriptContent, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing script {scriptPath}: {ex.Message}");
                throw;
            }
        }

        private string GetEnvVar(string key) =>
            Environment.GetEnvironmentVariable(key)
            ?? throw new InvalidOperationException($"Can't find {key} in .env");

        private string GetMasterConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(_connectionString) { InitialCatalog = "master" };
            return builder.ConnectionString;
        }
    }
}
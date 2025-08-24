using Dapper;
using Flashcards.Config;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Flashcards.Database;
public sealed class AppDatabase
{
    private readonly string _connectionString;

    private static readonly Lazy<AppDatabase> _instance = new Lazy<AppDatabase>(() =>
    {
        string? connStr = AppConfig.Instance.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connStr))
            throw new InvalidOperationException("Missing 'DefaultConnection' in configuration.");

        return new AppDatabase(connStr);
    });

    public static AppDatabase Instance => _instance.Value;

    private AppDatabase(string connectionString)
    {
        _connectionString = connectionString;
        DebugDropTables();
        InitializeDatabase();
    }

    public IDbConnection GetConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }

    public void InitializeDatabase()
    {
        using (var connection = GetConnection())
        {
            string sql = @"

                IF OBJECT_ID('dbo.Stacks', 'U') IS NULL
                CREATE TABLE Stacks (
                    Id INT PRIMARY KEY IDENTITY,
                    Name NVARCHAR(255) UNIQUE,
                );

                IF OBJECT_ID('dbo.Cards', 'U') IS NULL
                CREATE TABLE Cards (
                    Id INT PRIMARY KEY IDENTITY,
                    Front NVARCHAR(255),
                    Back NVARCHAR(255),
                    StackId INT NOT NULL 
                        FOREIGN KEY REFERENCES Stacks(Id) 
                        ON DELETE CASCADE
                        ON UPDATE CASCADE
                );

                IF OBJECT_ID('dbo.StudySessions', 'U') IS NULL
                CREATE TABLE StudySessions (
                    Id INT PRIMARY KEY IDENTITY,
                    SessionTime DATETIME,
                    Score DECIMAL(5,2),
                    StackId INT NOT NULL 
                        FOREIGN KEY REFERENCES Stacks(Id) 
                        ON DELETE CASCADE
                        ON UPDATE CASCADE
                );";

            connection.Execute(sql);
        }
    }

    public void DebugDropTables()
    {
        using (var connection = GetConnection())
        {
            string sql = @"
                IF OBJECT_ID('dbo.Cards', 'U') IS NOT NULL
                    DROP TABLE dbo.Cards;

                IF OBJECT_ID('dbo.StudySessions', 'U') IS NOT NULL
                    DROP TABLE dbo.StudySessions;

                IF OBJECT_ID('dbo.Stacks', 'U') IS NOT NULL
                    DROP TABLE dbo.Stacks;";

            connection.Execute(sql);
        }
    }
}

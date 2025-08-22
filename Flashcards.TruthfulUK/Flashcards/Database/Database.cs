using Dapper;
using Flashcards.Config;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Flashcards.Database;
public sealed class Database
{
    private readonly string _connectionString;

    private static readonly Lazy<Database> _instance = new Lazy<Database>(() =>
    {
        string? connStr = AppConfig.Instance.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connStr))
            throw new InvalidOperationException("Missing 'DefaultConnection' in configuration.");

        return new Database(connStr);
    });

    public static Database Instance => _instance.Value;

    private Database(string connectionString)
    {
        _connectionString = connectionString;
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
                );";

            connection.Execute(sql);
        }
    }

    public List<Card> SelectCardsFromStackId(int stackId)
    {
        using (var connection = GetConnection())
        {
            var parameters = new { StackId = stackId };
            string sql = @"
                SELECT * From Cards
                WHERE StackId = @StackId";
            return connection.Query<Card>(sql, parameters).ToList();
        }
    }

    public List<Stack> SelectAllStacks()
    {
        using (var connection = GetConnection())
        {
            string sql = @"SELECT * FROM Stacks";
            return connection.Query<Stack>(sql).ToList();
        }
    }

    public void DebugDropTables()
    {
        using (var connection = GetConnection())
        {
            string sql = @"
                IF OBJECT_ID('dbo.Cards', 'U') IS NOT NULL
                    DROP TABLE dbo.Cards;

                IF OBJECT_ID('dbo.Stacks', 'U') IS NOT NULL
                    DROP TABLE dbo.Stacks;";

            connection.Execute(sql);
        }
    }
}

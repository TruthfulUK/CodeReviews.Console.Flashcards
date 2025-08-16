using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Flashcards.Models;

namespace Flashcards.Database;
public class Database
{
    private readonly string _connectionString;

    public Database(string connectionString)
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

using Dapper;
using Flashcards.Database;
using System;
using System.Collections.Generic;
using System.Data;

public class SeedData
{
    private readonly Database _database;

    public SeedData(Database database)
    {
        _database = database;
    }

    public void InsertSeedData()
    {
        using var connection = _database.GetConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            var stacks = new List<(string Name, List<(string Front, string Back)> Cards)>
            {
                ("C# Basics", new List<(string, string)>
                {
                    ("Keyword to define a class?", "class"),
                    ("Access modifier for public visibility?", "public"),
                    ("Type-safe loop keyword?", "foreach"),
                    ("Entry point method in C#?", "Main"),
                    ("Symbol used for inheritance?", ":")
                }),
                ("SQL Essentials", new List<(string, string)>
                {
                    ("Keyword to retrieve data?", "SELECT"),
                    ("Clause to filter rows?", "WHERE"),
                    ("Clause to sort results?", "ORDER BY"),
                    ("Keyword to join tables?", "JOIN"),
                    ("Keyword to remove duplicates?", "DISTINCT")
                })
            };

            foreach (var (stackName, cards) in stacks)
            {
                var stackId = connection.QuerySingle<int>(
                    @"INSERT INTO Stacks (Name) VALUES (@Name);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    new { Name = stackName }, transaction);

                foreach (var (front, back) in cards)
                {
                    connection.Execute(
                        @"INSERT INTO Cards (Front, Back, StackId)
                          VALUES (@Front, @Back, @StackId);",
                        new { Front = front, Back = back, StackId = stackId },
                        transaction);
                }
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}

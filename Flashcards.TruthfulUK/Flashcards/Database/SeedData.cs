using Dapper;
using Flashcards.Database;
using Flashcards.Models;

public class SeedData
{
    private AppDatabase _database;

    public SeedData()
    {
        _database = AppDatabase.Instance;
    }

    public void InsertSeedData()
    {
        using var connection = _database.GetConnection();

        var existingCount = connection.ExecuteScalar<int>("SELECT COUNT(1) FROM Stacks");
        if (existingCount > 0) return;

        var stacks = new List<Stack>
        {
            new Stack { Name = "CSharp" },
            new Stack { Name = "SQL" }
        };

        foreach (var stack in stacks)
        {
            var insertStackSql = "INSERT INTO Stacks (Name) OUTPUT INSERTED.Id VALUES (@Name)";
            stack.Id = connection.ExecuteScalar<int>(insertStackSql, stack);

            var cards = GetCardsForStack(stack.Name);
            foreach (var card in cards)
            {
                var insertCardSql = "INSERT INTO Cards (Front, Back, StackId) VALUES (@Front, @Back, @StackId)";
                connection.Execute(insertCardSql, new { card.Front, card.Back, StackId = stack.Id });
            }
        }
    }

    private List<Card> GetCardsForStack(string stackName)
    {
        return stackName switch
        {
            "CSharp" => new List<Card>
        {
            new Card { Front = "Keyword to define a class", Back = "class" },
            new Card { Front = "Keyword to create object", Back = "new" },
            new Card { Front = "Namespace for collections", Back = "System.Collections" },
            new Card { Front = "Access modifier for inheritance", Back = "protected" },
            new Card { Front = "Base type of all types", Back = "object" }
        },
            "SQL" => new List<Card>
        {
            new Card { Front = "Keyword to select data", Back = "SELECT" },
            new Card { Front = "Clause to filter rows", Back = "WHERE" },
            new Card { Front = "Command to add data", Back = "INSERT" },
            new Card { Front = "Command to remove table", Back = "DROP" },
            new Card { Front = "Function to count rows", Back = "COUNT" }
        },
            _ => new List<Card>()
        };
    }
}

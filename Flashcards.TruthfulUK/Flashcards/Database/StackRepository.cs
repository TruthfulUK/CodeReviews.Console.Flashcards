using Flashcards.Models;
using Dapper;

namespace Flashcards.Database;
internal class StackRepository
{
    private readonly AppDatabase _db = AppDatabase.Instance;

    public List<Stack> SelectAllStacks()
    {
        using (var connection = _db.GetConnection())
        {
            string sql = @"SELECT * FROM Stacks";
            return connection.Query<Stack>(sql).ToList();
        }
    }
}

using Flashcards.Models;
using Dapper;

namespace Flashcards.Database;
internal class CardRepository
{
    private readonly AppDatabase _db = AppDatabase.Instance;

    public List<Card> GetAllCardsByStackId(int stackId)
    {
        using (var connection = _db.GetConnection())
        {
            var parameters = new { StackId = stackId };
            string sql = @"
                SELECT * From Cards
                WHERE StackId = @StackId";
            return connection.Query<Card>(sql, parameters).ToList();
        }
    }
}

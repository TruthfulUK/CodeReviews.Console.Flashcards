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

    public void Insert(int stackId, string front, string back)
    {
        using (var connection = _db.GetConnection())
        {
            var parameters = new { StackId = stackId, Front = front, Back = back };
            string sql = @"
            INSERT INTO Cards (StackId, Front, Back)
            VALUES (@StackId, @Front, @Back)";
            connection.Execute(sql, parameters);
        }
    }

    public void Delete(int rowId)
    {
        using (var connection = _db.GetConnection())
        {
            var parameters = new { RowId = rowId };
            var sql = @"
                DELETE FROM Cards
                WHERE Id = @RowId";
            connection.Execute(sql, parameters);
        }
    }
}

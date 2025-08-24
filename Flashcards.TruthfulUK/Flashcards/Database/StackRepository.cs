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

    public string SelectStackById(int stackId)
    {
        using (var connection = _db.GetConnection())
        {
            var parameters = new { StackId = stackId };
            string sql = @"
                SELECT Name From Stacks
                WHERE Id = @StackId";
            return connection.QuerySingle<string>(sql, parameters);
        }
    }

    public void Insert(string stackName)
    {
        using (var connection = _db.GetConnection())
        {
            var parameters = new { Name = stackName };
            string sql = @"
            INSERT INTO Stacks (Name)
            VALUES (@Name)";
            connection.Execute(sql, parameters);
        }
    }

    public void Delete(int rowId)
    {
        using (var connection = _db.GetConnection())
        {
            var parameters = new { RowId = rowId };
            var sql = @"
                DELETE FROM Stacks
                WHERE Id = @RowId";
            connection.Execute(sql, parameters);
        }
    }

    public int GetCardCountForStack(int stackId)
    {
        using (var connection = _db.GetConnection())
        {
            var parameters = new { StackId = stackId };
            string sql = @"SELECT COUNT(*) FROM Cards WHERE StackId = @StackId";
            return connection.ExecuteScalar<int>(sql, parameters);
        }
    }
}

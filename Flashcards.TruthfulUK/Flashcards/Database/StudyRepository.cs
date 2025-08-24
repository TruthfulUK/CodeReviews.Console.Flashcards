using Flashcards.Models;
using Dapper;

namespace Flashcards.Database;
internal class StudyRepository
{
    private readonly AppDatabase _db = AppDatabase.Instance;

    public void Insert(decimal score, int stackId)
    {
        using (var connection = _db.GetConnection())
        {
            var parameters = new { Score = score, StackId = stackId };
            string sql = @"
            INSERT INTO StudySessions (SessionTime, Score, StackId)
            VALUES (GETDATE(), @Score, @StackId)";
            connection.Execute(sql, parameters);
        }
    }

    public List<StudySession> GetAllStudySessionsByStackId(int stackId)
    {
        using (var connection = _db.GetConnection())
        {
            var parameters = new { StackId = stackId };
            string sql = @"
                SELECT * From StudySessions
                WHERE StackId = @StackId";
            return connection.Query<StudySession>(sql, parameters).ToList();
        }
    }
}

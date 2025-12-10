using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgentActivitiesTracker;

public class CaseService
{
    private readonly IMongoCollection<Case> _cases;

    public CaseService()
    {
        _cases = AppState.Db.GetCollection<Case>("cases");
    }

    public async Task<List<Case>> GetCasesByAgentOrSupervisorAsync(string id)
    {
        var filter = Builders<Case>.Filter.Or(
            Builders<Case>.Filter.Eq(c => c.AgentId, id),
            Builders<Case>.Filter.Eq(c => c.SupervisorId, id)
        );

        return await _cases.Find(filter).ToListAsync();
    }
}

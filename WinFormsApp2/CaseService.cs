using AgentActivitiesTracker;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CaseService
{
    private readonly IMongoCollection<Case> _cases;

    public CaseService()
    {
        _cases = AppState.Db.GetCollection<Case>("cases");
    }

    public async Task<bool> ArchiveAndDeleteCaseAsync(string caseId)
    {
        var db = AppState.Db;

        var cases = db.GetCollection<BsonDocument>("cases");
        var casesArchive = db.GetCollection<BsonDocument>("cases_archive");
        var actions = db.GetCollection<BsonDocument>("actions");
        var actionsArchive = db.GetCollection<BsonDocument>("actions_archive");

        var filter = Builders<BsonDocument>.Filter.Eq("case_id", caseId);

        var caseDoc = await cases.Find(filter).FirstOrDefaultAsync();
        if (caseDoc == null)
            return false;

        string status = caseDoc.GetValue("status", "").ToString().ToLower();
        if (status != "closed")
            throw new InvalidOperationException("Only closed cases can be archived.");

        caseDoc["archived_at"] = DateTime.UtcNow;
        await casesArchive.InsertOneAsync(caseDoc);

        var caseActions = await actions.Find(filter).ToListAsync();
        foreach (var act in caseActions)
        {
            act["archived_at"] = DateTime.UtcNow;
            await actionsArchive.InsertOneAsync(act);
        }

        await cases.DeleteOneAsync(filter);
        await actions.DeleteManyAsync(filter);

        return true;
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

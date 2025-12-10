using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


[BsonIgnoreExtraElements]
public class Case
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("case_id")]
    public string CaseId { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("priority")]
    public string Priority { get; set; }

    [BsonElement("status")]
    public string Status { get; set; }

    [BsonElement("case_origin")]
    public string CaseOrigin { get; set; }

    [BsonElement("creation_date")]
    public DateTime CreationDate { get; set; }

    [BsonElement("tags")]
    public List<string> Tags { get; set; }

    [BsonElement("client_id")]
    public string ClientId { get; set; }

    [BsonElement("agent_id")]
    public string AgentId { get; set; }

    [BsonElement("supervisor_id")]
    public string SupervisorId { get; set; }

    [BsonElement("actions")]
    public List<string> Actions { get; set; }
}

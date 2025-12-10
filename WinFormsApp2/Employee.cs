using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgentActivitiesTracker
{
    [BsonIgnoreExtraElements]
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("employee_id")]
        public string employee_id { get; set; }

        [BsonElement("first_name")]
        public string first_name { get; set; }

        [BsonElement("last_name")]
        public string last_name { get; set; }

        [BsonElement("phone_number")]
        public string phone_number { get; set; }

        [BsonElement("email")]
        public string email { get; set; }

        [BsonElement("role")]
        public string role { get; set; }

        [BsonElement("password")]
        public string password { get; set; }
    }
}

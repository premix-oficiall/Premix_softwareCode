using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Premix.Models;

public class Gestor
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } 

    [BsonElement("Username")] 
    public string Username { get; set; } = string.Empty;

    [BsonElement("Password")] 
    public string Password { get; set; } = string.Empty;
    

}
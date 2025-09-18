using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Premix.Models;

public class Func
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] // Permite serializar como string se precisar
    public ObjectId Id { get; set; }

    [BsonElement("cpf")]
    public long cpf { get; set; } 

    [BsonElement("senha")]
    public string senha { get; set; } = string.Empty;
    
    [BsonElement("usuario")]
    public string usuario { get; set; } = string.Empty;
}
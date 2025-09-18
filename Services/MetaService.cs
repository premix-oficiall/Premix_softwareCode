using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Premix.Models;

namespace Premix.Services;

public class MetaService
{
    private readonly IMongoCollection<Meta> _meta;

    private readonly MongoDbService _dbService;

    public MetaService()
    {
        _dbService = new MongoDbService();
        
        var client = new MongoClient("mongodb+srv://Fabricio:premix@dbpremix.4c9gbkl.mongodb.net/?retryWrites=true&w=majority&appName=dbPremix");
        var database = client.GetDatabase("dbPremix");
        _meta = database.GetCollection<Meta>("metas");
    }
    
    public async Task<List<Meta>> GetByMetaAsync(string nome)
    {
        var filter = Builders<Meta>.Filter.Eq(m => m.Nome, nome);
        return await _meta.Find(filter).ToListAsync();
    }
    
    public async Task AddAsync(Meta meta)
    {
        await _meta.InsertOneAsync(meta);
    }
    
    public async Task UpdateAsync(Meta meta)
    {
        await _meta.ReplaceOneAsync(m => m.Id == meta.Id, meta);
    }
    
    public async Task DeleteAsync(string id)
    {
        await _meta.DeleteOneAsync(m => m.Id == id);
    }

    public async Task<List<Meta>> GetByUsuarioAsync(string usuarioId)
    {
        var filter = Builders<Meta>.Filter.Eq(m => m.UsuarioId, usuarioId);
        return await _meta.Find(filter).ToListAsync();
    }
    
    public async Task<bool> NomeMetaExisteAsync(string nome)
    {
        // Aqui depende do seu repositório / ORM (Mongo, EF Core, etc).
        // Exemplo genérico para Mongo:
        return await _meta
            .Find(m => m.Nome == nome)
            .AnyAsync();
    }
}
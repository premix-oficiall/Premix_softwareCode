using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Premix.Models;
using Premix.ViewModels;

namespace Premix.Services;

public class DepartamentoService
{
    private readonly IMongoCollection<Departamentos> _departamentos;

    
    private readonly MongoDbService _dbService;

    public DepartamentoService()
    {
        _dbService = new MongoDbService();
        
        var client = new MongoClient("mongodb+srv://Fabricio:premix@dbpremix.4c9gbkl.mongodb.net/?retryWrites=true&w=majority&appName=dbPremix");
        var database = client.GetDatabase("dbPremix");
        _departamentos = database.GetCollection<Departamentos>("Departamentos");
        
    }
    
    public async Task<List<Departamentos>> GetAllAsync()
    {
        return await _departamentos.Find(_ => true).ToListAsync();
    }

    public async Task AddAsync(Departamentos dep)
    {
        await _departamentos.InsertOneAsync(dep);
    }

    public async Task UpdateAsync(Departamentos dep)
    {
        await _departamentos.ReplaceOneAsync(d => d.Id == dep.Id, dep);
    }

    public async Task DeleteAsync(string id)
    {
        await _departamentos.DeleteOneAsync(d => d.Id == id);
    }
    
    public async Task<List<Departamentos>> GetByUsuarioAsync(string usuarioId)
    {
        var filter = Builders<Departamentos>.Filter.Eq(d => d.UsuarioId, usuarioId);
        return await _departamentos.Find(filter).ToListAsync();
    }
    
    public async Task<Departamentos?> GetByCodigoAsync(string codigo)
    {
        return await _departamentos
            .Find(d => d.Codigo == codigo && d.IsActive == true)
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> CodigoExisteAsync(string codigo)
    {
        // Aqui depende do seu repositório / ORM (Mongo, EF Core, etc).
        // Exemplo genérico para Mongo:
        return await _departamentos
            .Find(d => d.Codigo.ToLower() == codigo.ToLower())
            .AnyAsync();
    }
    
    public async Task<bool> NomeExisteAsync(string nome)
    {
        // Aqui depende do seu repositório / ORM (Mongo, EF Core, etc).
        // Exemplo genérico para Mongo:
        return await _departamentos
            .Find(d => d.Nome == nome)
            .AnyAsync();
    }
    
    public async Task<Departamentos?> GetByCodigoAndUsuarioAsync(string codigo, string usuarioId) // SO DEIXA CERTO USUARIO ENTRAR NO DEPARTAMENTO;
    {
        return await _departamentos
            .Find(d => d.Codigo == codigo && d.UsuarioId == usuarioId)
            .FirstOrDefaultAsync();
    }
    
    
    
}
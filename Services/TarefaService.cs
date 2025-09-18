using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Premix.Models;
using Premix.ViewModels;

namespace Premix.Services;

public class TarefaService
{
    private readonly IMongoCollection<Tarefa> _tarefa;
    private readonly IMongoCollection<Departamentos> _departamento;
    private readonly IMongoCollection<Func> _funcionario;
    
    private readonly MongoDbService _dbService;
    
    public TarefaService()
    {
        _dbService = new MongoDbService();
        
        var client = new MongoClient("mongodb+srv://Fabricio:premix@dbpremix.4c9gbkl.mongodb.net/?retryWrites=true&w=majority&appName=dbPremix");
        var database = client.GetDatabase("dbPremix");
        _tarefa = database.GetCollection<Tarefa>("tarefas");
        _departamento = database.GetCollection<Departamentos>("Departamentos");
        _funcionario = database.GetCollection<Func>("funcionarios");
    }
    
    public async Task<List<Tarefa>> GetByUsuarioAsync(ObjectId usuarioId)
    {
        var filter = Builders<Tarefa>.Filter.Eq(t => t.UsuarioId, usuarioId);
        return await _tarefa.Find(filter).ToListAsync();
    }

    public async Task<List<string>> GetNomesFuncionariosAsync()
    {
        return await _funcionario
            .Find(Builders<Func>.Filter.Empty)
            .Project(d => d.usuario)
            .ToListAsync();
    }
    
    public async Task<List<string>> GetNomesDepartamentosAsync()
    {
        return await _departamento
            .Find(Builders<Departamentos>.Filter.Empty)
            .Project(d => d.Nome)
            .ToListAsync();
    
    }
    
    public async Task<List<Tarefa>> GetByTarefaAsync(string nome)
    {
        var filter = Builders<Tarefa>.Filter.Eq(t => t.Nome, nome);
        return await _tarefa.Find(filter).ToListAsync();
    }
    
    public async Task CriarTarefaAsync(Tarefa tarefa)
    {
        await _tarefa.InsertOneAsync(tarefa);
    }
    
    public async Task AddAsync(Tarefa tar)
    {
        await _tarefa.InsertOneAsync(tar);
    }
    
    public async Task UpdateAsync(Tarefa tar)
    {
        await _tarefa.ReplaceOneAsync(t => t.Id == tar.Id, tar);
    }
    
    public async Task DeleteAsync(ObjectId id)
    {
        var filter = Builders<Tarefa>.Filter.Eq(t => t.Id, id);
        await _tarefa.DeleteOneAsync(filter);
    }

    
    public async Task<bool> NomeTarefaExisteAsync(string nome)
    {
        // Aqui depende do seu repositório / ORM (Mongo, EF Core, etc).
        // Exemplo genérico para Mongo:
        return await _tarefa
            .Find(f => f.Nome == nome)
            .AnyAsync();
    }
    
    public async Task<bool> TestConnectionAndListCollectionsAsync()
    {
        try
        {
            var client = new MongoClient("mongodb+srv://Fabricio:premix@dbpremix.4c9gbkl.mongodb.net/?retryWrites=true&w=majority&appName=dbPremix");
            var db = client.GetDatabase("dbPremix");

            Console.WriteLine("Conectado ao Mongo — Database: ");

            var collections = await db.ListCollectionNames().ToListAsync();
            Console.WriteLine("Collections neste DB: " + string.Join(", ", collections));

            if (!collections.Contains("Departamentos"))
            {
                Console.WriteLine("ATENÇÃO: collection 'Departamentos' não encontrada.");
            }
            else
            {
                var coll = db.GetCollection<BsonDocument>("Departamentos");
                var count = await coll.CountDocumentsAsync(FilterDefinition<BsonDocument>.Empty);
                Console.WriteLine($"Documentos em 'Departamentos': {count}");
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro na conexão/test: " + ex);
            return false;
        }
    }
    
}
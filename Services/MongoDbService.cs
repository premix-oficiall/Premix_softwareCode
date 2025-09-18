using System.Threading.Tasks;
using MongoDB.Driver;
using Premix.Models;

namespace Premix.Services;

public class MongoDbService
{
    private readonly IMongoCollection<Gestor> _gestoresCollection;
    private readonly IMongoCollection<Departamentos> _departamentosCollection;
    private readonly IMongoCollection<Func> _funcionariosCollection;
    private readonly IMongoCollection<Tarefa> _tarefaCollection;
    private readonly IMongoCollection<Meta> _metaCollection;


    public MongoDbService()
    {
        var client = new MongoClient("mongodb+srv://Fabricio:premix@dbpremix.4c9gbkl.mongodb.net/?retryWrites=true&w=majority&appName=dbPremix");
        var database = client.GetDatabase("dbPremix");
        _gestoresCollection = database.GetCollection<Gestor>("Gestor");
        _departamentosCollection = database.GetCollection<Departamentos>("departamentos");
        _funcionariosCollection = database.GetCollection<Func>("funcionarios");
        _tarefaCollection = database.GetCollection<Tarefa>("tarefas");
        _metaCollection = database.GetCollection<Meta>("metas");
    }
    
    
    public async Task<Gestor?> GetGestorByLoginAsync(string username)
    {
        return await _gestoresCollection
            .Find(g => g.Username == username)
            .FirstOrDefaultAsync();
    }
    
    
    public async Task<Func?> GetUserByLoginFuncAsync(string username)
    {
        return await _funcionariosCollection
            .Find(c => c.usuario == username)
            .FirstOrDefaultAsync();
    }
    
    public async Task<Departamentos?> GetDepartamentoByCodigoAsync(string codigo)
    {
        return await _departamentosCollection
            .Find(d => d.Codigo == codigo)
            .FirstOrDefaultAsync();
    }
    
    public async Task<Tarefa?> GetTarefaByCodigoAsync(string nome)
    {
        return await _tarefaCollection
            .Find(f => f.Nome == nome)
            .FirstOrDefaultAsync();
    }
    
    public async Task<Meta?> GetMetaByCodigoAsync(string nome)
    {
        return await _metaCollection
            .Find(m => m.Nome == nome)
            .FirstOrDefaultAsync();
    }

    public async Task<Func?> GetCadastroByCodigoAsync(string usuario, string senha, long cpf)
    {
        // Verifica se já existe
        var cadastroExistente  = await _funcionariosCollection
            .Find(c => c.cpf == cpf || c.usuario == usuario || c.senha == senha)
            .FirstOrDefaultAsync();
        
        if (cadastroExistente != null)
        {
            return null; // já existe, não insere
        }

        // Cria um novo cadastro
        var novoCadastro = new Func
        {
            usuario = usuario,
            senha = senha,
            cpf = cpf
        };

        await _funcionariosCollection.InsertOneAsync(novoCadastro);

        return novoCadastro;
    }
    
    

    

    
    
   
    
    
}
    
    


    

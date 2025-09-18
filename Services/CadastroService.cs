using System.Threading.Tasks;
using MongoDB.Driver;
using Premix.Models;

namespace Premix.Services;

public class CadastroService
{
    private readonly IMongoCollection<Func> _cadastros;
    
    private readonly MongoDbService _dbService;

    public CadastroService()
    {
        _dbService = new MongoDbService();
    }
    
    public async Task<Func?> CadastrarFuncionarioAsync(string usuario, string senha, long cpf)
    {
        return await _dbService.GetCadastroByCodigoAsync(usuario, senha, cpf);
    }
}
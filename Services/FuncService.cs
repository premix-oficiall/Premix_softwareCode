using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Premix.Models;

namespace Premix.Services;

public class FuncService
{
    private readonly IMongoCollection<Func> _funcionarios;

    
    private readonly MongoDbService _dbService;

    public FuncService()
    {
        _dbService = new MongoDbService();
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var func = await _dbService.GetUserByLoginFuncAsync(username);
        
        if (func == null)
            return false;

        // Trim para evitar espaços
        if (func.senha.Trim() != password.Trim())
            return false;

        // Salvar user logado na sessão
        SessionService.SetUser(func.Id, func.usuario);
        Console.WriteLine($"Usuário logado: {SessionService.Username}, ID: {SessionService.LoggedUserId}");
        return true;
    }

   
}
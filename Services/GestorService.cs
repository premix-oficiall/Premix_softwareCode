using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Premix.Models;

namespace Premix.Services;

public class GestorService
{
    private readonly IMongoCollection<Gestor> _gestores;

    private readonly MongoDbService _dbService;

    public GestorService()
    {
        _dbService = new MongoDbService();
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var gest = await _dbService.GetGestorByLoginAsync(username);

        if (gest == null)
            return false;

        // Trim para evitar espaços
        if (gest.Password.Trim() != password.Trim())
            return false;

        // Salvar user logado na sessão
        SessionService.SetUser(gest.Id, gest.Username);
        Console.WriteLine($"Usuário logado: {SessionService.Username}, ID: {SessionService.LoggedUserId}");
        return true;
    }
}
   
    

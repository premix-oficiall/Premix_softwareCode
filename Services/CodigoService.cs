using System.Threading.Tasks;
using MongoDB.Driver;
using Premix.Models;
using Premix.ViewModels;

namespace Premix.Services;

public class CodigoService
{
    private readonly DepartamentoService _departamentoService;

    public CodigoService()
    {
        _departamentoService = new DepartamentoService();
    }

    public async Task<Departamentos?> ValidarCodigoAsync(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return null;

        return await _departamentoService.GetByCodigoAsync(codigo);
    }
}
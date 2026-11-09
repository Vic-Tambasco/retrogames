using System.Collections.Generic;
using System.Threading.Tasks;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Services;

public class GeneroService : IGeneroService
{
    private readonly IRepositorySet _repository;

    public GeneroService(IRepositorySet repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Genero>> ListarAsync()
    {
        return await _repository.Genero.ListarTodosAsync();
    }
}
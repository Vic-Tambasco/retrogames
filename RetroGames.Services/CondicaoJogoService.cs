using System.Collections.Generic;
using System.Threading.Tasks;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Services;

public class CondicaoJogoService : ICondicaoJogoService
{
    private readonly IRepositorySet _repository;

    public CondicaoJogoService(IRepositorySet repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CondicaoJogo>> ListarAsync()
    {
        return await _repository.CondicaoJogo.ListarAsync();
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using RetroGames.Model.Model;

namespace RetroGames.Services;

public interface IGeneroService
{
    Task<IEnumerable<Genero>> ListarAsync();
}
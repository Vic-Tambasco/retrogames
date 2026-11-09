using System.Collections.Generic;
using System.Threading.Tasks;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;
using RetroGames.Repository;

namespace RetroGames.Services;

public class PlataformaService : IPlataformaService
{
    private readonly IRepositorySet _repository;

    public PlataformaService(IRepositorySet repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Plataforma>> ListarAsync()
    {
        return await _repository.Plataforma.ListarAsync();
    }

    public async Task<Plataforma> BuscarPorIdAsync(int id)
    {
        var plataforma = await _repository.Plataforma.BuscarPorIdAsync(id);
        if (plataforma == null)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);
        }
        return plataforma;
    }

    public async Task<Plataforma> InserirAsync(Plataforma plataforma)
    {
        // RN01: Remove espaços do início e fim do nome
        plataforma.Nome = plataforma.Nome.Trim();

        // RN01: Verifica se o nome já existe
        if (await _repository.Plataforma.ExisteComNomeAsync(plataforma.Nome))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        try
        {
            plataforma.Id = await _repository.Plataforma.InserirAsync(plataforma);
        }
        catch (RegistroDuplicadoException) // Tratamento de concorrência (corrida)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        return plataforma;
    }

    public async Task<Plataforma> AlterarAsync(Plataforma plataforma)
    {
        // Verifica se a plataforma existe
        var plataformaExistente = await _repository.Plataforma.BuscarPorIdAsync(plataforma.Id);
        if (plataformaExistente == null)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);
        }

        plataforma.Nome = plataforma.Nome.Trim();

        // RN01: Verifica se o nome já existe, ignorando o próprio registro
        if (await _repository.Plataforma.ExisteComNomeAsync(plataforma.Nome, ignorarId: plataforma.Id))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        try
        {
            await _repository.Plataforma.AlterarAsync(plataforma);
        }
        catch (RegistroDuplicadoException)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        return plataforma;
    }

    public async Task ExcluirAsync(int id)
    {
        // Verifica se a plataforma existe
        var plataforma = await _repository.Plataforma.BuscarPorIdAsync(id);
        if (plataforma == null)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);
        }

        // RN02: Verifica se a plataforma possui jogos
        if (await _repository.Plataforma.PossuiJogosAsync(id))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_POSSUI_JOGOS);
        }

        await _repository.Plataforma.ExcluirAsync(id);
    }
}
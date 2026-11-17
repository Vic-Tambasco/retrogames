using Microsoft.AspNetCore.Identity;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Services;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly IRepositorySet _repository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public AutenticacaoService(IRepositorySet repository, IPasswordHasher<Usuario> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Usuario?> AutenticarAsync(string email, string senha)
    {
        var usuario = await _repository.Usuario.BuscarPorEmailAsync(email);

        if (usuario == null) 
            return null;

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, senha);
        
        if (resultado == PasswordVerificationResult.Failed)
            return null;

        return usuario;
    }

    public async Task GarantirAdministradorInicialAsync(string email, string senha)
    {
        if (await _repository.Usuario.ExisteAdministradorAsync())
            return;

        var admin = new Usuario
        {
            Nome = "Administrador",
            Email = email,
            IdPerfil = 1 // Assumindo que 1 é Administrador
        };

        // A mágica da segurança acontece aqui: gera o hash!
        admin.SenhaHash = _passwordHasher.HashPassword(admin, senha);

        await _repository.Usuario.InserirAsync(admin);
    }
}
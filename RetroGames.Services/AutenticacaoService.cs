using Microsoft.AspNetCore.Identity;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;
using RetroGames.Model.Constantes;


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

    public async Task<Usuario> CadastrarClienteAsync(string nome, string email, string senha)
    {
        // 1. Verifica se o e-mail já existe
        var usuarioExistente = await _repository.Usuario.BuscarPorEmailAsync(email);
        if (usuarioExistente != null)
        {
            throw new BusinessRuleException(BusinessErrors.EMAIL_JA_CADASTRADO);
        }

        // 2. Cria o novo usuário com perfil 2 (Cliente)
        var novoUsuario = new Usuario
        {
            Nome = nome,
            Email = email,
            IdPerfil = PerfilIds.Cliente // Usando a constante que já existe no seu projeto
        };

        // 3. Gera o hash da senha
        novoUsuario.SenhaHash = _passwordHasher.HashPassword(novoUsuario, senha);

        // 4. Salva no banco
        novoUsuario.Id = await _repository.Usuario.InserirAsync(novoUsuario);

        return novoUsuario;
    }

    public async Task GarantirAdministradorInicialAsync(string email, string senha)
    {
        if (await _repository.Usuario.ExisteAdministradorAsync())
            return;

        var admin = new Usuario
        {
            Nome = "Administrador",
            Email = email,
            IdPerfil = PerfilIds.Administrador
        };

        // A mágica da segurança acontece aqui: gera o hash!
        admin.SenhaHash = _passwordHasher.HashPassword(admin, senha);

        await _repository.Usuario.InserirAsync(admin);
    }
}
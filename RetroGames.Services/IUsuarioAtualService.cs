namespace RetroGames.Services;

public interface IUsuarioAtualService
{
    bool EstaAutenticado();
    int ObterId();
    bool EhAdministrador();
}
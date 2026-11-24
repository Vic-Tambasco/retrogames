import { Link, Outlet, useNavigate } from 'react-router-dom';
import { useAutenticacao } from '../hooks/useAutenticacao';

export default function Layout() {
  const { usuario, carregando, estaAutenticado, ehAdministrador, sair } = useAutenticacao();
  const navigate = useNavigate();

  const handleSair = async () => {
    await sair();
    navigate('/'); // Volta para o catálogo
  };

  return (
    <div className="d-flex flex-column min-vh-100">
      <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
        <div className="container">
          <Link className="navbar-brand" to="/">RetroGames</Link>
          
          <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#menu">
            <span className="navbar-toggler-icon"></span>
          </button>
          
          <div className="collapse navbar-collapse" id="menu">
            <ul className="navbar-nav me-auto">
              <li className="nav-item">
                <Link className="nav-link" to="/">Catálogo</Link>
              </li>
              {/* Menu Administração só aparece para admin */}
              {ehAdministrador && (
                <li className="nav-item dropdown">
                  <a className="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown">
                    Administração
                  </a>
                  <ul className="dropdown-menu">
                    <li><Link className="dropdown-item" to="/admin/jogos">Jogos</Link></li>
                    <li><Link className="dropdown-item" to="/admin/plataformas">Plataformas</Link></li>
                  </ul>
                </li>
              )}
            </ul>

            <div className="d-flex align-items-center gap-3">
              {!carregando && (
                estaAutenticado ? (
                  <>
                    <span className="navbar-text text-light">Olá, {usuario.nome}</span>
                    <button className="btn btn-outline-light btn-sm" onClick={handleSair}>Sair</button>
                  </>
                ) : (
                  <Link to="/login" className="btn btn-outline-light btn-sm">Entrar</Link>
                )
              )}
            </div>
          </div>
        </div>
      </nav>

      <main className="flex-grow-1 py-4">
        <Outlet />
      </main>

      <footer className="bg-dark text-light text-center py-3 mt-auto">
        <div className="container">
          <small>&copy; 2026 RetroGames. Todos os direitos reservados.</small>
        </div>
      </footer>
    </div>
  );
}
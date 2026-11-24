import { Link, Outlet } from 'react-router-dom';
import { useAutenticacao } from '../hooks/useAutenticacao'; // <-- Adicione esta linha

export default function Layout() {
  const { usuario, carregando } = useAutenticacao(); // <-- Adicione esta linha

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
            </ul>
            
            {/* TESTE TEMPORÁRIO: Mostra o nome ou "Visitante" */}
            <span className="navbar-text text-white">
              {carregando ? 'Carregando...' : (usuario ? `Olá, ${usuario.nome}` : 'Visitante')}
            </span>

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
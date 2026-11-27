import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { useAutenticacao } from '../hooks/useAutenticacao';

export default function RotaProtegida({ somenteAdministrador = false }) {
  const { carregando, estaAutenticado, ehAdministrador } = useAutenticacao();
  const location = useLocation();

  // Enquanto o React não sabe quem é o usuário, mostra uma mensagem de espera
  if (carregando) {
    return <div className="text-center my-5">Verificando sessão...</div>;
  }

  // Se não estiver logado, vai para o login, guardando para onde queria ir (?voltarPara=...)
  if (!estaAutenticado) {
    return <Navigate to={`/login?voltarPara=${location.pathname}`} replace />;
  }

  // Se a rota exige administrador e o usuário não é admin, volta para o catálogo
  if (somenteAdministrador && !ehAdministrador) {
    return <Navigate to="/" replace />;
  }

  // Se passou por tudo, pode ver a tela (O Outlet renderiza a página filha)
  return <Outlet />;
}
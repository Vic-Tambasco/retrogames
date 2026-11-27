import { Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import RotaProtegida from './components/RotaProtegida'; // <-- Importe
import Catalogo from './pages/Catalogo/Catalogo';
import DetalheJogo from './pages/Catalogo/DetalheJogo';
import NaoEncontrada from './pages/NaoEncontrada';
import Login from './pages/Login/Login';
import Jogos from './pages/Administracao/Jogos'; // <-- Importe
import Plataforma from './pages/Auxiliares/Plataforma'; // <-- Importe

function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Catalogo />} />
        <Route path="jogos/:id" element={<DetalheJogo />} />
        <Route path="login" element={<Login />} />
        
        {/* Grupo de rotas protegidas exclusivamente para Administrador */}
        <Route element={<RotaProtegida somenteAdministrador={true} />}>
          <Route path="admin/jogos" element={<Jogos />} />
          <Route path="admin/plataformas" element={<Plataforma />} />
        </Route>

        <Route path="*" element={<NaoEncontrada />} />
      </Route>
    </Routes>
  );
}

export default App;
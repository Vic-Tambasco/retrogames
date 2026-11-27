import { Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import RotaProtegida from './components/RotaProtegida';
import Catalogo from './pages/Catalogo/Catalogo';
import DetalheJogo from './pages/Catalogo/DetalheJogo';
import NaoEncontrada from './pages/NaoEncontrada';
import Login from './pages/Login/Login';
import Jogos from './pages/Administracao/Jogos'; 
import Plataforma from './pages/Auxiliares/Plataforma'; 
import FormularioJogo from './pages/Administracao/FormularioJogo';
import Cadastro from './pages/Cadastro/Cadastro';


function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Catalogo />} />
        <Route path="jogos/:id" element={<DetalheJogo />} />
        <Route path="login" element={<Login />} />
        <Route path="cadastro" element={<Cadastro />} /> 
        
        {/* Grupo de rotas protegidas exclusivamente para Administrador */}
        <Route element={<RotaProtegida somenteAdministrador={true} />}>
          <Route path="admin/jogos" element={<Jogos />} />
          <Route path="admin/jogos/novo" element={<FormularioJogo />} />          
          <Route path="admin/jogos/:id/editar" element={<FormularioJogo />} />
          <Route path="admin/plataformas" element={<Plataforma />} />
        </Route>

        <Route path="*" element={<NaoEncontrada />} />
      </Route>
    </Routes>
  );
}

export default App;
import { useState, useEffect } from 'react';
import { AutenticacaoContext } from './autenticacaoContext';
import { autenticacaoApi } from '../api/servicos';

export function AutenticacaoProvider({ children }) {
  const [usuario, setUsuario] = useState(null);
  const [carregando, setCarregando] = useState(true);

  // Função que busca o usuário na API
  const buscarUsuario = async () => {
    try {
      const dados = await autenticacaoApi.usuarioAtual();
      setUsuario(dados);
    } catch (erro) {
      setUsuario(null); // 401 significa visitante
    } finally {
      setCarregando(false);
    }
  };

  // Roda ao abrir o site e escuta o evento de sessão expirada
  useEffect(() => {
    buscarUsuario();

    const aoExpirarSessao = () => setUsuario(null);
    window.addEventListener('sessao:expirada', aoExpirarSessao);

    return () => {
      window.removeEventListener('sessao:expirada', aoExpirarSessao);
    };
  }, []);

  const entrar = async (email, senha) => {
    const dados = await autenticacaoApi.entrar(email, senha);
    setUsuario(dados);
  };

  const sair = async () => {
    await autenticacaoApi.sair();
    setUsuario(null);
  };

  return (
    <AutenticacaoContext.Provider value={{
      usuario,
      carregando,
      estaAutenticado: !!usuario,
      ehAdministrador: usuario?.idPerfil === 1, // 1 = Administrador no seu banco
      entrar,
      sair
    }}>
      {children}
    </AutenticacaoContext.Provider>
  );
}
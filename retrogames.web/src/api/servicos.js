// src/api/servicos.js
import api from './api';

export const jogoApi = {
  listar: async (filtros) => {
    const resposta = await api.get('/Jogo', { params: filtros });
    return resposta.data.data; // Retorna direto a página de jogos do envelope
  },
  buscarPorId: async (id) => {
    const resposta = await api.get(`/Jogo/${id}`);
    return resposta.data.data;
  },
    inserir: async (dados) => {
    const resposta = await api.post('/Jogo', dados);
    return resposta.data.data;
  },
  alterar: async (id, dados) => {
    const resposta = await api.put(`/Jogo/${id}`, dados);
    return resposta.data.data;
  },
  desativar: async (id) => {
    const resposta = await api.delete(`/Jogo/${id}`);
    return resposta.data;
  },
};

export const plataformaApi = {
  listar: async () => {
    const resposta = await api.get('/Plataforma');
    return resposta.data.data;
  },
    inserir: async (dados) => {
    const resposta = await api.post('/Plataforma', dados);
    return resposta.data.data;
  },
  alterar: async (id, dados) => {
    const resposta = await api.put(`/Plataforma/${id}`, dados);
    return resposta.data.data;
  },
  excluir: async (id) => {
    const resposta = await api.delete(`/Plataforma/${id}`);
    return resposta.data;
  },
};

export const generoApi = {
  listar: async () => {
    const resposta = await api.get('/Genero');
    return resposta.data.data;
  },
};

export const condicaoJogoApi = {
  listar: async () => {
    const resposta = await api.get('/CondicaoJogo');
    return resposta.data.data;
  },
};

export const autenticacaoApi = {
  entrar: async (email, senha) => {
    const resposta = await api.post('/Autenticacao/login', { email, senha });
    return resposta.data.data;
  },
  sair: async () => {
    const resposta = await api.post('/Autenticacao/logout');
    return resposta.data;
  },
  usuarioAtual: async () => {
    const resposta = await api.get('/Autenticacao/eu');
    return resposta.data.data;
  }
};
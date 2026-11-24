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
};

export const plataformaApi = {
  listar: async () => {
    const resposta = await api.get('/Plataforma');
    return resposta.data.data;
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
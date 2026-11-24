import axios from 'axios';

const api = axios.create({
  baseURL: '/api',
  withCredentials: true,
});

// Interceptor de resposta
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Se o erro for 401 (Não autorizado) e não for nas rotas de login/eu
    if (error.response?.status === 401) {
      const url = error.config.url;
      if (!url.includes('/Autenticacao/login') && !url.includes('/Autenticacao/eu')) {
        // Dispara um evento para o Provider limpar a sessão
        window.dispatchEvent(new Event('sessao:expirada'));
      }
    }
    return Promise.reject(error);
  }
);

export default api;
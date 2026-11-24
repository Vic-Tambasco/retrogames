// src/api/api.js
import axios from 'axios';

const api = axios.create({
  baseURL: '/api',
  withCredentials: true, // Permite que o cookie de sessão viaje nas requisições
});

export default api;
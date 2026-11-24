// src/api/erros.js
export function obterMensagemErro(erro) {
  if (!erro.response) {
    return "Não foi possível conectar ao servidor.";
  }
  
  const envelope = erro.response.data;
  if (envelope && envelope.errorMessage) {
    return envelope.errorMessage;
  }
  
  return "Ocorreu um erro inesperado.";
}
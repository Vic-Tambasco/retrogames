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

export function obterErrosDosCampos(erro) {
  const envelope = erro.response?.data;
  // Se for erro de validação (código 3), a API devolve os campos em 'data'
  if (envelope && envelope.responseCode === 3 && envelope.data) {
    return envelope.data;
  }
  return {};
}
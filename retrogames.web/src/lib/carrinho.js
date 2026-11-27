// src/lib/carrinho.js

export function obterCarrinho() {
  const carrinho = localStorage.getItem('carrinho');
  return carrinho ? JSON.parse(carrinho) : [];
}

function salvarCarrinho(carrinho) {
  localStorage.setItem('carrinho', JSON.stringify(carrinho));
}

export function adicionarAoCarrinho(jogo) {
  const carrinho = obterCarrinho();
  const itemExistente = carrinho.find(item => item.id === jogo.id);
  
  if (itemExistente) {
    if (itemExistente.quantidade >= 10) {
      alert("Você já adicionou o limite de 10 unidades deste jogo.");
      return;
    }
    itemExistente.quantidade += 1;
  } else {
    carrinho.push({
      id: jogo.id,
      nome: jogo.nome,
      preco: jogo.preco,
      nomePlataforma: jogo.nomePlataforma,
      quantidade: 1
    });
  }
  salvarCarrinho(carrinho);
}

export function removerDoCarrinho(id) {
  const carrinho = obterCarrinho();
  salvarCarrinho(carrinho.filter(item => item.id !== id));
}

export function alterarQuantidade(id, novaQuantidade) {
  if (novaQuantidade < 1 || novaQuantidade > 10) return;
  const carrinho = obterCarrinho();
  const item = carrinho.find(item => item.id === id);
  if (item) {
    item.quantidade = novaQuantidade;
    salvarCarrinho(carrinho);
  }
}

export function limparCarrinho() {
  localStorage.removeItem('carrinho');
}
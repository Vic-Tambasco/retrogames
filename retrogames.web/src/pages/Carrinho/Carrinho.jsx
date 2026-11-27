import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { obterCarrinho, removerDoCarrinho, alterarQuantidade, limparCarrinho } from '../../lib/carrinho';
import { formatarPreco } from '../../lib/formato';
import { pedidoApi } from '../../api/servicos'; // <-- Importe a API do pedido
import { obterMensagemErro } from '../../api/erros'; // <-- Importe para tratar erros

export default function Carrinho() {
  const [itens, setItens] = useState([]);
  const [finalizando, setFinalizando] = useState(false); // <-- Estado para o botão
  const navigate = useNavigate();

  const carregarCarrinho = () => {
    setItens(obterCarrinho());
  };

  useEffect(() => {
    carregarCarrinho();
  }, []);

  const handleAlterarQtd = (id, qtd) => {
    alterarQuantidade(id, qtd);
    carregarCarrinho();
  };

  const handleRemover = (id) => {
    removerDoCarrinho(id);
    carregarCarrinho();
  };

  // Função para finalizar a compra
  const handleFinalizar = async () => {
    setFinalizando(true);
    try {
      // Prepara os dados no formato que a API espera
      const itensParaEnviar = itens.map(i => ({ idJogo: i.id, quantidade: i.quantidade }));

      await pedidoApi.finalizar(itensParaEnviar);

      limparCarrinho(); // Limpa o carrinho
      alert("Compra realizada com sucesso!");
      navigate('/meus-pedidos'); // Redireciona para os pedidos

    } catch (falha) {
      alert(obterMensagemErro(falha)); // Mostra erro (ex: Estoque insuficiente)
    } finally {
      setFinalizando(false);
    }
  };

  const total = itens.reduce((acc, item) => acc + (item.preco * item.quantidade), 0);

  if (itens.length === 0) {
    return (
      <div className="container mt-5 text-center">
        <h3>Seu carrinho está vazio.</h3>
        <Link to="/" className="btn btn-primary mt-3">Voltar ao catálogo</Link>
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <h2>Meu Carrinho</h2>
      <div className="row mt-4">
        <div className="col-md-8">
          <table className="table align-middle">
            <thead className="table-dark">
              <tr>
                <th>Jogo</th>
                <th>Preço</th>
                <th>Qtd</th>
                <th>Subtotal</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {itens.map(item => (
                <tr key={item.id}>
                  <td>
                    <strong>{item.nome}</strong><br/>
                    <small className="text-muted">{item.nomePlataforma}</small>
                  </td>
                  <td>{formatarPreco(item.preco)}</td>
                  <td style={{ width: '100px' }}>
                    <select 
                      className="form-select" 
                      value={item.quantidade} 
                      onChange={(e) => handleAlterarQtd(item.id, Number(e.target.value))}
                    >
                      {[...Array(10)].map((_, i) => (
                        <option key={i + 1} value={i + 1}>{i + 1}</option>
                      ))}
                    </select>
                  </td>
                  <td>{formatarPreco(item.preco * item.quantidade)}</td>
                  <td>
                    <button className="btn btn-sm btn-outline-danger" onClick={() => handleRemover(item.id)}>
                      Remover
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        
        <div className="col-md-4">
          <div className="card bg-light">
            <div className="card-body">
              <h4 className="card-title">Resumo</h4>
              <hr />
              <div className="d-flex justify-content-between mb-3">
                <span>Total:</span>
                <strong className="fs-5 text-success">{formatarPreco(total)}</strong>
              </div>

              {/* BOTÃO ATUALIZADO */}
              <button 
                className="btn btn-success w-100" 
                onClick={handleFinalizar}
                disabled={finalizando}
              >
                {finalizando ? 'Finalizando...' : 'Finalizar Compra'}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
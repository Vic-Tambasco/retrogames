import { useEffect, useState } from 'react';
import { pedidoApi } from '../../api/servicos';
import { obterMensagemErro } from '../../api/erros';
import { formatarPreco } from '../../lib/formato';
import { Carregando, MensagemErro } from '../../components/MensagensEstado';
import { Link } from 'react-router-dom';

export default function MeusPedidos() {
  const [pedidos, setPedidos] = useState([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState('');

  const carregarPedidos = () => {
    setCarregando(true);
    setErro('');
    pedidoApi.meusPedidos()
      .then(setPedidos)
      .catch((falha) => setErro(obterMensagemErro(falha)))
      .finally(() => setCarregando(false));
  };

  useEffect(() => {
    carregarPedidos();
  }, []);

  if (carregando) return <Carregando />;
  if (erro) return <div className="container mt-4"><MensagemErro mensagem={erro} onTentarNovamente={carregarPedidos} /></div>;

  if (pedidos.length === 0) {
    return (
      <div className="container mt-5 text-center">
        <h3>Você ainda não fez nenhum pedido.</h3>
        <Link to="/" className="btn btn-primary mt-3">Ir às compras</Link>
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <h2>Meus Pedidos</h2>
      
      <div className="mt-4">
        {pedidos.map(pedido => (
          <div key={pedido.id} className="card mb-4 shadow-sm">
            <div className="card-header bg-dark text-white d-flex justify-content-between align-items-center">
              <span>
                Pedido #{pedido.id} - {new Date(pedido.data).toLocaleDateString('pt-BR')}
              </span>
              <span className="badge bg-warning text-dark">{pedido.situacao}</span>
            </div>
            <div className="card-body">
              <ul className="list-group list-group-flush mb-3">
                {pedido.itens.map((item, index) => (
                  <li key={index} className="list-group-item d-flex justify-content-between align-items-center">
                    <div>
                      <strong>{item.quantidade}x</strong> {item.nomeJogo}
                    </div>
                    <span>{formatarPreco(item.valorUnitario)}</span>
                  </li>
                ))}
              </ul>
              <div className="d-flex justify-content-end align-items-center">
                <strong className="me-3">Total do Pedido:</strong>
                <h4 className="text-success mb-0">{formatarPreco(pedido.total)}</h4>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
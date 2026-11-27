import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { jogoApi } from '../../api/servicos';
import { obterMensagemErro } from '../../api/erros';
import { formatarPreco } from '../../lib/formato';
import { Carregando, MensagemErro, EstadoVazio } from '../../components/MensagensEstado';

export default function Jogos() {
  const [jogos, setJogos] = useState([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState('');
  const navigate = useNavigate();

  const carregarJogos = () => {
    setCarregando(true);
    // incluirInativos: true é obrigatório para o admin ver os desativados (RN04)
    jogoApi.listar({ incluirInativos: true, tamanhoPagina: 50 })
      .then((resultado) => setJogos(resultado.itens))
      .catch((falha) => setErro(obterMensagemErro(falha)))
      .finally(() => setCarregando(false));
  };

  useEffect(() => {
    carregarJogos();
  }, []);

  const handleDesativar = async (id) => {
    if (!window.confirm("Tem certeza que deseja desativar este jogo? Ele sumirá do catálogo público.")) return;
    
    try {
      await jogoApi.desativar(id);
      carregarJogos(); // Recarrega para atualizar o status na tela
    } catch (falha) {
      alert(obterMensagemErro(falha));
    }
  };

  if (carregando) return <Carregando />;
  if (erro) return <div className="container mt-4"><MensagemErro mensagem={erro} onTentarNovamente={carregarJogos} /></div>;

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>Jogos</h2>
        <Link to="/admin/jogos/novo" className="btn btn-success">Novo Jogo</Link>
      </div>

      {jogos.length === 0 ? (
        <EstadoVazio mensagem="Nenhum jogo cadastrado." />
      ) : (
        <table className="table table-hover table-bordered align-middle shadow-sm">
          <thead className="table-dark">
            <tr>
              <th>Nome</th>
              <th>Plataforma</th>
              <th>Preço</th>
              <th>Situação</th>
              <th className="text-center">Ações</th>
            </tr>
          </thead>
          <tbody>
            {jogos.map(jogo => (
              <tr key={jogo.id}>
                <td>{jogo.nome}</td>
                <td>{jogo.nomePlataforma}</td>
                <td>{formatarPreco(jogo.preco)}</td>
                <td>
                  {jogo.ativo ? (
                    <span className="badge bg-success">Ativo</span>
                  ) : (
                    <span className="badge bg-danger">Inativo</span>
                  )}
                </td>
                <td className="text-center">
                  <button className="btn btn-sm btn-outline-primary me-2" onClick={() => navigate(`/admin/jogos/${jogo.id}/editar`)}>
                    Editar
                  </button>
                  {jogo.ativo && (
                    <button className="btn btn-sm btn-outline-danger" onClick={() => handleDesativar(jogo.id)}>
                      Desativar
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
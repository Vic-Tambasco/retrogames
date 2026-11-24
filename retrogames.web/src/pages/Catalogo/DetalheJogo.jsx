import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { jogoApi } from '../../api/servicos';
import { obterMensagemErro } from '../../api/erros';
import { formatarPreco } from '../../lib/formato';
import { Carregando, MensagemErro } from '../../components/MensagensEstado';

export default function DetalheJogo() {
  const { id } = useParams(); // Pega o ID da URL (ex: /jogos/3)
  
  const [jogo, setJogo] = useState(null);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState('');

  const carregarJogo = () => {
    setCarregando(true);
    setErro('');
    
    jogoApi.buscarPorId(id)
      .then(setJogo)
      .catch((falha) => setErro(obterMensagemErro(falha)))
      .finally(() => setCarregando(false));
  };

  useEffect(() => {
    carregarJogo();
  }, [id]);

  if (carregando) return <Carregando />;
  if (erro) return <div className="container mt-4"><MensagemErro mensagem={erro} onTentarNovamente={carregarJogo} /></div>;
  if (!jogo) return null;

  return (
    <div className="container mt-4">
      <div className="mb-3">
        <Link to="/" className="btn btn-outline-secondary btn-sm">
          &larr; Voltar ao catálogo
        </Link>
      </div>

      <div className="card shadow-sm">
        <div className="card-body">
          <div className="d-flex justify-content-between align-items-start mb-3">
            <div>
              <h1 className="display-5">{jogo.nome}</h1>
              <p className="text-muted">
                {jogo.nomePlataforma} · {jogo.anoDeLancamento}
              </p>
            </div>
            <h2 className="text-success">{formatarPreco(jogo.preco)}</h2>
          </div>

          <hr />

          <div className="row">
            <div className="col-md-8">
              <h5>Descrição</h5>
              <p>{jogo.descricao || 'Nenhuma descrição fornecida para este jogo.'}</p>
              
              <h5 className="mt-4">Gêneros</h5>
              <div>
                {jogo.generos.map((genero) => (
                  <span key={genero.id} className="badge bg-primary me-2 fs-6">
                    {genero.nome}
                  </span>
                ))}
              </div>
            </div>

            <div className="col-md-4">
              <div className="border rounded p-3 bg-light">
                <p className="mb-1"><strong>Condição:</strong> {jogo.descricaoCondicao}</p>
                <p className="mb-1"><strong>Estoque:</strong> {jogo.estoque} unidades</p>
                <p className="mb-0">
                  <strong>Status:</strong>{' '}
                  {jogo.ativo ? (
                    <span className="text-success">Ativo</span>
                  ) : (
                    <span className="text-danger">Inativo</span>
                  )}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
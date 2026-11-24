import { Link } from 'react-router-dom';
import { formatarPreco } from '../lib/formato';

export default function CardJogo({ jogo }) {
  const esgotado = jogo.estoque === 0;

  return (
    <article className="card h-100 shadow-sm">
      <div className="card-body d-flex flex-column">
        <div className="d-flex justify-content-between align-items-center mb-2">
          <span className="badge bg-secondary">{jogo.nomePlataforma}</span>
          <small className="text-muted">{jogo.anoDeLancamento}</small>
        </div>
        
        <h2 className="h5 card-title text-truncate" title={jogo.nome}>{jogo.nome}</h2>
        <p className="card-text text-muted small mb-1">Condição: {jogo.descricaoCondicao}</p>
        
        <div className="mb-3">
          {jogo.nomesGeneros.map((genero, index) => (
            <span key={index} className="badge bg-light text-dark border me-1 small">
              {genero}
            </span>
          ))}
        </div>

        <div className="mt-auto d-flex justify-content-between align-items-center">
          <strong className="fs-5 text-success">{formatarPreco(jogo.preco)}</strong>
          {esgotado ? (
            <span className="badge bg-danger">Esgotado</span>
          ) : (
            <span className="badge bg-success bg-opacity-75">Em estoque</span>
          )}
        </div>
      </div>
      <div className="card-footer bg-white border-top-0 pt-0">
        <Link to={`/jogos/${jogo.id}`} className="btn btn-outline-primary w-100">
          Ver detalhes
        </Link>
      </div>
    </article>
  );
}
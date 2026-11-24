import { Link } from 'react-router-dom';

export default function NaoEncontrada() {
  return (
    <div className="container text-center mt-5">
      <h1>404</h1>
      <p>Página não encontrada.</p>
      <Link to="/" className="btn btn-primary">Voltar ao catálogo</Link>
    </div>
  );
}
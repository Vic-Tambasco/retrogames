import { useState } from 'react';
import { useNavigate, useSearchParams, Link } from 'react-router-dom';
import { useAutenticacao } from '../../hooks/useAutenticacao';
import { obterMensagemErro } from '../../api/erros';

export default function Login() {
  const { entrar } = useAutenticacao();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  
  // Se o usuário foi barrado numa rota admin, a URL tem um ?voltarPara=...
  const voltarPara = searchParams.get('voltarPara') || '/';

  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState('');
  const [enviando, setEnviando] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErro('');
    setEnviando(true);

    try {
      await entrar(email, senha);
      navigate(voltarPara);
    } catch (falha) {
      setErro(obterMensagemErro(falha));
    } finally {
      setEnviando(false);
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: '400px' }}>
      <div className="card shadow">
        <div className="card-body p-4">
          <h2 className="text-center mb-4">Entrar</h2>
          
          {erro && <div className="alert alert-danger">{erro}</div>}

          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <label className="form-label">E-mail</label>
              <input 
                type="email" 
                className="form-control" 
                value={email} 
                onChange={(e) => setEmail(e.target.value)} 
                required 
              />
            </div>
            <div className="mb-3">
              <label className="form-label">Senha</label>
              <input 
                type="password" 
                className="form-control" 
                value={senha} 
                onChange={(e) => setSenha(e.target.value)} 
                required 
              />
            </div>
            <button type="submit" className="btn btn-primary w-100" disabled={enviando}>
              {enviando ? 'Entrando...' : 'Entrar'}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
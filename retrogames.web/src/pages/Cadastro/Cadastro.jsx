import { useState } from 'react';
import { Link } from 'react-router-dom';
import { autenticacaoApi } from '../../api/servicos';
import { obterMensagemErro, obterErrosDosCampos } from '../../api/erros';

export default function Cadastro() {
  const [nome, setNome] = useState('');
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  
  const [erroGeral, setErroGeral] = useState('');
  const [errosCampos, setErrosCampos] = useState({});
  const [enviando, setEnviando] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setEnviando(true);
    setErroGeral('');
    setErrosCampos({});

    try {
      await autenticacaoApi.cadastrar(nome, email, senha);
      
      // Como o cookie já foi gerado pelo backend, recarregamos a página 
      // para que o Contexto (AuthProvider) leia o novo usuário logado.
      window.location.href = '/'; 
      
    } catch (falha) {
      setErrosCampos(obterErrosDosCampos(falha));
      setErroGeral(obterMensagemErro(falha));
    } finally {
      setEnviando(false);
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: '400px' }}>
      <div className="card shadow">
        <div className="card-body p-4">
          <h2 className="text-center mb-4">Criar Conta</h2>
          
          {erroGeral && <div className="alert alert-danger">{erroGeral}</div>}

          <form onSubmit={handleSubmit} noValidate> 
            <div className="mb-3">
              <label className="form-label">Nome</label>
              <input 
                type="text" 
                className={`form-control ${errosCampos.nome ? 'is-invalid' : ''}`}
                value={nome} 
                onChange={(e) => setNome(e.target.value)} 
              />
              {errosCampos.nome && <div className="invalid-feedback">{errosCampos.nome[0]}</div>}
            </div>

            <div className="mb-3">
              <label className="form-label">E-mail</label>
              <input 
                type="email" 
                className={`form-control ${errosCampos.email ? 'is-invalid' : ''}`}
                value={email} 
                onChange={(e) => setEmail(e.target.value)} 
              />
              {errosCampos.email && <div className="invalid-feedback">{errosCampos.email[0]}</div>}
            </div>

            <div className="mb-3">
              <label className="form-label">Senha</label>
              <input 
                type="password" 
                className={`form-control ${errosCampos.senha ? 'is-invalid' : ''}`}
                value={senha} 
                onChange={(e) => setSenha(e.target.value)} 
              />
              {errosCampos.senha && <div className="invalid-feedback">{errosCampos.senha[0]}</div>}
              <small className="text-muted">Mínimo de 6 caracteres.</small>
            </div>

            <button type="submit" className="btn btn-primary w-100" disabled={enviando}>
              {enviando ? 'Cadastrando...' : 'Cadastrar'}
            </button>
          </form>
          
          <div className="mt-3 text-center">
            <small>Já tem uma conta? <Link to="/login">Faça login</Link></small>
          </div>
        </div>
      </div>
    </div>
  );
}
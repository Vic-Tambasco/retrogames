import { useEffect, useState } from 'react';
import { plataformaApi } from '../../api/servicos';
import { obterMensagemErro, obterErrosDosCampos } from '../../api/erros';
import { Carregando, MensagemErro } from '../../components/MensagensEstado';

export default function Plataforma() {
  const [plataformas, setPlataformas] = useState([]);
  const [carregando, setCarregando] = useState(true);
  const [erroLista, setErroLista] = useState('');

  // Estados do formulário
  const [emEdicao, setEmEdicao] = useState(null); // Guarda o objeto inteiro da plataforma sendo editada
  const [nome, setNome] = useState('');
  const [fabricante, setFabricante] = useState('');
  const [anoDeLancamento, setAnoDeLancamento] = useState('');
  
  // Estados de erro do formulário
  const [erroGeral, setErroGeral] = useState('');
  const [errosCampos, setErrosCampos] = useState({});
  const [enviando, setEnviando] = useState(false);

  const carregarPlataformas = () => {
    setCarregando(true);
    plataformaApi.listar()
      .then(setPlataformas)
      .catch((falha) => setErroLista(obterMensagemErro(falha)))
      .finally(() => setCarregando(false));
  };

  useEffect(() => {
    carregarPlataformas();
  }, []);

  const limparFormulario = () => {
    setEmEdicao(null);
    setNome('');
    setFabricante('');
    setAnoDeLancamento('');
    setErrosCampos({});
    setErroGeral('');
  };

  const prepararEdicao = (plataforma) => {
    setEmEdicao(plataforma);
    setNome(plataforma.nome);
    setFabricante(plataforma.fabricante);
    setAnoDeLancamento(plataforma.anoDeLancamento.toString());
    setErrosCampos({});
    setErroGeral('');
  };

  const handleSalvar = async (e) => {
    e.preventDefault();
    setEnviando(true);
    setErrosCampos({});
    setErroGeral('');

    const dados = {
      nome,
      fabricante,
      anoDeLancamento: Number(anoDeLancamento) // Converte texto para número
    };

    try {
      if (emEdicao) {
        await plataformaApi.alterar(emEdicao.id, dados);
      } else {
        await plataformaApi.inserir(dados);
      }
      limparFormulario();
      carregarPlataformas(); // Recarrega a lista
    } catch (falha) {
      setErrosCampos(obterErrosDosCampos(falha));
      setErroGeral(obterMensagemErro(falha));
    } finally {
      setEnviando(false);
    }
  };

  const handleExcluir = async (id) => {
    if (!window.confirm("Tem certeza que deseja excluir esta plataforma?")) return;

    try {
      await plataformaApi.excluir(id);
      carregarPlataformas();
    } catch (falha) {
      alert(obterMensagemErro(falha)); // Mostra o erro 102 (plataforma com jogos)
    }
  };

  if (carregando) return <Carregando />;
  if (erroLista) return <div className="container mt-4"><MensagemErro mensagem={erroLista} onTentarNovamente={carregarPlataformas} /></div>;

  return (
    <div className="container mt-4">
      <h2>Plataformas</h2>
      
      <div className="row mt-4">
        {/* Formulário */}
        <div className="col-md-4">
          <div className="card shadow-sm">
            <div className="card-header bg-dark text-white">
              {emEdicao ? 'Editar Plataforma' : 'Nova Plataforma'}
            </div>
            <div className="card-body">
              {erroGeral && <div className="alert alert-danger small">{erroGeral}</div>}
              
              <form onSubmit={handleSalvar}>
                <div className="mb-3">
                  <label className="form-label">Nome</label>
                  <input 
                    type="text" 
                    className={`form-control ${errosCampos.nome ? 'is-invalid' : ''}`}
                    value={nome} 
                    onChange={e => setNome(e.target.value)} 
                  />
                  {errosCampos.nome && <div className="invalid-feedback">{errosCampos.nome[0]}</div>}
                </div>

                <div className="mb-3">
                  <label className="form-label">Fabricante</label>
                  <input 
                    type="text" 
                    className={`form-control ${errosCampos.fabricante ? 'is-invalid' : ''}`}
                    value={fabricante} 
                    onChange={e => setFabricante(e.target.value)} 
                  />
                  {errosCampos.fabricante && <div className="invalid-feedback">{errosCampos.fabricante[0]}</div>}
                </div>

                <div className="mb-3">
                  <label className="form-label">Ano de Lançamento</label>
                  <input 
                    type="number" 
                    className={`form-control ${errosCampos.anoDeLancamento ? 'is-invalid' : ''}`}
                    value={anoDeLancamento} 
                    onChange={e => setAnoDeLancamento(e.target.value)} 
                  />
                  {errosCampos.anoDeLancamento && <div className="invalid-feedback">{errosCampos.anoDeLancamento[0]}</div>}
                </div>

                <div className="d-flex gap-2">
                  <button type="submit" className="btn btn-primary flex-grow-1" disabled={enviando}>
                    {enviando ? 'Salvando...' : 'Salvar'}
                  </button>
                  {emEdicao && (
                    <button type="button" className="btn btn-secondary" onClick={limparFormulario}>
                      Cancelar
                    </button>
                  )}
                </div>
              </form>
            </div>
          </div>
        </div>

        {/* Tabela */}
        <div className="col-md-8">
          <table className="table table-hover table-bordered shadow-sm">
            <thead className="table-dark">
              <tr>
                <th>Nome</th>
                <th>Fabricante</th>
                <th>Ano</th>
                <th className="text-center">Ações</th>
              </tr>
            </thead>
            <tbody>
              {plataformas.map(p => (
                <tr key={p.id}>
                  <td>{p.nome}</td>
                  <td>{p.fabricante}</td>
                  <td>{p.anoDeLancamento}</td>
                  <td className="text-center">
                    <button className="btn btn-sm btn-outline-primary me-2" onClick={() => prepararEdicao(p)}>
                      Editar
                    </button>
                    <button className="btn btn-sm btn-outline-danger" onClick={() => handleExcluir(p.id)}>
                      Excluir
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
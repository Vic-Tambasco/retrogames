import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { jogoApi, plataformaApi, generoApi, condicaoJogoApi } from '../../api/servicos';
import { obterMensagemErro, obterErrosDosCampos } from '../../api/erros';
import { Carregando } from '../../components/MensagensEstado';

export default function FormularioJogo() {
  const { id } = useParams(); // Se tiver ID na URL, é edição
  const navigate = useNavigate();
  const ehEdicao = !!id;

  const [carregando, setCarregando] = useState(true);
  const [enviando, setEnviando] = useState(false);
  const [erroGeral, setErroGeral] = useState('');
  const [errosCampos, setErrosCampos] = useState({});

  // Listas de apoio
  const [plataformas, setPlataformas] = useState([]);
  const [generos, setGeneros] = useState([]);
  const [condicoes, setCondicoes] = useState([]);

  // Dados do formulário
  const [jogo, setJogo] = useState({
    nome: '',
    descricao: '',
    anoDeLancamento: '',
    preco: '',
    estoque: '',
    idPlataforma: '',
    idCondicaoJogo: '',
    ativo: true,
    idsGeneros: []
  });

  // Carrega as listas de apoio e, se for edição, os dados do jogo
  useEffect(() => {
    const carregarDados = async () => {
      try {
        const [listaPlataformas, listaGeneros, listaCondicoes] = await Promise.all([
          plataformaApi.listar(),
          generoApi.listar(),
          condicaoJogoApi.listar()
        ]);
        
        setPlataformas(listaPlataformas);
        setGeneros(listaGeneros);
        setCondicoes(listaCondicoes);

        if (ehEdicao) {
          const dadosJogo = await jogoApi.buscarPorId(id);
          setJogo({
            nome: dadosJogo.nome,
            descricao: dadosJogo.descricao || '',
            anoDeLancamento: dadosJogo.anoDeLancamento,
            preco: dadosJogo.preco,
            estoque: dadosJogo.estoque,
            idPlataforma: dadosJogo.idPlataforma,
            idCondicaoJogo: dadosJogo.idCondicaoJogo,
            ativo: dadosJogo.ativo,
            idsGeneros: dadosJogo.generos.map(g => g.id)
          });
        }
      } catch (falha) {
        setErroGeral("Erro ao carregar dados: " + obterMensagemErro(falha));
      } finally {
        setCarregando(false);
      }
    };
    carregarDados();
  }, [id, ehEdicao]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setJogo(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleGeneroChange = (idGenero) => {
    setJogo(prev => {
      const jaTem = prev.idsGeneros.includes(idGenero);
      return {
        ...prev,
        idsGeneros: jaTem 
          ? prev.idsGeneros.filter(g => g !== idGenero)
          : [...prev.idsGeneros, idGenero]
      };
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setEnviando(true);
    setErroGeral('');
    setErrosCampos({});

    // Converte os textos para números
    const dadosParaEnviar = {
      ...jogo,
      anoDeLancamento: Number(jogo.anoDeLancamento),
      preco: Number(jogo.preco),
      estoque: Number(jogo.estoque),
      idPlataforma: Number(jogo.idPlataforma),
      idCondicaoJogo: Number(jogo.idCondicaoJogo),
      descricao: jogo.descricao.trim() || null
    };

    try {
      if (ehEdicao) {
        await jogoApi.alterar(id, dadosParaEnviar);
      } else {
        await jogoApi.inserir(dadosParaEnviar);
      }
      navigate('/admin/jogos');
    } catch (falha) {
      setErrosCampos(obterErrosDosCampos(falha));
      setErroGeral(obterMensagemErro(falha));
    } finally {
      setEnviando(false);
    }
  };

  if (carregando) return <Carregando />;

  return (
    <div className="container mt-4" style={{ maxWidth: '800px' }}>
      <h2>{ehEdicao ? 'Editar Jogo' : 'Novo Jogo'}</h2>
      
      <div className="card shadow-sm mt-4">
        <div className="card-body">
          {erroGeral && <div className="alert alert-danger">{erroGeral}</div>}
          
          <form onSubmit={handleSubmit}>
            <div className="row">
              <div className="col-md-8 mb-3">
                <label className="form-label">Nome</label>
                <input type="text" name="nome" className={`form-control ${errosCampos.nome ? 'is-invalid' : ''}`} value={jogo.nome} onChange={handleChange} />
                {errosCampos.nome && <div className="invalid-feedback">{errosCampos.nome[0]}</div>}
              </div>
              <div className="col-md-4 mb-3">
                <label className="form-label">Ano de Lançamento</label>
                <input type="number" name="anoDeLancamento" className={`form-control ${errosCampos.anoDeLancamento ? 'is-invalid' : ''}`} value={jogo.anoDeLancamento} onChange={handleChange} />
                {errosCampos.anoDeLancamento && <div className="invalid-feedback">{errosCampos.anoDeLancamento[0]}</div>}
              </div>
            </div>

            <div className="mb-3">
              <label className="form-label">Descrição (Opcional)</label>
              <textarea name="descricao" className={`form-control ${errosCampos.descricao ? 'is-invalid' : ''}`} value={jogo.descricao} onChange={handleChange} rows="3"></textarea>
              {errosCampos.descricao && <div className="invalid-feedback">{errosCampos.descricao[0]}</div>}
            </div>

            <div className="row">
              <div className="col-md-6 mb-3">
                <label className="form-label">Plataforma</label>
                <select name="idPlataforma" className={`form-select ${errosCampos.idPlataforma ? 'is-invalid' : ''}`} value={jogo.idPlataforma} onChange={handleChange}>
                  <option value="">Selecione...</option>
                  {plataformas.map(p => <option key={p.id} value={p.id}>{p.nome}</option>)}
                </select>
                {errosCampos.idPlataforma && <div className="invalid-feedback">{errosCampos.idPlataforma[0]}</div>}
              </div>
              <div className="col-md-6 mb-3">
                <label className="form-label">Condição</label>
                <select name="idCondicaoJogo" className={`form-select ${errosCampos.idCondicaoJogo ? 'is-invalid' : ''}`} value={jogo.idCondicaoJogo} onChange={handleChange}>
                  <option value="">Selecione...</option>
                  {condicoes.map(c => <option key={c.id} value={c.id}>{c.descricao}</option>)}
                </select>
                {errosCampos.idCondicaoJogo && <div className="invalid-feedback">{errosCampos.idCondicaoJogo[0]}</div>}
              </div>
            </div>

            <div className="row">
              <div className="col-md-4 mb-3">
                <label className="form-label">Preço (R$)</label>
                <input type="number" step="0.01" name="preco" className={`form-control ${errosCampos.preco ? 'is-invalid' : ''}`} value={jogo.preco} onChange={handleChange} />
                {errosCampos.preco && <div className="invalid-feedback">{errosCampos.preco[0]}</div>}
              </div>
              <div className="col-md-4 mb-3">
                <label className="form-label">Estoque</label>
                <input type="number" name="estoque" className={`form-control ${errosCampos.estoque ? 'is-invalid' : ''}`} value={jogo.estoque} onChange={handleChange} />
                {errosCampos.estoque && <div className="invalid-feedback">{errosCampos.estoque[0]}</div>}
              </div>
              <div className="col-md-4 mb-3 d-flex align-items-center">
                <div className="form-check mt-4">
                  <input type="checkbox" name="ativo" className="form-check-input" checked={jogo.ativo} onChange={handleChange} id="checkAtivo" />
                  <label className="form-check-label" htmlFor="checkAtivo">Jogo Ativo</label>
                </div>
              </div>
            </div>

            <div className="mb-4">
              <label className="form-label d-block">Gêneros</label>
              <div className={`border rounded p-3 ${errosCampos.idsGeneros ? 'border-danger' : ''}`}>
                {generos.map(g => (
                  <div key={g.id} className="form-check form-check-inline">
                    <input type="checkbox" className="form-check-input" id={`genero-${g.id}`} checked={jogo.idsGeneros.includes(g.id)} onChange={() => handleGeneroChange(g.id)} />
                    <label className="form-check-label" htmlFor={`genero-${g.id}`}>{g.nome}</label>
                  </div>
                ))}
              </div>
              {errosCampos.idsGeneros && <div className="text-danger small mt-1">{errosCampos.idsGeneros[0]}</div>}
            </div>

            <div className="d-flex gap-2">
              <button type="submit" className="btn btn-primary" disabled={enviando}>
                {enviando ? 'Salvando...' : 'Salvar Jogo'}
              </button>
              <button type="button" className="btn btn-secondary" onClick={() => navigate('/admin/jogos')}>
                Cancelar
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
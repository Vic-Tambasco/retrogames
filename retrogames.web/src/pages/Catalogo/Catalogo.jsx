import { useEffect, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { jogoApi, plataformaApi, generoApi } from '../../api/servicos';
import { obterMensagemErro } from '../../api/erros';
import CardJogo from '../../components/CardJogo';
import Paginacao from '../../components/Paginacao';
import { Carregando, MensagemErro, EstadoVazio } from '../../components/MensagensEstado';

export default function Catalogo() {
  const [searchParams, setSearchParams] = useSearchParams();
  
  // Lê os filtros da URL
  const busca = searchParams.get('busca') || '';
  const idPlataforma = searchParams.get('idPlataforma') || '';
  const idGenero = searchParams.get('idGenero') || '';
  const pagina = parseInt(searchParams.get('pagina')) || 1;
  const tamanhoPagina = 12;

  // Estados dos dados
  const [resultado, setResultado] = useState(null);
  const [plataformas, setPlataformas] = useState([]);
  const [generos, setGeneros] = useState([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState('');

  // Carrega as listas de apoio (plataformas e gêneros) apenas uma vez
  useEffect(() => {
    Promise.all([plataformaApi.listar(), generoApi.listar()])
      .then(([dadosPlataformas, dadosGeneros]) => {
        setPlataformas(dadosPlataformas);
        setGeneros(dadosGeneros);
      })
      .catch((falha) => console.error("Erro ao carregar filtros", falha));
  }, []);

  // Carrega os jogos sempre que um filtro ou a página mudar
  useEffect(() => {
    setCarregando(true);
    setErro('');

    jogoApi.listar({ busca, idPlataforma, idGenero, pagina, tamanhoPagina })
      .then(setResultado)
      .catch((falha) => setErro(obterMensagemErro(falha)))
      .finally(() => setCarregando(false));
  }, [busca, idPlataforma, idGenero, pagina]);

  // Função auxiliar para mudar a URL
  const atualizarFiltro = (chave, valor) => {
    const novosParams = new URLSearchParams(searchParams);
    if (valor) {
      novosParams.set(chave, valor);
    } else {
      novosParams.delete(chave);
    }
    // Sempre que mudar um filtro, volta para a página 1
    if (chave !== 'pagina') novosParams.set('pagina', '1');
    
    setSearchParams(novosParams);
  };

  return (
    <div className="container">
      <div className="row mb-4">
        <div className="col-md-4 mb-2">
          <input 
            type="text" 
            className="form-control" 
            placeholder="Buscar por nome..."
            value={busca}
            onChange={(e) => atualizarFiltro('busca', e.target.value)}
          />
        </div>
        <div className="col-md-4 mb-2">
          <select className="form-select" value={idPlataforma} onChange={(e) => atualizarFiltro('idPlataforma', e.target.value)}>
            <option value="">Todas as plataformas</option>
            {plataformas.map(p => <option key={p.id} value={p.id}>{p.nome}</option>)}
          </select>
        </div>
        <div className="col-md-4 mb-2">
          <select className="form-select" value={idGenero} onChange={(e) => atualizarFiltro('idGenero', e.target.value)}>
            <option value="">Todos os gêneros</option>
            {generos.map(g => <option key={g.id} value={g.id}>{g.nome}</option>)}
          </select>
        </div>
      </div>

      {carregando && <Carregando />}
      {erro && <MensagemErro mensagem={erro} />}
      
      {!carregando && !erro && resultado && (
        <>
          {resultado.itens.length === 0 ? (
            <EstadoVazio mensagem="Nenhum jogo encontrado com esses filtros." />
          ) : (
            <>
              <div className="row row-cols-1 row-cols-md-2 row-cols-lg-3 row-cols-xl-4 g-4">
                {resultado.itens.map(jogo => (
                  <div className="col" key={jogo.id}>
                    <CardJogo jogo={jogo} />
                  </div>
                ))}
              </div>
              
              <Paginacao 
                pagina={resultado.pagina} 
                totalPaginas={resultado.totalPaginas} 
                aoMudar={(novaPagina) => atualizarFiltro('pagina', novaPagina)} 
              />
            </>
          )}
        </>
      )}
    </div>
  );
}
export function Carregando() {
  return (
    <div className="text-center my-5" role="status">
      <div className="spinner-border text-primary" role="status"></div>
      <p className="mt-2">Carregando...</p>
    </div>
  );
}

export function MensagemErro({ mensagem, onTentarNovamente }) {
  return (
    <div className="alert alert-danger my-4" role="alert">
      <p>{mensagem}</p>
      {onTentarNovamente && (
        <button className="btn btn-outline-danger btn-sm" onClick={onTentarNovamente}>
          Tentar novamente
        </button>
      )}
    </div>
  );
}

export function EstadoVazio({ mensagem = "Nenhum item encontrado." }) {
  return (
    <div className="alert alert-info my-4">
      {mensagem}
    </div>
  );
}
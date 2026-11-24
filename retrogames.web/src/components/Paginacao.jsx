export default function Paginacao({ pagina, totalPaginas, aoMudar }) {
  if (totalPaginas <= 1) return null;

  const paginas = Array.from({ length: totalPaginas }, (_, i) => i + 1);

  return (
    <nav className="mt-4">
      <ul className="pagination justify-content-center">
        <li className={`page-item ${pagina === 1 ? 'disabled' : ''}`}>
          <button className="page-link" onClick={() => aoMudar(pagina - 1)}>Anterior</button>
        </li>
        
        {paginas.map(p => (
          <li key={p} className={`page-item ${p === pagina ? 'active' : ''}`}>
            <button className="page-link" onClick={() => aoMudar(p)}>{p}</button>
          </li>
        ))}

        <li className={`page-item ${pagina === totalPaginas ? 'disabled' : ''}`}>
          <button className="page-link" onClick={() => aoMudar(pagina + 1)}>Próxima</button>
        </li>
      </ul>
    </nav>
  );
}
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { api, urlUpload } from "../api";
import { Vazio, Avatar } from "../components/UI";
import { idade } from "../utils/format";

export default function PacientesLista() {
  const [busca, setBusca] = useState("");
  const [pacientes, setPacientes] = useState([]);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    let ativo = true;
    setCarregando(true);
    const timer = setTimeout(() => {
      api.get(`/pacientes?q=${encodeURIComponent(busca)}`).then((dados) => {
        if (ativo) { setPacientes(dados); setCarregando(false); }
      });
    }, 250);
    return () => { ativo = false; clearTimeout(timer); };
  }, [busca]);

  return (
    <div>
      <div className="pagina-cabecalho">
        <div>
          <h1>Pacientes</h1>
          <p className="pagina-subtitulo">{carregando ? "carregando..." : `${pacientes.length} paciente(s) cadastrado(s)`}</p>
        </div>
        <Link to="/pacientes/novo" className="botao botao-primario botao-grande">➕ Novo paciente</Link>
      </div>

      <form className="busca-form" role="search" onSubmit={(e) => e.preventDefault()}>
        <input type="text" value={busca} onChange={(e) => setBusca(e.target.value)} placeholder="Buscar paciente pelo nome..." aria-label="Buscar paciente" />
      </form>

      <div className="cartao">
        {pacientes.length ? (
          <ul className="link-lista">
            {pacientes.map((p) => (
              <li key={p.id} style={{ padding: ".75rem 0", borderBottom: "1px solid var(--cor-borda)" }}>
                <Link to={`/pacientes/${p.id}`} className="linha-paciente">
                  <Avatar src={urlUpload("pacientes", p.foto)} nome={p.nome} />
                  <span>
                    <span className="linha-paciente-nome">{p.nome}</span><br />
                    <span className="linha-paciente-info">
                      {p.data_nascimento ? `${idade(p.data_nascimento)} anos · ` : ""}
                      {p.telefone || "sem telefone cadastrado"}
                    </span>
                  </span>
                </Link>
              </li>
            ))}
          </ul>
        ) : (
          !carregando && <Vazio icone="🗂️">{busca ? `Nenhum paciente encontrado para "${busca}".` : "Nenhum paciente cadastrado ainda."}</Vazio>
        )}
      </div>
    </div>
  );
}

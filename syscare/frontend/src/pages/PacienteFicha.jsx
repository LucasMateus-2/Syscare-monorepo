import { useCallback, useEffect, useState } from "react";
import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { api, urlUpload } from "../api";
import { Avatar } from "../components/UI";
import { brData, idade } from "../utils/format";
import { useFlash } from "../context/FlashContext";

import AbaDados from "./FichaAbas/Dados";
import AbaFeridas from "./FichaAbas/Feridas";
import AbaDiagnosticos from "./FichaAbas/Diagnosticos";
import AbaPrescricoes from "./FichaAbas/Prescricoes";
import AbaAgenda from "./FichaAbas/AgendaAba";

const ABAS = [
  ["dados", "Dados & Saúde"],
  ["feridas", "Feridas"],
  ["diagnosticos", "Diagnósticos"],
  ["prescricoes", "Prescrições"],
  ["agenda", "Agendamentos"],
];

export default function PacienteFicha() {
  const { id } = useParams();
  const [searchParams, setSearchParams] = useSearchParams();
  const aba = searchParams.get("aba") || "dados";
  const navigate = useNavigate();
  const { setFlash } = useFlash();

  const [ficha, setFicha] = useState(null);
  const [carregando, setCarregando] = useState(true);

  const recarregar = useCallback(async () => {
    const dados = await api.get(`/pacientes/${id}`);
    setFicha(dados);
    setCarregando(false);
  }, [id]);

  useEffect(() => { setCarregando(true); recarregar(); }, [recarregar]);

  async function trocarFoto(e) {
    const arquivo = e.target.files[0];
    if (!arquivo) return;
    const formData = new FormData();
    formData.append("foto", arquivo);
    await api.postForm(`/pacientes/${id}/foto`, formData);
    setFlash("Foto atualizada.");
    recarregar();
  }

  if (carregando || !ficha) return <p className="texto-suave">Carregando ficha...</p>;
  const { paciente } = ficha;

  return (
    <div>
      <button className="voltar-link" onClick={() => navigate("/pacientes")} type="button">← Voltar para pacientes</button>

      <div className="paciente-topo">
        <Avatar src={urlUpload("pacientes", paciente.foto)} nome={paciente.nome} grande />
        <div className="paciente-topo-dados">
          <h1>{paciente.nome}</h1>
          <p className="paciente-topo-meta">
            {paciente.data_nascimento ? `${idade(paciente.data_nascimento)} anos (${brData(paciente.data_nascimento)}) · ` : ""}
            {paciente.sexo || ""}{paciente.telefone ? ` · ${paciente.telefone}` : ""}
          </p>
          <label className="foto-upload-form botao botao-secundario botao-pequeno" style={{ cursor: "pointer", display: "inline-flex" }}>
            Atualizar foto
            <input type="file" accept="image/*" onChange={trocarFoto} style={{ display: "none" }} />
          </label>
        </div>
      </div>

      <nav className="abas" aria-label="Seções do prontuário">
        {ABAS.map(([chave, rotulo]) => (
          <button
            key={chave}
            type="button"
            className={`aba-link ${aba === chave ? "ativa" : ""}`}
            onClick={() => setSearchParams({ aba: chave })}
          >
            {rotulo}
          </button>
        ))}
      </nav>

      {aba === "dados" && <AbaDados ficha={ficha} recarregar={recarregar} pacienteId={id} />}
      {aba === "feridas" && <AbaFeridas ficha={ficha} recarregar={recarregar} pacienteId={id} />}
      {aba === "diagnosticos" && <AbaDiagnosticos ficha={ficha} recarregar={recarregar} pacienteId={id} />}
      {aba === "prescricoes" && <AbaPrescricoes ficha={ficha} recarregar={recarregar} pacienteId={id} />}
      {aba === "agenda" && <AbaAgenda ficha={ficha} pacienteId={id} />}
    </div>
  );
}

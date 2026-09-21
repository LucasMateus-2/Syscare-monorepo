import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { api } from "../../api";
import { Vazio, Selo } from "../../components/UI";
import { brData } from "../../utils/format";

export default function AbaAgenda({ pacienteId }) {
  const [agendamentos, setAgendamentos] = useState([]);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    api.get(`/agenda/paciente/${pacienteId}`).then((dados) => { setAgendamentos(dados); setCarregando(false); });
  }, [pacienteId]);

  return (
    <div className="cartao">
      <div className="cartao-titulo">
        <h2>Próximos agendamentos</h2>
        <Link to="/agenda" className="botao botao-primario">📅 Ir para a agenda</Link>
      </div>
      {!carregando && (agendamentos.length ? agendamentos.map((a) => (
        <div className="slot-agendamento" key={a.id}>
          <div className="slot-hora">{brData(a.data)}<br /><span className="texto-suave" style={{ fontWeight: 400, fontSize: ".8rem" }}>{a.hora}</span></div>
          <div className="slot-info"><strong>{a.procedimento || "Atendimento"}</strong><br /><span className="texto-suave">{a.observacoes || ""}</span></div>
          <Selo status={a.status} />
        </div>
      )) : (
        <Vazio icone="🗓️">
          Nenhum agendamento futuro para este paciente.<br />
          <Link to="/agenda" className="botao botao-secundario" style={{ marginTop: "1rem" }}>Agendar atendimento</Link>
        </Vazio>
      ))}
    </div>
  );
}

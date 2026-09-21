import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { api } from "../api";
import { useAuth } from "../context/AuthContext";
import { Vazio, Avatar, Selo } from "../components/UI";
import { urlUpload } from "../api";
import { brData, hojeISO } from "../utils/format";

export default function Dashboard() {
  const { admin } = useAuth();
  const [carregando, setCarregando] = useState(true);
  const [agsHoje, setAgsHoje] = useState([]);
  const [totalPacientes, setTotalPacientes] = useState(0);
  const [totalFeridas, setTotalFeridas] = useState(0);
  const [proximos, setProximos] = useState([]);

  useEffect(() => {
    async function carregar() {
      const hoje = hojeISO();
      const [agendaHoje, pacientes] = await Promise.all([
        api.get(`/agenda?data=${hoje}`),
        api.get("/pacientes"),
      ]);
      setAgsHoje(agendaHoje.agendamentos.filter((a) => a.status !== "cancelado"));
      setTotalPacientes(pacientes.length);

      // Próximos agendamentos (próximos 7 dias, além de hoje) — busca simples dia a dia
      const proximasDatas = [];
      const base = new Date(hoje + "T00:00:00");
      for (let i = 1; i <= 14 && proximasDatas.length < 6; i++) {
        const d = new Date(base);
        d.setDate(d.getDate() + i);
        const iso = d.toISOString().slice(0, 10);
        const resp = await api.get(`/agenda?data=${iso}`);
        resp.agendamentos.filter((a) => a.status !== "cancelado").forEach((a) => proximasDatas.push(a));
      }
      setProximos(proximasDatas.slice(0, 6));

      // Total de feridas ativas: soma via lista de pacientes (uma chamada por paciente seria custoso;
      // para o dashboard usamos uma estimativa simples chamando cada ficha só se houver poucos pacientes)
      let totalF = 0;
      if (pacientes.length <= 30) {
        const fichas = await Promise.all(pacientes.map((p) => api.get(`/pacientes/${p.id}`)));
        fichas.forEach((f) => { totalF += f.feridas.filter((x) => x.ativa).length; });
      }
      setTotalFeridas(totalF);
      setCarregando(false);
    }
    carregar();
  }, []);

  return (
    <div>
      <div className="pagina-cabecalho">
        <div>
          <h1>Olá, {admin?.nome} 👋</h1>
          <p className="pagina-subtitulo">Aqui está o resumo de hoje.</p>
        </div>
        <Link to="/agenda" className="botao botao-primario botao-grande">Ver agenda completa</Link>
      </div>

      <div className="grade grade-3" style={{ marginBottom: "1.5rem" }}>
        <div className="stat-cartao"><div className="stat-numero">{carregando ? "…" : agsHoje.length}</div><div className="stat-legenda">Atendimentos hoje</div></div>
        <div className="stat-cartao"><div className="stat-numero">{carregando ? "…" : totalPacientes}</div><div className="stat-legenda">Pacientes cadastrados</div></div>
        <div className="stat-cartao"><div className="stat-numero">{carregando ? "…" : totalFeridas}</div><div className="stat-legenda">Feridas em acompanhamento</div></div>
      </div>

      <div className="grade grade-2">
        <div className="cartao">
          <h2>Atendimentos de hoje</h2>
          {agsHoje.length ? agsHoje.map((a) => (
            <div className="slot-agendamento" key={a.id}>
              <div className="slot-hora">{a.hora}</div>
              <Avatar src={urlUpload("pacientes", a.paciente_foto)} nome={a.paciente_nome || a.nome_paciente_avulso} />
              <div className="slot-info">
                <strong>{a.paciente_nome || a.nome_paciente_avulso || "Paciente"}</strong><br />
                <span className="texto-suave">{a.procedimento || "Atendimento"}</span>
              </div>
              <Selo status={a.status} />
            </div>
          )) : !carregando && <Vazio icone="📅">Nenhum atendimento agendado para hoje.</Vazio>}
        </div>

        <div className="cartao">
          <h2>Próximos agendamentos</h2>
          {proximos.length ? proximos.map((a) => (
            <div className="slot-agendamento" key={a.id}>
              <div className="slot-hora">{brData(a.data)}<br /><span className="texto-suave" style={{ fontWeight: 400, fontSize: ".8rem" }}>{a.hora}</span></div>
              <div className="slot-info">
                <strong>{a.paciente_nome || a.nome_paciente_avulso || "Paciente"}</strong><br />
                <span className="texto-suave">{a.procedimento || "Atendimento"}</span>
              </div>
            </div>
          )) : !carregando && <Vazio icone="🗓️">Nada agendado nos próximos dias.</Vazio>}
        </div>
      </div>
    </div>
  );
}

import { useCallback, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { api, urlUpload } from "../api";
import { Vazio, Avatar } from "../components/UI";
import { useFlash } from "../context/FlashContext";
import { MESES, DIAS_SEMANA, hojeISO } from "../utils/format";

function diaOffset(iso, offset) {
  const d = new Date(iso + "T00:00:00");
  d.setDate(d.getDate() + offset);
  return d.toISOString().slice(0, 10);
}

function gerarSemanas(ano, mes) {
  const primeiroDiaSemana = new Date(ano, mes - 1, 1).getDay();
  const diasNoMes = new Date(ano, mes, 0).getDate();
  const celulas = [];
  for (let i = 0; i < primeiroDiaSemana; i++) celulas.push(null);
  for (let d = 1; d <= diasNoMes; d++) celulas.push(d);
  while (celulas.length % 7 !== 0) celulas.push(null);
  const semanas = [];
  for (let i = 0; i < celulas.length; i += 7) semanas.push(celulas.slice(i, i + 7));
  return semanas;
}

const VAZIO_FORM = { paciente_id: "", nome_paciente_avulso: "", telefone_avulso: "", data: hojeISO(), hora: "", duracao_min: 30, procedimento: "", observacoes: "" };

export default function Agenda() {
  const [dataAtual, setDataAtual] = useState(hojeISO());
  const [agendamentos, setAgendamentos] = useState([]);
  const [contagemPorDia, setContagemPorDia] = useState({});
  const [painelAberto, setPainelAberto] = useState(false);
  const [form, setForm] = useState(VAZIO_FORM);
  const [buscaPaciente, setBuscaPaciente] = useState("");
  const [resultadosBusca, setResultadosBusca] = useState([]);
  const { setFlash } = useFlash();

  const [ano, mes, diaNum] = dataAtual.split("-").map(Number);
  const dataObj = new Date(dataAtual + "T00:00:00");

  const recarregar = useCallback(async () => {
    const r = await api.get(`/agenda?data=${dataAtual}`);
    setAgendamentos(r.agendamentos);
    setContagemPorDia(r.contagemPorDia);
  }, [dataAtual]);

  useEffect(() => { recarregar(); }, [recarregar]);

  useEffect(() => {
    if (buscaPaciente.trim().length < 2) { setResultadosBusca([]); return; }
    const t = setTimeout(() => {
      api.get(`/agenda/buscar-pacientes?q=${encodeURIComponent(buscaPaciente)}`).then(setResultadosBusca);
    }, 250);
    return () => clearTimeout(t);
  }, [buscaPaciente]);

  async function mudarStatus(id, status) {
    await api.put(`/agenda/${id}/status`, { status });
    recarregar();
  }

  async function excluir(id) {
    if (!window.confirm("Excluir este agendamento definitivamente?")) return;
    await api.delete(`/agenda/${id}`);
    recarregar();
  }

  async function salvarAgendamento(e) {
    e.preventDefault();
    await api.post("/agenda", form);
    setFlash("Agendamento criado.");
    setForm({ ...VAZIO_FORM, data: dataAtual });
    setBuscaPaciente("");
    setPainelAberto(false);
    recarregar();
  }

  const semanas = gerarSemanas(ano, mes);

  return (
    <div>
      <div className="pagina-cabecalho">
        <h1>Agenda</h1>
        <button type="button" className="botao botao-primario botao-grande" onClick={() => { setForm({ ...VAZIO_FORM, data: dataAtual }); setPainelAberto(true); }}>➕ Novo agendamento</button>
      </div>

      <div className="agenda-layout">
        <div className="cartao">
          <h3 style={{ textAlign: "center" }}>{MESES[mes]} de {ano}</h3>
          <table className="mini-calendario">
            <thead><tr>{["D", "S", "T", "Q", "Q", "S", "S"].map((d, i) => <th key={i}>{d}</th>)}</tr></thead>
            <tbody>
              {semanas.map((semana, i) => (
                <tr key={i}>
                  {semana.map((d, j) => {
                    if (!d) return <td key={j}></td>;
                    const iso = `${ano}-${String(mes).padStart(2, "0")}-${String(d).padStart(2, "0")}`;
                    const classes = ["", iso === dataAtual ? "dia-selecionado" : "", contagemPorDia[iso] ? "dia-com-eventos" : ""].join(" ");
                    return <td key={j}><button className={classes} onClick={() => setDataAtual(iso)}>{d}</button></td>;
                  })}
                </tr>
              ))}
            </tbody>
          </table>
          <div style={{ textAlign: "center", marginTop: ".6rem" }}>
            <button className="botao botao-fantasma botao-pequeno" onClick={() => setDataAtual(hojeISO())}>Ir para hoje</button>
          </div>
        </div>

        <div>
          <div className="agenda-nav-dia">
            <button className="botao botao-fantasma botao-pequeno" onClick={() => setDataAtual(diaOffset(dataAtual, -1))}>← Dia anterior</button>
            <h2>{DIAS_SEMANA[dataObj.getDay()]}, {diaNum} de {MESES[mes]}</h2>
            <button className="botao botao-fantasma botao-pequeno" onClick={() => setDataAtual(diaOffset(dataAtual, 1))}>Próximo dia →</button>
          </div>

          <div className="cartao">
            {agendamentos.length ? agendamentos.map((a) => (
              <div className="slot-agendamento" key={a.id}>
                <div className="slot-hora">{a.hora}</div>
                <Avatar src={urlUpload("pacientes", a.paciente_foto)} nome={a.paciente_nome || a.nome_paciente_avulso} />
                <div className="slot-info">
                  {a.paciente_id ? <Link to={`/pacientes/${a.paciente_id}`}><strong>{a.paciente_nome}</strong></Link> : <strong>{a.nome_paciente_avulso || "Paciente avulso"}</strong>}
                  <br />
                  <span className="texto-suave">{a.procedimento || "Atendimento"} · {a.duracao_min} min{a.paciente_telefone ? ` · ${a.paciente_telefone}` : ""}{a.telefone_avulso ? ` · ${a.telefone_avulso}` : ""}</span>
                  {a.observacoes && <><br /><span className="texto-suave">{a.observacoes}</span></>}
                </div>
                <span className={`selo selo-${a.status}`}>{a.status}</span>
                <div className="slot-acoes">
                  {a.status !== "concluido" && a.status !== "cancelado" && (
                    <>
                      <button className="botao botao-secundario botao-pequeno" onClick={() => mudarStatus(a.id, "confirmado")}>Confirmar</button>
                      <button className="botao botao-secundario botao-pequeno" onClick={() => mudarStatus(a.id, "concluido")}>Concluído</button>
                      <button className="botao botao-secundario botao-pequeno" onClick={() => mudarStatus(a.id, "faltou")}>Faltou</button>
                      <button className="botao botao-perigo botao-pequeno" onClick={() => mudarStatus(a.id, "cancelado")}>Cancelar</button>
                    </>
                  )}
                  <button className="botao botao-perigo botao-pequeno" onClick={() => excluir(a.id)}>🗑</button>
                </div>
              </div>
            )) : <Vazio icone="📅">Nenhum agendamento para este dia.</Vazio>}
          </div>

          {painelAberto && (
            <div className="cartao">
              <h3>Novo agendamento</h3>
              <form onSubmit={salvarAgendamento}>
                <div className="campo busca-paciente-wrap">
                  <label>Paciente cadastrado (opcional)</label>
                  <input type="text" placeholder="Digite o nome para buscar..." autoComplete="off" value={buscaPaciente}
                    onChange={(e) => { setBuscaPaciente(e.target.value); setForm({ ...form, paciente_id: "" }); }} />
                  {resultadosBusca.length > 0 && (
                    <div className="resultados-paciente" style={{ display: "block" }}>
                      {resultadosBusca.map((p) => (
                        <div className="resultado-paciente-item" key={p.id} onClick={() => { setBuscaPaciente(p.nome); setForm({ ...form, paciente_id: p.id }); setResultadosBusca([]); }}>
                          {p.nome}{p.telefone ? ` · ${p.telefone}` : ""}
                        </div>
                      ))}
                    </div>
                  )}
                </div>
                <p className="texto-suave" style={{ margin: "-.5rem 0 1rem" }}>Ou preencha abaixo para um paciente avulso (sem cadastro):</p>
                <div className="campos-linha">
                  <div className="campo"><label>Nome</label><input type="text" value={form.nome_paciente_avulso} onChange={(e) => setForm({ ...form, nome_paciente_avulso: e.target.value })} /></div>
                  <div className="campo"><label>Telefone</label><input type="tel" value={form.telefone_avulso} onChange={(e) => setForm({ ...form, telefone_avulso: e.target.value })} /></div>
                </div>
                <div className="campos-linha">
                  <div className="campo"><label>Data</label><input type="date" required value={form.data} onChange={(e) => setForm({ ...form, data: e.target.value })} /></div>
                  <div className="campo"><label>Hora</label><input type="time" required value={form.hora} onChange={(e) => setForm({ ...form, hora: e.target.value })} /></div>
                  <div className="campo"><label>Duração (min)</label><input type="number" min="5" step="5" value={form.duracao_min} onChange={(e) => setForm({ ...form, duracao_min: e.target.value })} /></div>
                </div>
                <div className="campo"><label>Procedimento</label><input type="text" placeholder="Ex: Curativo, avaliação, retorno..." value={form.procedimento} onChange={(e) => setForm({ ...form, procedimento: e.target.value })} /></div>
                <div className="campo"><label>Observações</label><textarea value={form.observacoes} onChange={(e) => setForm({ ...form, observacoes: e.target.value })}></textarea></div>
                <div className="acoes-form">
                  <button type="submit" className="botao botao-primario botao-grande">Salvar agendamento</button>
                  <button type="button" className="botao botao-fantasma" onClick={() => setPainelAberto(false)}>Cancelar</button>
                </div>
              </form>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

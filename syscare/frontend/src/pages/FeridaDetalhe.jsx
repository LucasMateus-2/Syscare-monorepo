import { useCallback, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { api } from "../api";
import { useFlash } from "../context/FlashContext";
import { brData, hojeISO } from "../utils/format";
import diagnosticosData from "../utils/diagnosticosData";

const { ITB_INTERPRETACAO } = diagnosticosData;

function interpretarITB(valor) {
  if (!valor) return "";
  const v = parseFloat(valor);
  if (Number.isNaN(v)) return "";
  const achado = ITB_INTERPRETACAO.find(([lo, hi]) => v >= lo && v <= hi);
  return achado ? achado[2] : "";
}

export default function FeridaDetalhe() {
  const { id, feridaId } = useParams();
  const navigate = useNavigate();
  const { setFlash } = useFlash();
  const [dados, setDados] = useState(null);
  const [form, setForm] = useState({ data: hojeISO(), comprimento: "", largura: "", profundidade: "", exsudato_quantidade: "", observacoes: "" });
  const [foto, setFoto] = useState(null);

  const recarregar = useCallback(async () => {
    const r = await api.get(`/pacientes/${id}/feridas/${feridaId}`);
    setDados(r);
  }, [id, feridaId]);

  useEffect(() => { recarregar(); }, [recarregar]);

  async function encerrar() {
    if (!window.confirm("Marcar esta ferida como cicatrizada/encerrada?")) return;
    await api.post(`/pacientes/${id}/feridas/${feridaId}/encerrar`, {});
    recarregar();
  }

  async function salvarEvolucao(e) {
    e.preventDefault();
    const formData = new FormData();
    Object.entries(form).forEach(([k, v]) => formData.append(k, v || ""));
    if (foto) formData.append("foto", foto);
    await api.postForm(`/pacientes/${id}/feridas/${feridaId}/evolucoes`, formData);
    setFlash("Evolução registrada.");
    setForm({ data: hojeISO(), comprimento: "", largura: "", profundidade: "", exsudato_quantidade: "", observacoes: "" });
    setFoto(null);
    recarregar();
  }

  if (!dados) return <p className="texto-suave">Carregando...</p>;
  const { ferida, evolucoes } = dados;
  const itb = ferida.itb_dados || {};

  return (
    <div>
      <button className="voltar-link" onClick={() => navigate(`/pacientes/${id}?aba=feridas`)} type="button">← Voltar para feridas</button>
      <div className="pagina-cabecalho">
        <div><h1>{ferida.numero_lesao ? `Lesão ${ferida.numero_lesao}` : "Avaliação de ferida"}</h1><p className="pagina-subtitulo">{ferida.localizacao || "Local não informado"} · registrada em {brData(ferida.criado_em)}</p></div>
        {ferida.ativa ? (
          <button type="button" className="botao botao-secundario" onClick={encerrar}>✔ Marcar como cicatrizada</button>
        ) : <span className="selo selo-cancelado">Encerrada</span>}
      </div>

      <div className="grade grade-2">
        <div className="cartao">
          <h2>Avaliação inicial</h2>
          {ferida.foto && <img src={`/uploads/feridas/${ferida.foto}`} alt="Foto da ferida" style={{ width: "100%", maxWidth: 320, borderRadius: 10, marginBottom: "1rem" }} />}
          <table>
            <tbody>
              <tr><th>Etiologia</th><td>{ferida.etiologia || "—"}</td></tr>
              <tr><th>Tempo de ferida</th><td>{ferida.tempo_ferida || "—"}</td></tr>
              <tr><th>Dimensões</th><td>C: {ferida.comprimento || "—"} · L: {ferida.largura || "—"} · P: {ferida.profundidade || "—"} · Descolamento: {ferida.descolamento || "—"}</td></tr>
            </tbody>
          </table>

          <h3 style={{ marginTop: "1.25rem" }}>TIME</h3>
          <p><strong>T (tecido):</strong> {(ferida.time_tecido || []).join(", ") || "—"}</p>
          <p><strong>I (infecção):</strong> {(ferida.time_infeccao || []).join(", ") || "—"}</p>
          <p><strong>M (exsudato):</strong> {ferida.exsudato_tipo || "—"} — quantidade: {ferida.exsudato_quantidade || "—"}</p>
          <p><strong>E (bordas):</strong> {(ferida.bordas || []).join(", ") || "—"}</p>
          <p><strong>Perilesional:</strong> {(ferida.perilesional || []).join(", ") || "—"}</p>

          <h3 style={{ marginTop: "1.25rem" }}>Biofilme</h3>
          <p>{ferida.biofilme === "sim" ? "Sim" : "Não identificado"}</p>
          {ferida.biofilme === "sim" && (
            <div className="chip-lista">{(ferida.biofilme_sinais || []).map((s) => <span className="chip" key={s}>{s}</span>)}</div>
          )}

          <h3 style={{ marginTop: "1.25rem" }}>Índice tornozelo-braquial</h3>
          <table>
            <tbody>
              <tr><th>ITB direito</th><td>{itb.resultado_dir || "—"} {interpretarITB(itb.resultado_dir) && <span className="selo selo-confirmado">{interpretarITB(itb.resultado_dir)}</span>}</td></tr>
              <tr><th>ITB esquerdo</th><td>{itb.resultado_esq || "—"} {interpretarITB(itb.resultado_esq) && <span className="selo selo-confirmado">{interpretarITB(itb.resultado_esq)}</span>}</td></tr>
            </tbody>
          </table>

          <h3 style={{ marginTop: "1.25rem" }}>Sensibilidade</h3>
          <p>{ferida.sensibilidade || "—"}{ferida.sensibilidade_obs ? ` — ${ferida.sensibilidade_obs}` : ""}</p>
        </div>

        <div className="cartao">
          <div className="cartao-titulo"><h2>Evolução ao longo do tempo</h2></div>
          {evolucoes.length ? (
            <div className="tabela-wrap">
              <table>
                <thead><tr><th>Data</th><th>C×L×P</th><th>Exsudato</th><th>Obs.</th><th>Foto</th></tr></thead>
                <tbody>
                  {evolucoes.map((e) => (
                    <tr key={e.id}>
                      <td>{brData(e.data)}</td>
                      <td>{e.comprimento || "—"}×{e.largura || "—"}×{e.profundidade || "—"}</td>
                      <td>{e.exsudato_quantidade || "—"}</td>
                      <td>{e.observacoes || "—"}</td>
                      <td>{e.foto ? <img src={`/uploads/feridas/${e.foto}`} alt="" style={{ width: 44, height: 44, objectFit: "cover", borderRadius: 6 }} /> : "—"}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ) : <p className="texto-suave">Nenhuma evolução registrada ainda.</p>}

          {ferida.ativa && (
            <>
              <hr className="divisor" />
              <h3>Registrar nova evolução</h3>
              <form onSubmit={salvarEvolucao}>
                <div className="campos-linha">
                  <div className="campo"><label>Data</label><input type="date" value={form.data} onChange={(e) => setForm({ ...form, data: e.target.value })} /></div>
                  <div className="campo"><label>Comprimento</label><input type="text" value={form.comprimento} onChange={(e) => setForm({ ...form, comprimento: e.target.value })} /></div>
                  <div className="campo"><label>Largura</label><input type="text" value={form.largura} onChange={(e) => setForm({ ...form, largura: e.target.value })} /></div>
                  <div className="campo"><label>Profundidade</label><input type="text" value={form.profundidade} onChange={(e) => setForm({ ...form, profundidade: e.target.value })} /></div>
                </div>
                <div className="campo">
                  <label>Exsudato (quantidade)</label>
                  <select value={form.exsudato_quantidade} onChange={(e) => setForm({ ...form, exsudato_quantidade: e.target.value })}>
                    <option value="">Selecione</option>
                    {["Ausente", "Pequena", "Moderada", "Grande"].map((op) => <option key={op} value={op}>{op}</option>)}
                  </select>
                </div>
                <div className="campo"><label>Observações</label><textarea value={form.observacoes} onChange={(e) => setForm({ ...form, observacoes: e.target.value })}></textarea></div>
                <div className="campo"><label>Foto</label><input type="file" accept="image/*" onChange={(e) => setFoto(e.target.files[0] || null)} /></div>
                <button type="submit" className="botao botao-primario">Adicionar evolução</button>
              </form>
            </>
          )}
        </div>
      </div>
    </div>
  );
}

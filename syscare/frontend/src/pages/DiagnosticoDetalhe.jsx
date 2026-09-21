import { useCallback, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { api } from "../api";
import { useFlash } from "../context/FlashContext";
import { brData, hojeISO } from "../utils/format";
import diagnosticosData from "../utils/diagnosticosData";

const { DIAGNOSTICOS } = diagnosticosData;

export default function DiagnosticoDetalhe() {
  const { id, chave } = useParams();
  const navigate = useNavigate();
  const { setFlash } = useFlash();
  const template = DIAGNOSTICOS[chave];

  const [avaliacoes, setAvaliacoes] = useState([]);
  const [atividadesMap, setAtividadesMap] = useState({});
  const [dataAvaliacao, setDataAvaliacao] = useState(hojeISO());
  const [valoresForm, setValoresForm] = useState({});
  const [carregando, setCarregando] = useState(true);

  const recarregar = useCallback(async () => {
    const r = await api.get(`/pacientes/${id}/diagnosticos/${chave}`);
    setAvaliacoes(r.avaliacoes);
    setAtividadesMap(r.atividadesMap);
    setCarregando(false);
  }, [id, chave]);

  useEffect(() => { recarregar(); }, [recarregar]);

  if (!template) return <p>Diagnóstico não encontrado.</p>;

  async function salvarAvaliacao(e) {
    e.preventDefault();
    await api.post(`/pacientes/${id}/diagnosticos/${chave}/avaliacoes`, { data: dataAvaliacao, valores: valoresForm });
    setFlash("Avaliação registrada.");
    setValoresForm({});
    recarregar();
  }

  async function marcarFeito(atividade, data) {
    await api.post(`/pacientes/${id}/diagnosticos/${chave}/atividades`, { atividade, data });
    recarregar();
  }

  return (
    <div>
      <button className="voltar-link" onClick={() => navigate(`/pacientes/${id}?aba=diagnosticos`)} type="button">← Voltar para diagnósticos</button>
      <div className="pagina-cabecalho"><div><h1>{template.nome}</h1><p className="pagina-subtitulo">{template.definicao}</p></div></div>

      {(template.caracteristicas_definidoras || template.fatores_relacionados) && (
        <details className="cartao">
          <summary style={{ cursor: "pointer", fontWeight: 700 }}>Ver características definidoras e fatores relacionados (referência)</summary>
          <div className="grade grade-2" style={{ marginTop: "1rem" }}>
            {template.caracteristicas_definidoras && (
              <div><h3>Características definidoras</h3><ul>{template.caracteristicas_definidoras.map((c) => <li key={c}>{c}</li>)}</ul></div>
            )}
            {template.fatores_relacionados && (
              <div><h3>Fatores relacionados</h3><ul>{template.fatores_relacionados.map((c) => <li key={c}>{c}</li>)}</ul></div>
            )}
          </div>
        </details>
      )}

      <div className="cartao">
        <div className="cartao-titulo"><h2>Resultados esperados (NOC): {template.noc_titulo}</h2></div>
        <h3>Registrar nova avaliação</h3>
        <form onSubmit={salvarAvaliacao}>
          <div className="campo" style={{ maxWidth: 220 }}>
            <label>Data da avaliação</label>
            <input type="date" value={dataAvaliacao} onChange={(e) => setDataAvaliacao(e.target.value)} />
          </div>
          {template.noc_escala ? (
            <p className="campo-ajuda">Escala: {template.noc_escala.map((e, i) => `${i + 1} = ${e}`).join(", ")}</p>
          ) : <p className="campo-ajuda">Escala de 1 (menor) a 5 (maior).</p>}

          {template.noc_indicadores.map((ind) => (
            <div className="campo" key={ind}>
              <label>{ind}</label>
              <div className="escala-noc">
                {[1, 2, 3, 4, 5].map((n) => (
                  <label key={n} style={{ margin: 0 }}>
                    <input type="radio" name={`ind_${ind}`} checked={valoresForm[ind] === String(n)} onChange={() => setValoresForm({ ...valoresForm, [ind]: String(n) })} style={{ display: "none" }} />
                    <span
                      onClick={() => setValoresForm({ ...valoresForm, [ind]: String(n) })}
                      style={{
                        display: "flex", alignItems: "center", justifyContent: "center", width: 34, height: 34,
                        borderRadius: "50%", border: "1.5px solid var(--cor-borda)", cursor: "pointer", fontWeight: 700, fontSize: ".85rem",
                        background: valoresForm[ind] === String(n) ? "var(--cor-primaria)" : "transparent",
                        color: valoresForm[ind] === String(n) ? "#fff" : "inherit",
                        borderColor: valoresForm[ind] === String(n) ? "var(--cor-primaria)" : "var(--cor-borda)",
                      }}
                    >{n}</span>
                  </label>
                ))}
              </div>
            </div>
          ))}
          <div className="acoes-form"><button type="submit" className="botao botao-primario">Salvar avaliação</button></div>
        </form>

        {!carregando && avaliacoes.length > 0 && (
          <>
            <hr className="divisor" />
            <h3>Histórico</h3>
            <div className="tabela-wrap">
              <table>
                <thead><tr><th>Indicador</th>{avaliacoes.map((a) => <th key={a.id}>{brData(a.data)}</th>)}</tr></thead>
                <tbody>
                  {template.noc_indicadores.map((ind) => (
                    <tr key={ind}><td>{ind}</td>{avaliacoes.map((a) => <td key={a.id}>{(a.valores || {})[ind] || "—"}</td>)}</tr>
                  ))}
                </tbody>
              </table>
            </div>
          </>
        )}
      </div>

      <div className="cartao">
        <div className="cartao-titulo"><h2>Intervenções de enfermagem (NIC): {template.nic_titulo}</h2></div>
        <table>
          <thead><tr><th>Atividade</th><th>Datas realizadas</th><th></th></tr></thead>
          <tbody>
            {template.nic_atividades.map((a) => (
              <LinhaAtividade key={a} atividade={a} datas={atividadesMap[a] || []} onMarcar={marcarFeito} />
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

function LinhaAtividade({ atividade, datas, onMarcar }) {
  const [data, setData] = useState(hojeISO());
  return (
    <tr>
      <td style={{ maxWidth: 420 }}>{atividade}</td>
      <td>{datas.length ? <div className="chip-lista">{datas.map((d) => <span className="chip" key={d}>{brData(d)}</span>)}</div> : <span className="texto-suave">—</span>}</td>
      <td>
        <div style={{ display: "flex", gap: ".4rem", alignItems: "center" }}>
          <input type="date" value={data} onChange={(e) => setData(e.target.value)} style={{ width: 150 }} />
          <button type="button" className="botao botao-secundario botao-pequeno" onClick={() => onMarcar(atividade, data)}>Marcar feito</button>
        </div>
      </td>
    </tr>
  );
}

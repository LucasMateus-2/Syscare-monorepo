import { useState } from "react";
import { api } from "../../api";
import { useFlash } from "../../context/FlashContext";
import { Vazio } from "../../components/UI";
import { brData, hojeISO } from "../../utils/format";
import diagnosticosData from "../../utils/diagnosticosData";

const { COBRIR_COM_OPCOES } = diagnosticosData;

const VAZIO = {
  data: hojeISO(), aos_cuidados_de: "", limpeza_sf09: false, phmb: false, creme_barreira: false,
  cobertura: "", cobrir_com: [], frequencia_troca: "", observacoes: "",
};

export default function AbaPrescricoes({ ficha, recarregar, pacienteId }) {
  const [painelAberto, setPainelAberto] = useState(false);
  const [form, setForm] = useState(VAZIO);
  const { setFlash } = useFlash();

  function toggleCobrirCom(op) {
    setForm((f) => ({
      ...f,
      cobrir_com: f.cobrir_com.includes(op) ? f.cobrir_com.filter((v) => v !== op) : [...f.cobrir_com, op],
    }));
  }

  async function salvar(e) {
    e.preventDefault();
    await api.post(`/pacientes/${pacienteId}/prescricoes`, form);
    setFlash("Prescrição de curativo registrada.");
    setForm(VAZIO);
    setPainelAberto(false);
    recarregar();
  }

  return (
    <div className="cartao">
      <div className="cartao-titulo">
        <h2>Prescrições de curativo</h2>
        <button type="button" className="botao botao-primario" onClick={() => setPainelAberto((v) => !v)}>➕ Nova prescrição</button>
      </div>

      {ficha.prescricoes.length ? (
        <div className="tabela-wrap">
          <table>
            <thead><tr><th>Data</th><th>Cobertura</th><th>Troca a cada</th><th></th></tr></thead>
            <tbody>
              {ficha.prescricoes.map((p) => (
                <tr key={p.id}>
                  <td>{brData(p.data)}</td>
                  <td>{p.cobertura || "—"}</td>
                  <td>{p.frequencia_troca || "—"} dias</td>
                  <td><a href={`/pacientes/${pacienteId}/prescricoes/${p.id}/imprimir`} target="_blank" rel="noreferrer" className="botao botao-secundario botao-pequeno">Ver / imprimir</a></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      ) : <Vazio icone="📋">Nenhuma prescrição registrada ainda.</Vazio>}

      {painelAberto && (
        <div style={{ marginTop: "1.5rem" }}>
          <hr className="divisor" />
          <h3>Nova prescrição de curativo</h3>
          <form onSubmit={salvar}>
            <div className="campos-linha">
              <div className="campo"><label>Data</label><input type="date" value={form.data} onChange={(e) => setForm({ ...form, data: e.target.value })} /></div>
              <div className="campo"><label>Aos cuidados de</label><input type="text" value={form.aos_cuidados_de} onChange={(e) => setForm({ ...form, aos_cuidados_de: e.target.value })} /></div>
            </div>

            <fieldset>
              <legend>Orientações</legend>
              <label className="checkbox-linha"><input type="checkbox" checked={form.limpeza_sf09} onChange={(e) => setForm({ ...form, limpeza_sf09: e.target.checked })} /> Utilizar SF 0,9% aquecido para umedecer o curativo anterior; limpeza da lesão com SF 0,9% em jato</label>
              <label className="checkbox-linha"><input type="checkbox" checked={form.phmb} onChange={(e) => setForm({ ...form, phmb: e.target.checked })} /> Aplicar PHMB solução embebido em gaze e deixar agir por 10 minutos (não enxaguar)</label>
              <label className="checkbox-linha"><input type="checkbox" checked={form.creme_barreira} onChange={(e) => setForm({ ...form, creme_barreira: e.target.checked })} /> Aplicar fina camada de creme de barreira em perilesional</label>
            </fieldset>

            <div className="campo"><label>Colocar a cobertura</label><input type="text" placeholder="Nome da cobertura" value={form.cobertura} onChange={(e) => setForm({ ...form, cobertura: e.target.value })} /></div>

            <fieldset>
              <legend>Cobrir com</legend>
              {COBRIR_COM_OPCOES.map((op) => (
                <label className="checkbox-linha" key={op}><input type="checkbox" checked={form.cobrir_com.includes(op)} onChange={() => toggleCobrirCom(op)} /> {op}</label>
              ))}
            </fieldset>

            <div className="campo"><label>Troca do curativo a cada (dias)</label><input type="text" style={{ maxWidth: 120 }} value={form.frequencia_troca} onChange={(e) => setForm({ ...form, frequencia_troca: e.target.value })} /></div>
            <div className="campo"><label>Observações adicionais</label><textarea value={form.observacoes} onChange={(e) => setForm({ ...form, observacoes: e.target.value })}></textarea></div>

            <div className="caixa-info" style={{ marginBottom: "1rem" }}>
              Atenção: não utilizar fitas na pele, sabão, clorexidina, PVPI ou qualquer produto não mencionado — contraindicados em feridas. Orientar aumento de proteínas e vitaminas A, C e K, salvo restrição de dieta.
            </div>

            <div className="acoes-form">
              <button type="submit" className="botao botao-primario botao-grande">Salvar prescrição</button>
              <button type="button" className="botao botao-fantasma" onClick={() => setPainelAberto(false)}>Cancelar</button>
            </div>
          </form>
        </div>
      )}
    </div>
  );
}

import { useState } from "react";
import { Link } from "react-router-dom";
import { api } from "../../api";
import { useFlash } from "../../context/FlashContext";
import { Vazio } from "../../components/UI";
import { brData } from "../../utils/format";
import diagnosticosData from "../../utils/diagnosticosData";

const {
  TIME_TECIDO_OPCOES, TIME_INFECCAO_SUPERFICIAL, TIME_INFECCAO_PROFUNDA,
  EXSUDATO_TIPOS, EXSUDATO_QUANTIDADE, BORDAS_OPCOES, PERILESIONAL_OPCOES, BIOFILME_SINAIS,
} = diagnosticosData;

const VAZIO_FORM = {
  numero_lesao: "", etiologia: "", tempo_ferida: "", localizacao: "",
  comprimento: "", largura: "", profundidade: "", descolamento: "",
  time_tecido: [], time_infeccao: [], exsudato_tipo: "", exsudato_quantidade: "",
  bordas: [], perilesional: [], biofilme: "nao", biofilme_sinais: [],
  itb_braco_esq: "", itb_braco_dir: "", itb_tornozelo_dir_tp: "", itb_tornozelo_dir_pd: "",
  itb_tornozelo_esq_tp: "", itb_tornozelo_esq_pd: "", itb_resultado_dir: "", itb_resultado_esq: "",
  sensibilidade: "", sensibilidade_obs: "",
};

export default function AbaFeridas({ ficha, recarregar, pacienteId }) {
  const [painelAberto, setPainelAberto] = useState(false);
  const [form, setForm] = useState(VAZIO_FORM);
  const [foto, setFoto] = useState(null);
  const { setFlash } = useFlash();

  function toggleLista(campo, valor) {
    setForm((f) => {
      const atual = f[campo] || [];
      return { ...f, [campo]: atual.includes(valor) ? atual.filter((v) => v !== valor) : [...atual, valor] };
    });
  }

  async function salvar(e) {
    e.preventDefault();
    const formData = new FormData();
    Object.entries(form).forEach(([k, v]) => {
      if (Array.isArray(v)) v.forEach((item) => formData.append(k, item));
      else formData.append(k, v || "");
    });
    if (foto) formData.append("foto", foto);
    await api.postForm(`/pacientes/${pacienteId}/feridas`, formData);
    setFlash("Avaliação de ferida registrada.");
    setForm(VAZIO_FORM);
    setFoto(null);
    setPainelAberto(false);
    recarregar();
  }

  return (
    <div className="cartao">
      <div className="cartao-titulo">
        <h2>Feridas / lesões</h2>
        <button type="button" className="botao botao-primario" onClick={() => setPainelAberto((v) => !v)}>➕ Nova avaliação de ferida</button>
      </div>

      {ficha.feridas.length ? ficha.feridas.map((f) => (
        <Link key={f.id} to={`/pacientes/${pacienteId}/feridas/${f.id}`} className="slot-agendamento" style={{ textDecoration: "none", color: "inherit" }}>
          {f.foto ? <img src={`/uploads/feridas/${f.foto}`} alt="" style={{ width: 56, height: 56, borderRadius: 8, objectFit: "cover" }} /> : <span className="avatar-espaco" style={{ borderRadius: 8 }}>🩹</span>}
          <div className="slot-info">
            <strong>{f.numero_lesao ? `Lesão ${f.numero_lesao}` : `Lesão em ${f.localizacao || "local não informado"}`}</strong><br />
            <span className="texto-suave">{f.etiologia || "Etiologia não informada"} · registrada em {brData(f.criado_em)}</span>
          </div>
          <span className={`selo ${f.ativa ? "selo-confirmado" : "selo-cancelado"}`}>{f.ativa ? "Em acompanhamento" : "Encerrada"}</span>
        </Link>
      )) : <Vazio icone="🩹">Nenhuma ferida registrada ainda.</Vazio>}

      {painelAberto && (
        <div style={{ marginTop: "1.5rem" }}>
          <hr className="divisor" />
          <h3>Nova avaliação de ferida</h3>
          <form onSubmit={salvar} encType="multipart/form-data">
            <div className="campo">
              <label>Foto da ferida</label>
              <input type="file" accept="image/*" onChange={(e) => setFoto(e.target.files[0] || null)} />
            </div>

            <div className="campos-linha">
              <div className="campo"><label>Lesão nº</label><input type="text" value={form.numero_lesao} onChange={(e) => setForm({ ...form, numero_lesao: e.target.value })} /></div>
              <div className="campo"><label>Etiologia</label><input type="text" value={form.etiologia} onChange={(e) => setForm({ ...form, etiologia: e.target.value })} /></div>
              <div className="campo"><label>Tempo de ferida</label><input type="text" value={form.tempo_ferida} onChange={(e) => setForm({ ...form, tempo_ferida: e.target.value })} /></div>
            </div>
            <div className="campo"><label>Localização (nome do local)</label><input type="text" placeholder="Ex: Maléolo lateral, perna direita" value={form.localizacao} onChange={(e) => setForm({ ...form, localizacao: e.target.value })} /></div>

            <fieldset>
              <legend>Dimensões (cm)</legend>
              <div className="campos-linha">
                <div className="campo"><label>Comprimento</label><input type="text" value={form.comprimento} onChange={(e) => setForm({ ...form, comprimento: e.target.value })} /></div>
                <div className="campo"><label>Largura</label><input type="text" value={form.largura} onChange={(e) => setForm({ ...form, largura: e.target.value })} /></div>
                <div className="campo"><label>Profundidade</label><input type="text" value={form.profundidade} onChange={(e) => setForm({ ...form, profundidade: e.target.value })} /></div>
                <div className="campo"><label>Descolamento</label><input type="text" value={form.descolamento} onChange={(e) => setForm({ ...form, descolamento: e.target.value })} /></div>
              </div>
            </fieldset>

            <fieldset>
              <legend>T — Tipo de tecido no leito</legend>
              <div className="checkbox-grade">
                {TIME_TECIDO_OPCOES.map((op) => (
                  <label className="checkbox-linha" key={op}><input type="checkbox" checked={form.time_tecido.includes(op)} onChange={() => toggleLista("time_tecido", op)} /> {op}</label>
                ))}
              </div>
            </fieldset>

            <fieldset>
              <legend>I — Sinais de infecção</legend>
              <div className="campos-linha">
                <div>
                  <strong>Infecção superficial</strong>
                  {TIME_INFECCAO_SUPERFICIAL.map((op) => (
                    <label className="checkbox-linha" key={op}><input type="checkbox" checked={form.time_infeccao.includes(op)} onChange={() => toggleLista("time_infeccao", op)} /> {op}</label>
                  ))}
                </div>
                <div>
                  <strong>Infecção profunda</strong>
                  {TIME_INFECCAO_PROFUNDA.map((op) => (
                    <label className="checkbox-linha" key={op}><input type="checkbox" checked={form.time_infeccao.includes(op)} onChange={() => toggleLista("time_infeccao", op)} /> {op}</label>
                  ))}
                </div>
              </div>
            </fieldset>

            <fieldset>
              <legend>M — Exsudato</legend>
              <div className="campos-linha">
                <div className="campo">
                  <label>Tipo</label>
                  <select value={form.exsudato_tipo} onChange={(e) => setForm({ ...form, exsudato_tipo: e.target.value })}>
                    <option value="">Selecione</option>
                    {EXSUDATO_TIPOS.map(([tipo, cor, consist]) => <option key={tipo} value={tipo}>{tipo} ({cor}, {consist})</option>)}
                  </select>
                </div>
                <div className="campo">
                  <label>Quantidade</label>
                  <select value={form.exsudato_quantidade} onChange={(e) => setForm({ ...form, exsudato_quantidade: e.target.value })}>
                    <option value="">Selecione</option>
                    {EXSUDATO_QUANTIDADE.map((op) => <option key={op} value={op}>{op}</option>)}
                  </select>
                </div>
              </div>
            </fieldset>

            <fieldset>
              <legend>E — Bordas e região perilesional</legend>
              <div className="campos-linha">
                <div>
                  <strong>Bordas da ferida</strong>
                  {BORDAS_OPCOES.map((op) => (
                    <label className="checkbox-linha" key={op}><input type="checkbox" checked={form.bordas.includes(op)} onChange={() => toggleLista("bordas", op)} /> {op}</label>
                  ))}
                </div>
                <div>
                  <strong>Perilesional (10 a 20 cm)</strong>
                  {PERILESIONAL_OPCOES.map((op) => (
                    <label className="checkbox-linha" key={op}><input type="checkbox" checked={form.perilesional.includes(op)} onChange={() => toggleLista("perilesional", op)} /> {op}</label>
                  ))}
                </div>
              </div>
            </fieldset>

            <fieldset>
              <legend>Identificação do biofilme</legend>
              <div className="radio-grupo" style={{ marginBottom: ".6rem" }}>
                <label className="radio-linha"><input type="radio" name="biofilme" checked={form.biofilme === "nao"} onChange={() => setForm({ ...form, biofilme: "nao" })} /> Não</label>
                <label className="radio-linha"><input type="radio" name="biofilme" checked={form.biofilme === "sim"} onChange={() => setForm({ ...form, biofilme: "sim" })} /> Sim</label>
              </div>
              <div className="checkbox-grade">
                {BIOFILME_SINAIS.map((op) => (
                  <label className="checkbox-linha" key={op}><input type="checkbox" checked={form.biofilme_sinais.includes(op)} onChange={() => toggleLista("biofilme_sinais", op)} /> {op}</label>
                ))}
              </div>
            </fieldset>

            <fieldset>
              <legend>Índice tornozelo-braquial (ITB)</legend>
              <div className="campos-linha">
                <div className="campo"><label>Pressão sistólica — Braço esquerdo (mmHg)</label><input type="text" value={form.itb_braco_esq} onChange={(e) => setForm({ ...form, itb_braco_esq: e.target.value })} /></div>
                <div className="campo"><label>Pressão sistólica — Braço direito (mmHg)</label><input type="text" value={form.itb_braco_dir} onChange={(e) => setForm({ ...form, itb_braco_dir: e.target.value })} /></div>
              </div>
              <div className="campos-linha">
                <div className="campo"><label>Tornozelo direito — Tibial posterior</label><input type="text" value={form.itb_tornozelo_dir_tp} onChange={(e) => setForm({ ...form, itb_tornozelo_dir_tp: e.target.value })} /></div>
                <div className="campo"><label>Tornozelo direito — Pediosa dorsal</label><input type="text" value={form.itb_tornozelo_dir_pd} onChange={(e) => setForm({ ...form, itb_tornozelo_dir_pd: e.target.value })} /></div>
                <div className="campo"><label>Tornozelo esquerdo — Tibial posterior</label><input type="text" value={form.itb_tornozelo_esq_tp} onChange={(e) => setForm({ ...form, itb_tornozelo_esq_tp: e.target.value })} /></div>
                <div className="campo"><label>Tornozelo esquerdo — Pediosa dorsal</label><input type="text" value={form.itb_tornozelo_esq_pd} onChange={(e) => setForm({ ...form, itb_tornozelo_esq_pd: e.target.value })} /></div>
              </div>
              <div className="campos-linha">
                <div className="campo"><label>ITB direito (resultado calculado)</label><input type="text" placeholder="ex: 0.95" value={form.itb_resultado_dir} onChange={(e) => setForm({ ...form, itb_resultado_dir: e.target.value })} /></div>
                <div className="campo"><label>ITB esquerdo (resultado calculado)</label><input type="text" placeholder="ex: 0.95" value={form.itb_resultado_esq} onChange={(e) => setForm({ ...form, itb_resultado_esq: e.target.value })} /></div>
              </div>
              <p className="campo-ajuda">Referência: &gt;1,4 falsamente elevado · 1,0–1,4 normal · 0,91–0,99 limítrofe · 0,8–0,90 leve · 0,51–0,79 moderado · ≤0,5 severo.</p>
            </fieldset>

            <fieldset>
              <legend>Teste de sensibilidade (monofilamento)</legend>
              <div className="radio-grupo" style={{ marginBottom: ".5rem" }}>
                <label className="radio-linha"><input type="radio" name="sensibilidade" checked={form.sensibilidade === "Normal"} onChange={() => setForm({ ...form, sensibilidade: "Normal" })} /> Normal</label>
                <label className="radio-linha"><input type="radio" name="sensibilidade" checked={form.sensibilidade === "Alterado"} onChange={() => setForm({ ...form, sensibilidade: "Alterado" })} /> Alterado</label>
              </div>
              <input type="text" placeholder="Pontos alterados / observações" value={form.sensibilidade_obs} onChange={(e) => setForm({ ...form, sensibilidade_obs: e.target.value })} />
            </fieldset>

            <div className="acoes-form">
              <button type="submit" className="botao botao-primario botao-grande">Salvar avaliação de ferida</button>
              <button type="button" className="botao botao-fantasma" onClick={() => setPainelAberto(false)}>Cancelar</button>
            </div>
          </form>
        </div>
      )}
    </div>
  );
}

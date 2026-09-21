import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "../../api";
import { useFlash } from "../../context/FlashContext";
import { brData } from "../../utils/format";
import diagnosticosData from "../../utils/diagnosticosData";

const { DOENCA_BASE_OPCOES, HABITOS_OPCOES, SONO_OPCOES } = diagnosticosData;

export default function AbaDados({ ficha, recarregar, pacienteId }) {
  const { paciente, avaliacaoSaude } = ficha;
  const [editando, setEditando] = useState(false);
  const [valores, setValores] = useState({ ...paciente });
  const navigate = useNavigate();
  const { setFlash } = useFlash();

  const [saude, setSaude] = useState(() => ({
    doenca_base: avaliacaoSaude?.doenca_base || "",
    doenca_base_quais: avaliacaoSaude?.doenca_base_quais || [],
    medicacoes: avaliacaoSaude?.medicacoes || "",
    medicacoes_quais: avaliacaoSaude?.medicacoes_quais || "",
    alergias: avaliacaoSaude?.alergias || "",
    alergias_quais: avaliacaoSaude?.alergias_quais || "",
    cirurgias: avaliacaoSaude?.cirurgias || "",
    cirurgias_quais: avaliacaoSaude?.cirurgias_quais || "",
    mobilidade: avaliacaoSaude?.mobilidade || "",
    mobilidade_obs: avaliacaoSaude?.mobilidade_obs || "",
    higiene: avaliacaoSaude?.higiene || "",
    higiene_obs: avaliacaoSaude?.higiene_obs || "",
    cuidado_ferida: avaliacaoSaude?.cuidado_ferida || "",
    cuidado_ferida_quem: avaliacaoSaude?.cuidado_ferida_quem || "",
    alimentacao: avaliacaoSaude?.alimentacao || "",
    atividade_fisica: avaliacaoSaude?.atividade_fisica || "",
    habitos: avaliacaoSaude?.habitos || [],
    sono: avaliacaoSaude?.sono || [],
    sono_medicacao: avaliacaoSaude?.sono_medicacao || "",
  }));

  function toggleLista(campo, valor) {
    setSaude((s) => {
      const atual = s[campo] || [];
      const novo = atual.includes(valor) ? atual.filter((v) => v !== valor) : [...atual, valor];
      return { ...s, [campo]: novo };
    });
  }

  async function salvarDados(e) {
    e.preventDefault();
    await api.put(`/pacientes/${pacienteId}`, valores);
    setFlash("Dados do paciente atualizados.");
    setEditando(false);
    recarregar();
  }

  async function salvarSaude(e) {
    e.preventDefault();
    await api.put(`/pacientes/${pacienteId}/saude`, saude);
    setFlash("Avaliação de saúde salva.");
    recarregar();
  }

  async function remover() {
    if (!window.confirm("Tem certeza que deseja remover este paciente? Os dados não serão apagados, mas ele sairá das listas.")) return;
    await api.delete(`/pacientes/${pacienteId}`);
    navigate("/pacientes");
  }

  return (
    <>
      <div className="cartao">
        <div className="cartao-titulo"><h2>Dados cadastrais</h2></div>
        <div className="grade grade-3" style={{ marginBottom: "1rem" }}>
          <div><strong>Data de nascimento</strong><br />{brData(paciente.data_nascimento)}</div>
          <div><strong>Sexo</strong><br />{paciente.sexo || "—"}</div>
          <div><strong>Estado civil</strong><br />{paciente.estado_civil || "—"}</div>
          <div><strong>Ocupação</strong><br />{paciente.ocupacao || "—"}</div>
          <div><strong>Nº de filhos</strong><br />{paciente.numero_filhos || "—"}</div>
          <div><strong>Religião</strong><br />{paciente.religiao || "—"}</div>
          <div><strong>Telefone</strong><br />{paciente.telefone || "—"}</div>
          <div><strong>Endereço</strong><br />{paciente.endereco || "—"}{paciente.numero ? `, ${paciente.numero}` : ""}</div>
          <div><strong>Bairro</strong><br />{paciente.bairro || "—"}</div>
          <div><strong>Escolaridade</strong><br />{paciente.escolaridade || "—"}</div>
          <div><strong>UBS de referência</strong><br />{paciente.ubs_referencia || "—"}</div>
          <div><strong>Convênio/Particular</strong><br />{paciente.convenio_particular || "—"}</div>
        </div>

        {!editando ? (
          <button type="button" className="botao botao-secundario botao-pequeno" onClick={() => { setValores({ ...paciente }); setEditando(true); }}>✏️ Editar dados cadastrais</button>
        ) : (
          <form onSubmit={salvarDados} style={{ marginTop: "1.25rem" }}>
            <div className="campo"><label>Nome completo *</label><input type="text" required value={valores.nome || ""} onChange={(e) => setValores({ ...valores, nome: e.target.value })} /></div>
            <div className="campos-linha">
              <div className="campo"><label>Data de nascimento</label><input type="date" value={valores.data_nascimento || ""} onChange={(e) => setValores({ ...valores, data_nascimento: e.target.value })} /></div>
              <div className="campo"><label>Sexo</label>
                <select value={valores.sexo || ""} onChange={(e) => setValores({ ...valores, sexo: e.target.value })}>
                  <option value="">Selecione</option><option value="Masculino">Masculino</option><option value="Feminino">Feminino</option>
                </select>
              </div>
              <div className="campo"><label>Estado civil</label><input type="text" value={valores.estado_civil || ""} onChange={(e) => setValores({ ...valores, estado_civil: e.target.value })} /></div>
            </div>
            <div className="campos-linha">
              <div className="campo"><label>Ocupação</label><input type="text" value={valores.ocupacao || ""} onChange={(e) => setValores({ ...valores, ocupacao: e.target.value })} /></div>
              <div className="campo"><label>Nº de filhos</label><input type="text" value={valores.numero_filhos || ""} onChange={(e) => setValores({ ...valores, numero_filhos: e.target.value })} /></div>
              <div className="campo"><label>Religião</label><input type="text" value={valores.religiao || ""} onChange={(e) => setValores({ ...valores, religiao: e.target.value })} /></div>
            </div>
            <div className="campo"><label>Telefone</label><input type="tel" value={valores.telefone || ""} onChange={(e) => setValores({ ...valores, telefone: e.target.value })} /></div>
            <div className="campos-linha">
              <div className="campo" style={{ gridColumn: "span 2" }}><label>Endereço</label><input type="text" value={valores.endereco || ""} onChange={(e) => setValores({ ...valores, endereco: e.target.value })} /></div>
              <div className="campo"><label>Número</label><input type="text" value={valores.numero || ""} onChange={(e) => setValores({ ...valores, numero: e.target.value })} /></div>
            </div>
            <div className="campo"><label>Bairro</label><input type="text" value={valores.bairro || ""} onChange={(e) => setValores({ ...valores, bairro: e.target.value })} /></div>
            <div className="campos-linha">
              <div className="campo"><label>Escolaridade</label><input type="text" value={valores.escolaridade || ""} onChange={(e) => setValores({ ...valores, escolaridade: e.target.value })} /></div>
              <div className="campo"><label>UBS de referência</label><input type="text" value={valores.ubs_referencia || ""} onChange={(e) => setValores({ ...valores, ubs_referencia: e.target.value })} /></div>
            </div>
            <div className="campos-linha">
              <div className="campo"><label>Convênio/Particular</label><input type="text" value={valores.convenio_particular || ""} onChange={(e) => setValores({ ...valores, convenio_particular: e.target.value })} /></div>
              <div className="campo"><label>Passa na Policlínica</label><input type="text" value={valores.passa_policlinica || ""} onChange={(e) => setValores({ ...valores, passa_policlinica: e.target.value })} /></div>
            </div>
            <div className="acoes-form">
              <button type="submit" className="botao botao-primario">Salvar alterações</button>
              <button type="button" className="botao botao-fantasma" onClick={() => setEditando(false)}>Cancelar</button>
            </div>
          </form>
        )}

        <hr className="divisor" />
        <button type="button" className="botao botao-perigo botao-pequeno" onClick={remover}>Remover paciente</button>
      </div>

      <div className="cartao">
        <h2>Avaliação integrativa da saúde física</h2>
        <form onSubmit={salvarSaude}>
          <fieldset>
            <legend>Doença de base</legend>
            <div className="radio-grupo" style={{ marginBottom: ".75rem" }}>
              <label className="radio-linha"><input type="radio" name="doenca_base" checked={saude.doenca_base === "nao"} onChange={() => setSaude({ ...saude, doenca_base: "nao" })} /> Não</label>
              <label className="radio-linha"><input type="radio" name="doenca_base" checked={saude.doenca_base === "sim"} onChange={() => setSaude({ ...saude, doenca_base: "sim" })} /> Sim</label>
            </div>
            <div className="checkbox-grade">
              {DOENCA_BASE_OPCOES.map((op) => (
                <label className="checkbox-linha" key={op}>
                  <input type="checkbox" checked={saude.doenca_base_quais.includes(op)} onChange={() => toggleLista("doenca_base_quais", op)} /> {op}
                </label>
              ))}
            </div>
          </fieldset>

          <div className="campos-linha">
            <fieldset>
              <legend>Medicações em uso</legend>
              <div className="radio-grupo" style={{ marginBottom: ".5rem" }}>
                <label className="radio-linha"><input type="radio" name="medicacoes" checked={saude.medicacoes === "nao"} onChange={() => setSaude({ ...saude, medicacoes: "nao" })} /> Não</label>
                <label className="radio-linha"><input type="radio" name="medicacoes" checked={saude.medicacoes === "sim"} onChange={() => setSaude({ ...saude, medicacoes: "sim" })} /> Sim</label>
              </div>
              <input type="text" placeholder="Quais?" value={saude.medicacoes_quais} onChange={(e) => setSaude({ ...saude, medicacoes_quais: e.target.value })} />
            </fieldset>
            <fieldset>
              <legend>Histórico de alergias</legend>
              <div className="radio-grupo" style={{ marginBottom: ".5rem" }}>
                <label className="radio-linha"><input type="radio" name="alergias" checked={saude.alergias === "nao"} onChange={() => setSaude({ ...saude, alergias: "nao" })} /> Não</label>
                <label className="radio-linha"><input type="radio" name="alergias" checked={saude.alergias === "sim"} onChange={() => setSaude({ ...saude, alergias: "sim" })} /> Sim</label>
              </div>
              <input type="text" placeholder="Quais?" value={saude.alergias_quais} onChange={(e) => setSaude({ ...saude, alergias_quais: e.target.value })} />
            </fieldset>
            <fieldset>
              <legend>Histórico de cirurgias</legend>
              <div className="radio-grupo" style={{ marginBottom: ".5rem" }}>
                <label className="radio-linha"><input type="radio" name="cirurgias" checked={saude.cirurgias === "nao"} onChange={() => setSaude({ ...saude, cirurgias: "nao" })} /> Não</label>
                <label className="radio-linha"><input type="radio" name="cirurgias" checked={saude.cirurgias === "sim"} onChange={() => setSaude({ ...saude, cirurgias: "sim" })} /> Sim</label>
              </div>
              <input type="text" placeholder="Quais / último ano" value={saude.cirurgias_quais} onChange={(e) => setSaude({ ...saude, cirurgias_quais: e.target.value })} />
            </fieldset>
          </div>

          <div className="campos-linha">
            <fieldset>
              <legend>Mobilidade</legend>
              <div className="radio-grupo" style={{ marginBottom: ".5rem" }}>
                <label className="radio-linha"><input type="radio" name="mobilidade" checked={saude.mobilidade === "Independente"} onChange={() => setSaude({ ...saude, mobilidade: "Independente" })} /> Independente</label>
                <label className="radio-linha"><input type="radio" name="mobilidade" checked={saude.mobilidade === "Limitada"} onChange={() => setSaude({ ...saude, mobilidade: "Limitada" })} /> Limitada</label>
              </div>
              <input type="text" placeholder="Observações" value={saude.mobilidade_obs} onChange={(e) => setSaude({ ...saude, mobilidade_obs: e.target.value })} />
            </fieldset>
            <fieldset>
              <legend>Condições de higiene pessoal</legend>
              <div className="radio-grupo" style={{ marginBottom: ".5rem" }}>
                <label className="radio-linha"><input type="radio" name="higiene" checked={saude.higiene === "Adequada"} onChange={() => setSaude({ ...saude, higiene: "Adequada" })} /> Adequada</label>
                <label className="radio-linha"><input type="radio" name="higiene" checked={saude.higiene === "Inadequada"} onChange={() => setSaude({ ...saude, higiene: "Inadequada" })} /> Inadequada</label>
              </div>
              <input type="text" placeholder="Observações" value={saude.higiene_obs} onChange={(e) => setSaude({ ...saude, higiene_obs: e.target.value })} />
            </fieldset>
            <fieldset>
              <legend>Cuidados com a ferida</legend>
              <div className="radio-grupo" style={{ marginBottom: ".5rem" }}>
                <label className="radio-linha"><input type="radio" name="cuidado_ferida" checked={saude.cuidado_ferida === "Adequado"} onChange={() => setSaude({ ...saude, cuidado_ferida: "Adequado" })} /> Adequado</label>
                <label className="radio-linha"><input type="radio" name="cuidado_ferida" checked={saude.cuidado_ferida === "Inadequado"} onChange={() => setSaude({ ...saude, cuidado_ferida: "Inadequado" })} /> Inadequado</label>
              </div>
              <select value={saude.cuidado_ferida_quem} onChange={(e) => setSaude({ ...saude, cuidado_ferida_quem: e.target.value })}>
                <option value="">Quem realiza o cuidado?</option>
                {["Autocuidado", "Familiar", "Profissional", "Cuidador"].map((op) => <option key={op} value={op}>{op}</option>)}
              </select>
            </fieldset>
          </div>

          <fieldset>
            <legend>Estilo de vida</legend>
            <div className="campos-linha">
              <div>
                <label>Alimentação</label>
                <div className="radio-grupo">
                  {["Equilibrada", "Má alimentação", "Estado nutricional inadequado"].map((op) => (
                    <label className="radio-linha" key={op}><input type="radio" name="alimentacao" checked={saude.alimentacao === op} onChange={() => setSaude({ ...saude, alimentacao: op })} /> {op}</label>
                  ))}
                </div>
              </div>
              <div>
                <label>Atividade física</label>
                <div className="radio-grupo">
                  {["Ativo", "Regular", "Não pratica"].map((op) => (
                    <label className="radio-linha" key={op}><input type="radio" name="atividade_fisica" checked={saude.atividade_fisica === op} onChange={() => setSaude({ ...saude, atividade_fisica: op })} /> {op}</label>
                  ))}
                </div>
              </div>
            </div>
            <div className="campos-linha" style={{ marginTop: "1rem" }}>
              <div>
                <label>Hábitos</label>
                {HABITOS_OPCOES.map((op) => (
                  <label className="checkbox-linha" key={op}><input type="checkbox" checked={saude.habitos.includes(op)} onChange={() => toggleLista("habitos", op)} /> {op}</label>
                ))}
              </div>
              <div>
                <label>Sono</label>
                {SONO_OPCOES.map((op) => (
                  <label className="checkbox-linha" key={op}><input type="checkbox" checked={saude.sono.includes(op)} onChange={() => toggleLista("sono", op)} /> {op}</label>
                ))}
                <input type="text" placeholder="Faz uso de medicação para dormir?" value={saude.sono_medicacao} onChange={(e) => setSaude({ ...saude, sono_medicacao: e.target.value })} style={{ marginTop: ".5rem" }} />
              </div>
            </div>
          </fieldset>

          <div className="acoes-form">
            <button type="submit" className="botao botao-primario botao-grande">Salvar avaliação de saúde</button>
          </div>
        </form>
      </div>
    </>
  );
}

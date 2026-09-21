import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "../api";
import { useFlash } from "../context/FlashContext";

const CAMPOS_INICIAIS = {
  nome: "", data_nascimento: "", sexo: "", estado_civil: "", ocupacao: "",
  numero_filhos: "", religiao: "", telefone: "", endereco: "", numero: "",
  bairro: "", escolaridade: "", ubs_referencia: "", convenio_particular: "", passa_policlinica: "",
};

export default function PacienteNovo() {
  const [valores, setValores] = useState(CAMPOS_INICIAIS);
  const [foto, setFoto] = useState(null);
  const [preview, setPreview] = useState(null);
  const [erro, setErro] = useState(null);
  const [enviando, setEnviando] = useState(false);
  const navigate = useNavigate();
  const { setFlash } = useFlash();

  function onChange(campo, valor) {
    setValores((v) => ({ ...v, [campo]: valor }));
  }

  function onFoto(e) {
    const arq = e.target.files[0];
    setFoto(arq || null);
    if (arq) {
      const leitor = new FileReader();
      leitor.onload = (ev) => setPreview(ev.target.result);
      leitor.readAsDataURL(arq);
    } else {
      setPreview(null);
    }
  }

  async function enviar(e) {
    e.preventDefault();
    if (!valores.nome.trim()) { setErro("O nome do paciente é obrigatório."); return; }
    setErro(null);
    setEnviando(true);
    try {
      const formData = new FormData();
      Object.entries(valores).forEach(([k, v]) => formData.append(k, v || ""));
      if (foto) formData.append("foto", foto);
      const paciente = await api.postForm("/pacientes", formData);
      setFlash("Paciente cadastrado com sucesso.");
      navigate(`/pacientes/${paciente.id}`);
    } catch (err) {
      setErro(err.message);
      setEnviando(false);
    }
  }

  return (
    <div>
      <button className="voltar-link" onClick={() => navigate("/pacientes")} type="button">← Voltar para pacientes</button>
      <div className="pagina-cabecalho"><h1>Novo paciente</h1></div>

      {erro && <div className="flash flash-erro" style={{ marginBottom: "1rem" }}>{erro}</div>}

      <form onSubmit={enviar} className="cartao">
        <fieldset>
          <legend>Foto do paciente</legend>
          <div className="campo">
            {preview && <img src={preview} className="foto-preview" alt="Pré-visualização" />}
            <input type="file" accept="image/*" onChange={onFoto} />
            <p className="campo-ajuda">Opcional. Formatos aceitos: JPG, PNG ou WEBP.</p>
          </div>
        </fieldset>

        <fieldset>
          <legend>Dados pessoais</legend>
          <div className="campo">
            <label htmlFor="nome">Nome completo *</label>
            <input type="text" id="nome" required value={valores.nome} onChange={(e) => onChange("nome", e.target.value)} />
          </div>
          <div className="campos-linha">
            <div className="campo">
              <label htmlFor="data_nascimento">Data de nascimento</label>
              <input type="date" id="data_nascimento" value={valores.data_nascimento} onChange={(e) => onChange("data_nascimento", e.target.value)} />
            </div>
            <div className="campo">
              <label htmlFor="sexo">Sexo</label>
              <select id="sexo" value={valores.sexo} onChange={(e) => onChange("sexo", e.target.value)}>
                <option value="">Selecione</option>
                <option value="Masculino">Masculino</option>
                <option value="Feminino">Feminino</option>
              </select>
            </div>
            <div className="campo">
              <label htmlFor="estado_civil">Estado civil</label>
              <input type="text" id="estado_civil" value={valores.estado_civil} onChange={(e) => onChange("estado_civil", e.target.value)} />
            </div>
          </div>
          <div className="campos-linha">
            <div className="campo"><label htmlFor="ocupacao">Ocupação</label><input type="text" id="ocupacao" value={valores.ocupacao} onChange={(e) => onChange("ocupacao", e.target.value)} /></div>
            <div className="campo"><label htmlFor="numero_filhos">Número de filhos</label><input type="text" id="numero_filhos" value={valores.numero_filhos} onChange={(e) => onChange("numero_filhos", e.target.value)} /></div>
            <div className="campo"><label htmlFor="religiao">Religião</label><input type="text" id="religiao" value={valores.religiao} onChange={(e) => onChange("religiao", e.target.value)} /></div>
          </div>
          <div className="campo"><label htmlFor="telefone">Telefone</label><input type="tel" id="telefone" value={valores.telefone} onChange={(e) => onChange("telefone", e.target.value)} /></div>
        </fieldset>

        <fieldset>
          <legend>Endereço</legend>
          <div className="campos-linha">
            <div className="campo" style={{ gridColumn: "span 2" }}><label htmlFor="endereco">Endereço</label><input type="text" id="endereco" value={valores.endereco} onChange={(e) => onChange("endereco", e.target.value)} /></div>
            <div className="campo"><label htmlFor="numero">Número</label><input type="text" id="numero" value={valores.numero} onChange={(e) => onChange("numero", e.target.value)} /></div>
          </div>
          <div className="campo"><label htmlFor="bairro">Bairro</label><input type="text" id="bairro" value={valores.bairro} onChange={(e) => onChange("bairro", e.target.value)} /></div>
        </fieldset>

        <fieldset>
          <legend>Informações complementares</legend>
          <div className="campos-linha">
            <div className="campo"><label htmlFor="escolaridade">Escolaridade</label><input type="text" id="escolaridade" value={valores.escolaridade} onChange={(e) => onChange("escolaridade", e.target.value)} /></div>
            <div className="campo"><label htmlFor="ubs_referencia">UBS de referência</label><input type="text" id="ubs_referencia" value={valores.ubs_referencia} onChange={(e) => onChange("ubs_referencia", e.target.value)} /></div>
          </div>
          <div className="campos-linha">
            <div className="campo"><label htmlFor="convenio_particular">Convênio/Particular</label><input type="text" id="convenio_particular" value={valores.convenio_particular} onChange={(e) => onChange("convenio_particular", e.target.value)} /></div>
            <div className="campo"><label htmlFor="passa_policlinica">Passa na Policlínica</label><input type="text" id="passa_policlinica" value={valores.passa_policlinica} onChange={(e) => onChange("passa_policlinica", e.target.value)} /></div>
          </div>
        </fieldset>

        <div className="acoes-form">
          <button type="submit" className="botao botao-primario botao-grande" disabled={enviando}>{enviando ? "Salvando..." : "Salvar paciente"}</button>
          <button type="button" className="botao botao-fantasma" onClick={() => navigate("/pacientes")}>Cancelar</button>
        </div>
      </form>
    </div>
  );
}

import { Link } from "react-router-dom";
import { api } from "../../api";
import { Toggle } from "../../components/UI";
import diagnosticosData from "../../utils/diagnosticosData";

const { DIAGNOSTICOS } = diagnosticosData;

export default function AbaDiagnosticos({ ficha, recarregar, pacienteId }) {
  async function alternar(chave, ligar) {
    await api.post(`/pacientes/${pacienteId}/diagnosticos/toggle`, { chave, ligar });
    recarregar();
  }

  return (
    <div className="cartao">
      <h2>Diagnósticos de enfermagem</h2>
      <p className="texto-suave">Ative apenas os diagnósticos que se aplicam a este paciente. Ao ativar, o formulário completo (NOC/NIC) fica disponível para preenchimento.</p>

      <div className="diag-lista">
        {Object.entries(DIAGNOSTICOS).map(([chave, d]) => {
          const ativo = ficha.diagnosticosAtivos.includes(chave);
          return (
            <div className={`diag-item ${ativo ? "diag-ativo" : ""}`} key={chave}>
              <div>
                <span className="diag-nome">{d.nome}</span><br />
                <span className="texto-suave" style={{ fontSize: ".88rem" }}>{d.definicao}</span>
              </div>
              <div style={{ display: "flex", alignItems: "center", gap: "1rem" }}>
                {ativo && <Link to={`/pacientes/${pacienteId}/diagnosticos/${chave}`} className="botao botao-secundario botao-pequeno">Preencher</Link>}
                <Toggle checked={ativo} onChange={(v) => alternar(chave, v)} />
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}

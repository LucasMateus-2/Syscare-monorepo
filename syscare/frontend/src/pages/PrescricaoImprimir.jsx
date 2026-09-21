import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { api } from "../api";
import { brData } from "../utils/format";

export default function PrescricaoImprimir() {
  const { id, prescricaoId } = useParams();
  const [dados, setDados] = useState(null);

  useEffect(() => {
    api.get(`/pacientes/${id}/prescricoes/${prescricaoId}`).then(setDados);
  }, [id, prescricaoId]);

  if (!dados) return <p style={{ padding: "2rem" }}>Carregando...</p>;
  const { paciente, prescricao } = dados;
  const cobrirCom = prescricao.cobrir_com || [];

  return (
    <div style={{ maxWidth: 700, margin: "2rem auto", padding: "0 1.5rem", fontFamily: "-apple-system, sans-serif" }}>
      <button className="botao botao-primario no-print" onClick={() => window.print()} style={{ marginBottom: "1.5rem" }}>🖨️ Imprimir</button>
      <h1 style={{ textAlign: "center" }}>Clínica de Enfermagem</h1>
      <hr className="divisor" />
      <p><strong>Aos cuidados de:</strong> {prescricao.aos_cuidados_de || "—"} &nbsp;&nbsp; <strong>Data:</strong> {brData(prescricao.data)}</p>
      <p><strong>Prescrição para curativo do(a) Sr(a):</strong> {paciente.nome}</p>

      <h3>Orientações</h3>
      <ul>
        <li>Efetuar o curativo usando E.P.I e luvas de procedimento.</li>
        {prescricao.limpeza_sf09 && <li>Utilizar SF 0,9% aquecido para umedecer o curativo anterior; limpeza da lesão com SF 0,9% em jato.</li>}
        {prescricao.phmb && <li>Aplicar PHMB solução embebido em gaze e deixar agir por 10 minutos. Não enxaguar.</li>}
        <li>Não secar o leito da lesão, apenas a volta da ferida.</li>
        {prescricao.creme_barreira && <li>Aplicar fina camada de creme de barreira (deixar transparente) em perilesional.</li>}
        {prescricao.cobertura && <li>Colocar a cobertura: {prescricao.cobertura}.</li>}
      </ul>

      {cobrirCom.length > 0 && (
        <>
          <h3>Cobrir com</h3>
          <ul>{cobrirCom.map((c) => <li key={c}>{c}</li>)}</ul>
        </>
      )}

      <div className="caixa-info">
        <strong>Atenção:</strong> não utilizar fitas na pele, sabão, clorexidina, PVPI ou qualquer produto não mencionado — são contraindicados em feridas.<br /><br />
        É necessário orientar o paciente a aumentar a ingesta de alimentos ricos em proteínas e vitaminas A, C e K: carnes, peixes, ovos, feijão, leite e derivados, limão, laranja, acerola, abacaxi, morango e verduras verdes escuras — desde que não haja restrição de dieta por outras doenças de base.
      </div>

      <p style={{ marginTop: "1.5rem" }}><strong>Obs:</strong> a troca do curativo deverá ser realizada a cada {prescricao.frequencia_troca || "____"} dias. Em lesões muito exsudativas, trocar a cobertura secundária (gaze ou chumaço) e terciária (atadura e esparadrapo) sempre que molharem.</p>
      {prescricao.observacoes && <p><strong>Observações adicionais:</strong> {prescricao.observacoes}</p>}

      <div style={{ marginTop: "3rem" }}>_____________________________<br />Assinatura e carimbo do enfermeiro</div>

      <style>{"@media print { .no-print { display: none; } }"}</style>
    </div>
  );
}

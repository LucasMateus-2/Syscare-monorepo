import { useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function Login() {
  const { carregando, hasAdmin, admin, login } = useAuth();
  const [usuario, setUsuario] = useState("");
  const [senha, setSenha] = useState("");
  const [erro, setErro] = useState(null);
  const navigate = useNavigate();

  if (carregando) return null;
  if (!hasAdmin) return <Navigate to="/configurar" replace />;
  if (admin) return <Navigate to="/" replace />;

  async function enviar(e) {
    e.preventDefault();
    setErro(null);
    try {
      await login(usuario, senha);
      navigate("/");
    } catch (err) {
      setErro(err.message);
    }
  }

  return (
    <div className="tela-centralizada">
      <div className="caixa-login">
        <div className="login-topo">
          <span className="marca-icone" aria-hidden="true">✚</span>
          <h1>Clínica de Enfermagem</h1>
          <p className="texto-suave">Acesso restrito ao administrador</p>
        </div>
        {erro && <div className="flash flash-erro" style={{ marginBottom: "1rem" }}>{erro}</div>}
        <form onSubmit={enviar}>
          <div className="campo">
            <label htmlFor="usuario">Usuário</label>
            <input type="text" id="usuario" value={usuario} onChange={(e) => setUsuario(e.target.value)} required autoFocus autoComplete="username" />
          </div>
          <div className="campo">
            <label htmlFor="senha">Senha</label>
            <input type="password" id="senha" value={senha} onChange={(e) => setSenha(e.target.value)} required autoComplete="current-password" />
          </div>
          <button type="submit" className="botao botao-primario botao-bloco botao-grande">Entrar</button>
        </form>
      </div>
    </div>
  );
}

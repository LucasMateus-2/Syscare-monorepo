import { useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function Configurar() {
  const { carregando, hasAdmin, admin, configurar } = useAuth();
  const [nome, setNome] = useState("");
  const [usuario, setUsuario] = useState("");
  const [senha, setSenha] = useState("");
  const [senha2, setSenha2] = useState("");
  const [erro, setErro] = useState(null);
  const navigate = useNavigate();

  if (carregando) return null;
  if (hasAdmin && !admin) return <Navigate to="/login" replace />;
  if (admin) return <Navigate to="/" replace />;

  async function enviar(e) {
    e.preventDefault();
    setErro(null);
    if (!usuario.trim()) return setErro("Informe um nome de usuário.");
    if (!senha || senha.length < 4) return setErro("A senha deve ter pelo menos 4 caracteres.");
    if (senha !== senha2) return setErro("As senhas não conferem.");
    try {
      await configurar({ nome, usuario, senha });
      navigate("/");
    } catch (err) {
      setErro(err.message);
    }
  }

  return (
    <div className="tela-centralizada">
      <div className="caixa-login" style={{ maxWidth: 460 }}>
        <div className="login-topo">
          <span className="marca-icone" aria-hidden="true">✚</span>
          <h1>Bem-vindo(a)!</h1>
          <p className="texto-suave">Esta é a primeira vez que o sistema é executado. Crie a conta do administrador para continuar.</p>
        </div>
        {erro && <div className="flash flash-erro" style={{ marginBottom: "1rem" }}>{erro}</div>}
        <form onSubmit={enviar}>
          <div className="campo">
            <label htmlFor="nome">Seu nome</label>
            <input type="text" id="nome" value={nome} onChange={(e) => setNome(e.target.value)} required autoFocus />
          </div>
          <div className="campo">
            <label htmlFor="usuario">Usuário de acesso</label>
            <input type="text" id="usuario" value={usuario} onChange={(e) => setUsuario(e.target.value)} required autoComplete="username" />
          </div>
          <div className="campo">
            <label htmlFor="senha">Senha</label>
            <input type="password" id="senha" value={senha} onChange={(e) => setSenha(e.target.value)} required autoComplete="new-password" />
            <p className="campo-ajuda">Mínimo de 4 caracteres.</p>
          </div>
          <div className="campo">
            <label htmlFor="senha2">Confirmar senha</label>
            <input type="password" id="senha2" value={senha2} onChange={(e) => setSenha2(e.target.value)} required autoComplete="new-password" />
          </div>
          <button type="submit" className="botao botao-primario botao-bloco botao-grande">Criar conta e continuar</button>
        </form>
      </div>
    </div>
  );
}

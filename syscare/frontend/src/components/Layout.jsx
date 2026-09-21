import { useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useFlash } from "../context/FlashContext";

export default function Layout({ children }) {
  const { admin, logout } = useAuth();
  const { mensagem } = useFlash();
  const navigate = useNavigate();
  const [menuAberto, setMenuAberto] = useState(false);

  async function sair() {
    await logout();
    navigate("/login");
  }

  return (
    <div className="app-shell">
      <nav className={`sidebar ${menuAberto ? "aberta" : ""}`} aria-label="Navegação principal">
        <div className="marca">
          <span className="marca-icone" aria-hidden="true">✚</span>
          <span className="marca-texto">Clínica de Enfermagem</span>
        </div>
        <ul className="menu">
          <li><NavLink to="/" end onClick={() => setMenuAberto(false)} className={({ isActive }) => isActive ? "ativo" : ""}><span className="menu-icone">🏠</span> Início</NavLink></li>
          <li><NavLink to="/agenda" onClick={() => setMenuAberto(false)} className={({ isActive }) => isActive ? "ativo" : ""}><span className="menu-icone">📅</span> Agenda</NavLink></li>
          <li><NavLink to="/pacientes" onClick={() => setMenuAberto(false)} className={({ isActive }) => isActive ? "ativo" : ""}><span className="menu-icone">🗂️</span> Pacientes</NavLink></li>
          <li><NavLink to="/pacientes/novo" onClick={() => setMenuAberto(false)} className="menu-destaque"><span className="menu-icone">➕</span> Novo paciente</NavLink></li>
        </ul>
        <div className="sidebar-rodape">
          <div className="admin-info">
            <span className="admin-avatar">{admin?.nome?.[0]?.toUpperCase() || "?"}</span>
            <span>{admin?.nome}</span>
          </div>
          <button className="botao-sair" onClick={sair}>Sair</button>
        </div>
      </nav>

      <button className="menu-mobile-toggle" onClick={() => setMenuAberto((v) => !v)} aria-label="Abrir menu" aria-expanded={menuAberto}>☰</button>

      <main className="conteudo">
        {mensagem && (
          <div className="flash-pilha"><div className="flash flash-sucesso">{mensagem}</div></div>
        )}
        {children}
      </main>
      <div className="selo-marca"><span className="ponto"></span>PolariScore</div>
    </div>
  );
}

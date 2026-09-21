import { createContext, useContext, useEffect, useState, useCallback } from "react";
import { api } from "../api";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [carregando, setCarregando] = useState(true);
  const [hasAdmin, setHasAdmin] = useState(false);
  const [admin, setAdmin] = useState(null);

  const carregarStatus = useCallback(async () => {
    setCarregando(true);
    try {
      const status = await api.get("/auth/status");
      setHasAdmin(status.hasAdmin);
      setAdmin(status.loggedIn ? status.admin : null);
    } finally {
      setCarregando(false);
    }
  }, []);

  useEffect(() => { carregarStatus(); }, [carregarStatus]);

  async function configurar(dados) {
    const resposta = await api.post("/auth/configurar", dados);
    setHasAdmin(true);
    setAdmin(resposta.admin);
    return resposta;
  }

  async function login(usuario, senha) {
    const resposta = await api.post("/auth/login", { usuario, senha });
    setAdmin(resposta.admin);
    return resposta;
  }

  async function logout() {
    await api.post("/auth/logout");
    setAdmin(null);
  }

  return (
    <AuthContext.Provider value={{ carregando, hasAdmin, admin, configurar, login, logout, recarregar: carregarStatus }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}

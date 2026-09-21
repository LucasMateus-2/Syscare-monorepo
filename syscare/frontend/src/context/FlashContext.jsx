import { createContext, useContext, useState, useCallback } from "react";

const FlashContext = createContext(null);

export function FlashProvider({ children }) {
  const [mensagem, setMensagem] = useState(null);

  const setFlash = useCallback((texto) => {
    setMensagem(texto);
    if (texto) {
      setTimeout(() => setMensagem((atual) => (atual === texto ? null : atual)), 5000);
    }
  }, []);

  return (
    <FlashContext.Provider value={{ mensagem, setFlash }}>
      {children}
    </FlashContext.Provider>
  );
}

export function useFlash() {
  return useContext(FlashContext);
}

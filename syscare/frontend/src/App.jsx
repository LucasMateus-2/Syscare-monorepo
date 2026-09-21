import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { FlashProvider } from "./context/FlashContext";
import ProtectedRoute from "./components/ProtectedRoute";

import Configurar from "./pages/Configurar";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import PacientesLista from "./pages/PacientesLista";
import PacienteNovo from "./pages/PacienteNovo";
import PacienteFicha from "./pages/PacienteFicha";
import FeridaDetalhe from "./pages/FeridaDetalhe";
import DiagnosticoDetalhe from "./pages/DiagnosticoDetalhe";
import PrescricaoImprimir from "./pages/PrescricaoImprimir";
import Agenda from "./pages/Agenda";

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <FlashProvider>
          <Routes>
            <Route path="/configurar" element={<Configurar />} />
            <Route path="/login" element={<Login />} />
            <Route path="/pacientes/:id/prescricoes/:prescricaoId/imprimir" element={<PrescricaoImprimir />} />

            <Route path="/" element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
            <Route path="/agenda" element={<ProtectedRoute><Agenda /></ProtectedRoute>} />
            <Route path="/pacientes" element={<ProtectedRoute><PacientesLista /></ProtectedRoute>} />
            <Route path="/pacientes/novo" element={<ProtectedRoute><PacienteNovo /></ProtectedRoute>} />
            <Route path="/pacientes/:id" element={<ProtectedRoute><PacienteFicha /></ProtectedRoute>} />
            <Route path="/pacientes/:id/feridas/:feridaId" element={<ProtectedRoute><FeridaDetalhe /></ProtectedRoute>} />
            <Route path="/pacientes/:id/diagnosticos/:chave" element={<ProtectedRoute><DiagnosticoDetalhe /></ProtectedRoute>} />
          </Routes>
        </FlashProvider>
      </AuthProvider>
    </BrowserRouter>
  );
}

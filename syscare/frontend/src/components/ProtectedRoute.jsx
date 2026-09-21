import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import Layout from "./Layout";

export default function ProtectedRoute({ children }) {
  const { carregando, hasAdmin, admin } = useAuth();

  if (carregando) return null;
  if (!hasAdmin) return <Navigate to="/configurar" replace />;
  if (!admin) return <Navigate to="/login" replace />;

  return <Layout>{children}</Layout>;
}

const BASE = "/api";

async function tratar(resposta) {
  let dados = null;
  try { dados = await resposta.json(); } catch { /* corpo vazio */ }
  if (!resposta.ok) {
    const erro = new Error((dados && dados.erro) || `Erro ${resposta.status}`);
    erro.status = resposta.status;
    throw erro;
  }
  return dados;
}

export const api = {
  get: (rota) => fetch(BASE + rota, { credentials: "include" }).then(tratar),

  post: (rota, corpo) =>
    fetch(BASE + rota, {
      method: "POST",
      credentials: "include",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(corpo || {}),
    }).then(tratar),

  put: (rota, corpo) =>
    fetch(BASE + rota, {
      method: "PUT",
      credentials: "include",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(corpo || {}),
    }).then(tratar),

  delete: (rota) =>
    fetch(BASE + rota, { method: "DELETE", credentials: "include" }).then(tratar),

  // Para envio de formulários com arquivo (foto)
  postForm: (rota, formData) =>
    fetch(BASE + rota, { method: "POST", credentials: "include", body: formData }).then(tratar),

  putForm: (rota, formData) =>
    fetch(BASE + rota, { method: "PUT", credentials: "include", body: formData }).then(tratar),
};

export function urlUpload(subpasta, arquivo) {
  if (!arquivo) return null;
  return `/uploads/${subpasta}/${arquivo}`;
}

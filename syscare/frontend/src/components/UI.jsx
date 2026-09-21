export function Vazio({ icone, children }) {
  return (
    <div className="espaco-vazio">
      <span className="espaco-vazio-icone">{icone}</span>
      {children}
    </div>
  );
}

export function Toggle({ checked, onChange }) {
  return (
    <label className="toggle-switch">
      <input type="checkbox" checked={checked} onChange={(e) => onChange(e.target.checked)} />
      <span className="toggle-slider"></span>
    </label>
  );
}

export function Selo({ status, children }) {
  return <span className={`selo selo-${status}`}>{children || status}</span>;
}

export function Avatar({ src, nome, grande }) {
  const cls = grande ? "avatar-grande" : "avatar";
  const clsEspaco = grande ? "avatar-espaco avatar-espaco-grande" : "avatar-espaco";
  if (src) return <img className={cls} src={src} alt="" />;
  return <span className={clsEspaco}>{(nome || "?")[0]?.toUpperCase()}</span>;
}

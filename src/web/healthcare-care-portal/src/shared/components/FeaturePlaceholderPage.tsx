type FeaturePlaceholderPageProps = {
  title: string;
  domain: string;
  storage: string;
};

export function FeaturePlaceholderPage({ title, domain, storage }: FeaturePlaceholderPageProps) {
  return (
    <section className="page-section">
      <div className="section-header">
        <div>
          <p className="eyebrow">Epic-ready module</p>
          <h2>{title}</h2>
        </div>
        <span className="badge">Foundation</span>
      </div>
      <div className="module-grid">
        <article className="info-panel">
          <span>Domain boundary</span>
          <strong>{domain}</strong>
        </article>
        <article className="info-panel">
          <span>Persistence direction</span>
          <strong>{storage}</strong>
        </article>
        <article className="info-panel">
          <span>Privacy posture</span>
          <strong>Synthetic demo data only; no sensitive details in logs.</strong>
        </article>
      </div>
    </section>
  );
}

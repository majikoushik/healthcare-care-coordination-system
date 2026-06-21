const workstreams = [
  ["Patient coordination", "Registration and consent readiness", "SQL"],
  ["Appointments", "Scheduling lifecycle boundary", "SQL"],
  ["Care plans", "Goals, instructions, and follow-ups", "Cosmos"],
  ["Clinical insights", "Mock AI provider with Azure AI Language readiness", "AI-ready"],
  ["Audit trail", "Traceability without sensitive clinical metadata", "Cosmos"],
  ["Observability", "Correlation IDs, health checks, structured logs", "Ops"]
] as const;

export function DashboardPage() {
  return (
    <section className="dashboard">
      <div className="dashboard-intro">
        <div>
          <p className="eyebrow">Portfolio architecture foundation</p>
          <h2>Cloud-native healthcare coordination, ready for MVP epics.</h2>
          <p>
            The foundation separates transactional healthcare master data from flexible coordination documents,
            prepares clinical AI through a safe mock provider, and keeps privacy constraints visible from day one.
          </p>
        </div>
        <div className="readiness-card">
          <span>Local default</span>
          <strong>AI_PROVIDER=Mock</strong>
          <small>Azure credentials are not required for local development or CI.</small>
        </div>
      </div>
      <div className="workstream-grid">
        {workstreams.map(([title, description, tag]) => (
          <article className="workstream-card" key={title}>
            <span className="badge">{tag}</span>
            <h3>{title}</h3>
            <p>{description}</p>
          </article>
        ))}
      </div>
    </section>
  );
}

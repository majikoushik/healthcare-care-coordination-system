import { NavLink } from "react-router-dom";
import type { PropsWithChildren } from "react";
import { featureRoutes } from "../../app/router";

export function AppShell({ children }: PropsWithChildren) {
  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div className="brand">
          <span className="brand-mark">HC</span>
          <div>
            <strong>Care Coordination</strong>
            <span>Healthcare operations portal</span>
          </div>
        </div>
        <nav aria-label="Primary navigation">
          {featureRoutes.map(({ path, label, icon: Icon }) => (
            <NavLink key={path} to={path} className={({ isActive }) => (isActive ? "nav-link active" : "nav-link")}>
              <Icon size={18} aria-hidden />
              <span>{label}</span>
            </NavLink>
          ))}
        </nav>
      </aside>
      <main className="content">
        <header className="topbar">
          <div>
            <p className="eyebrow">Azure-ready healthcare platform demo</p>
            <h1>Care Coordination Command Center</h1>
          </div>
          <div className="header-status">
            <span className="status-dot" />
            Mock AI local mode
          </div>
        </header>
        {children}
      </main>
    </div>
  );
}

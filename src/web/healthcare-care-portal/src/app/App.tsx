import { Navigate, Route, Routes } from "react-router-dom";
import { AppShell } from "../shared/layout/AppShell";
import { featureRoutes } from "./router";

export function App() {
  return (
    <AppShell>
      <Routes>
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        {featureRoutes.map(({ path, element: Element }) => (
          <Route key={path} path={path} element={<Element />} />
        ))}
      </Routes>
    </AppShell>
  );
}

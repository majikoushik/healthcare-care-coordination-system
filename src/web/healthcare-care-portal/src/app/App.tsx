import { Navigate, Route, Routes } from "react-router-dom";
import { AppShell } from "../shared/layout/AppShell";
import { featureRoutes } from "./router";
import { PatientRegistrationForm } from "../features/patients/PatientRegistrationForm";
import { PatientDetails } from "../features/patients/PatientDetails";

export function App() {
  return (
    <AppShell>
      <Routes>
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        {featureRoutes.map(({ path, element: Element }) => (
          <Route key={path} path={path} element={<Element />} />
        ))}
        <Route path="/patients/new" element={<PatientRegistrationForm />} />
        <Route path="/patients/:id" element={<PatientDetails />} />
      </Routes>
    </AppShell>
  );
}

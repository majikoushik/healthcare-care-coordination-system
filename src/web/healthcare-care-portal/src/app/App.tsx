import { Navigate, Route, Routes } from "react-router-dom";
import { AppShell } from "../shared/layout/AppShell";
import { featureRoutes } from "./router";
import { PatientRegistrationForm } from "../features/patients/PatientRegistrationForm";
import { PatientDetails } from "../features/patients/PatientDetails";
import { ProviderRegistrationForm } from "../features/providers/ProviderRegistrationForm";
import { ProviderDetails } from "../features/providers/ProviderDetails";
import { AppointmentRegistrationForm } from "../features/appointments/AppointmentRegistrationForm";
import { AppointmentDetails } from "../features/appointments/AppointmentDetails";

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
        <Route path="/providers/new" element={<ProviderRegistrationForm />} />
        <Route path="/providers/:id" element={<ProviderDetails />} />
        <Route path="/appointments/new" element={<AppointmentRegistrationForm />} />
        <Route path="/appointments/:id" element={<AppointmentDetails />} />
      </Routes>
    </AppShell>
  );
}

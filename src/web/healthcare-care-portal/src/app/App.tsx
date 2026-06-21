import { Navigate, Route, Routes } from "react-router-dom";
import { AppShell } from "../shared/layout/AppShell";
import { featureRoutes } from "./router";
import { PatientRegistrationForm } from "../features/patients/PatientRegistrationForm";
import { PatientDetails } from "../features/patients/PatientDetails";
import { ProviderRegistrationForm } from "../features/providers/ProviderRegistrationForm";
import { ProviderDetails } from "../features/providers/ProviderDetails";
import { AppointmentRegistrationForm } from "../features/appointments/AppointmentRegistrationForm";
import { AppointmentDetails } from "../features/appointments/AppointmentDetails";
import { CarePlanRegistrationForm } from "../features/care-plans/CarePlanRegistrationForm";
import { CarePlanDetails } from "../features/care-plans/CarePlanDetails";
import { ClinicalInsightAnalysisForm } from "../features/clinical-insights/ClinicalInsightAnalysisForm";
import { ClinicalInsightDetails } from "../features/clinical-insights/ClinicalInsightDetails";
import { FollowUpTaskRegistrationForm } from "../features/follow-up-tasks/FollowUpTaskRegistrationForm";
import { FollowUpTaskDetails } from "../features/follow-up-tasks/FollowUpTaskDetails";

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
        <Route path="/care-plans/new" element={<CarePlanRegistrationForm />} />
        <Route path="/care-plans/:id" element={<CarePlanDetails />} />
        <Route path="/clinical-insights/new" element={<ClinicalInsightAnalysisForm />} />
        <Route path="/clinical-insights/:id" element={<ClinicalInsightDetails />} />
        <Route path="/follow-up-tasks/new" element={<FollowUpTaskRegistrationForm />} />
        <Route path="/follow-up-tasks/:id" element={<FollowUpTaskDetails />} />
      </Routes>
    </AppShell>
  );
}

import { Activity, Bell, CalendarDays, ClipboardList, FileHeart, HeartPulse, LayoutDashboard, ShieldCheck, Stethoscope, UsersRound } from "lucide-react";
import { DashboardPage } from "../features/dashboard/DashboardPage";
import { FeaturePlaceholderPage } from "../shared/components/FeaturePlaceholderPage";
import { PatientList } from "../features/patients/PatientList";
import { ProviderList } from "../features/providers/ProviderList";
import { AppointmentList } from "../features/appointments/AppointmentList";

export const featureRoutes = [
  { path: "/dashboard", label: "Dashboard", icon: LayoutDashboard, element: DashboardPage },
  { path: "/patients", label: "Patients", icon: UsersRound, element: PatientList },
  { path: "/providers", label: "Providers", icon: Stethoscope, element: ProviderList },
  { path: "/appointments", label: "Appointments", icon: CalendarDays, element: AppointmentList },
  { path: "/care-plans", label: "Care Plans", icon: FileHeart, element: () => <FeaturePlaceholderPage title="Care Plans" domain="Care goals, instructions, and follow-up task documents" storage="Azure Cosmos DB" /> },
  { path: "/clinical-insights", label: "Clinical Insights", icon: HeartPulse, element: () => <FeaturePlaceholderPage title="Clinical Insights" domain="Synthetic clinical note insight readiness" storage="Azure Cosmos DB + mock AI provider" /> },
  { path: "/follow-up-tasks", label: "Follow-up Tasks", icon: ClipboardList, element: () => <FeaturePlaceholderPage title="Follow-up Tasks" domain="Task tracking embedded in care coordination workflows" storage="Azure Cosmos DB" /> },
  { path: "/notifications", label: "Notifications", icon: Bell, element: () => <FeaturePlaceholderPage title="Notifications" domain="Email, SMS, and portal notification simulation" storage="Cosmos DB or Service Bus-ready event model" /> },
  { path: "/audit", label: "Audit", icon: ShieldCheck, element: () => <FeaturePlaceholderPage title="Audit" domain="Traceability, event review, and safe audit metadata" storage="Azure Cosmos DB" /> },
  { path: "/system-health", label: "System Health", icon: Activity, element: () => <FeaturePlaceholderPage title="System Health" domain="Health checks, correlation IDs, and observability readiness" storage="Application Insights-ready telemetry" /> }
] as const;

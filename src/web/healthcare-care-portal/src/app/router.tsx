import { Activity, Bell, CalendarDays, ClipboardList, FileHeart, HeartPulse, LayoutDashboard, ShieldCheck, Stethoscope, UsersRound } from "lucide-react";
import { DashboardPage } from "../features/dashboard/DashboardPage";
import { FeaturePlaceholderPage } from "../shared/components/FeaturePlaceholderPage";
import { PatientList } from "../features/patients/PatientList";
import { ProviderList } from "../features/providers/ProviderList";
import { AppointmentList } from "../features/appointments/AppointmentList";
import { CarePlanList } from "../features/care-plans/CarePlanList";
import { ClinicalInsightList } from "../features/clinical-insights/ClinicalInsightList";
import { FollowUpTaskList } from "../features/follow-up-tasks/FollowUpTaskList";
import { FollowUpTaskRegistrationForm } from '../features/follow-up-tasks/FollowUpTaskRegistrationForm';
import { FollowUpTaskDetails } from '../features/follow-up-tasks/FollowUpTaskDetails';
import { NotificationList } from '../features/notifications/NotificationList';
import { AuditList } from '../features/audit/AuditList';
import { NotificationSimulationForm } from '../features/notifications/NotificationSimulationForm';
import { NotificationDetails } from '../features/notifications/NotificationDetails';

export const featureRoutes = [
  { path: "/dashboard", label: "Dashboard", icon: LayoutDashboard, element: DashboardPage },
  { path: "/patients", label: "Patients", icon: UsersRound, element: PatientList },
  { path: "/providers", label: "Providers", icon: Stethoscope, element: ProviderList },
  { path: "/appointments", label: "Appointments", icon: CalendarDays, element: AppointmentList },
  { path: "/care-plans", label: "Care Plans", icon: FileHeart, element: CarePlanList },
  { path: "/clinical-insights", label: "Clinical Insights", icon: HeartPulse, element: ClinicalInsightList },
  { path: "/follow-up-tasks", label: "Follow-up Tasks", icon: ClipboardList, element: FollowUpTaskList },
  { path: "/notifications", label: "Notifications", icon: Bell, element: NotificationList },
  { path: "/audit", label: "Audit", icon: ShieldCheck, element: AuditList },
  { path: "/system-health", label: "System Health", icon: Activity, element: () => <FeaturePlaceholderPage title="System Health" domain="Health checks, correlation IDs, and observability readiness" storage="Application Insights-ready telemetry" /> }
] as const;

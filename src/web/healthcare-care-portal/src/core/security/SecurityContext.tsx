import { createContext, useContext, useState, ReactNode, useEffect } from "react";

export type DemoRole = 
  | "Patient" 
  | "Provider" 
  | "CareCoordinator" 
  | "Admin" 
  | "Auditor" 
  | "System";

export interface SecurityContextState {
  role: DemoRole;
  userId: string;
  setRole: (role: DemoRole) => void;
  hasPermission: (permission: string) => boolean;
}

const defaultContext: SecurityContextState = {
  role: "CareCoordinator",
  userId: "00000000-0000-0000-0000-000000000000",
  setRole: () => {},
  hasPermission: () => true,
};

export const SecurityContext = createContext<SecurityContextState>(defaultContext);

// Hardcoded permission mapping for the frontend Demo Mode UI to avoid unnecessary API roundtrips
const rolePermissions: Record<DemoRole, string[]> = {
  Patient: [
    "PatientProfile.Read", "Appointment.Read", "CarePlan.Read", "FollowUpTask.Read", "Notification.Read"
  ],
  Provider: [
    "PatientProfile.Read", "Appointment.Read", "Appointment.StatusUpdate", 
    "CarePlan.Read", "CarePlan.Write", "ClinicalInsight.Read", "ClinicalInsight.Analyze",
    "FollowUpTask.Read", "FollowUpTask.Write"
  ],
  CareCoordinator: [
    "PatientProfile.Read", "PatientProfile.Write", "ProviderProfile.Read",
    "Appointment.Read", "Appointment.Write", "Appointment.StatusUpdate",
    "CarePlan.Read", "CarePlan.Write", "CarePlan.StatusUpdate",
    "ClinicalInsight.Read", "ClinicalInsight.Review",
    "FollowUpTask.Read", "FollowUpTask.Write", "FollowUpTask.StatusUpdate",
    "Notification.Read", "Notification.Write", "Notification.SimulateSend"
  ],
  Admin: [
    // Simulating "All"
    "PatientProfile.Read", "PatientProfile.Write", "ProviderProfile.Read", "ProviderProfile.Write",
    "Appointment.Read", "Appointment.Write", "Appointment.StatusUpdate",
    "CarePlan.Read", "CarePlan.Write", "CarePlan.StatusUpdate",
    "ClinicalInsight.Read", "ClinicalInsight.Analyze", "ClinicalInsight.Review",
    "FollowUpTask.Read", "FollowUpTask.Write", "FollowUpTask.StatusUpdate",
    "Notification.Read", "Notification.Write", "Notification.SimulateSend",
    "Audit.Read", "Audit.Write", "SystemHealth.Read"
  ],
  Auditor: [
    "Audit.Read", "PatientProfile.Read", "ProviderProfile.Read", "Appointment.Read",
    "CarePlan.Read", "ClinicalInsight.Read", "Notification.Read"
  ],
  System: [
    "Audit.Write", "Notification.Write", "SystemHealth.Read"
  ]
};

export const SecurityProvider = ({ children }: { children: ReactNode }) => {
  const [role, setRoleState] = useState<DemoRole>(() => {
    return (localStorage.getItem("demoRole") as DemoRole) || "CareCoordinator";
  });
  const [userId] = useState("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"); // Static demo ID

  const setRole = (newRole: DemoRole) => {
    localStorage.setItem("demoRole", newRole);
    setRoleState(newRole);
    // Reload page to clear TanStack Query cache and reset state for the new role securely
    window.location.reload(); 
  };

  const hasPermission = (permission: string) => {
    return rolePermissions[role]?.includes(permission) ?? false;
  };

  return (
    <SecurityContext.Provider value={{ role, userId, setRole, hasPermission }}>
      {children}
    </SecurityContext.Provider>
  );
};

export const useSecurity = () => useContext(SecurityContext);

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

// ---------------------------------------------------------------------------
// Safe storage helpers — guarded for test environments and private browsing
// ---------------------------------------------------------------------------
const safeStorageGet = (key: string): string | null => {
  try {
    return typeof localStorage !== "undefined" ? localStorage.getItem(key) : null;
  } catch {
    return null;
  }
};

const safeStorageSet = (key: string, value: string): void => {
  try {
    if (typeof localStorage !== "undefined") localStorage.setItem(key, value);
  } catch {
    // Storage unavailable — silently ignore
  }
};

const getOrCreateDemoUserId = () => {
  const existing = safeStorageGet("demoUserId");
  if (existing) {
    return existing;
  }

  const generated =
    typeof crypto !== "undefined" && "randomUUID" in crypto
      ? crypto.randomUUID()
      : `demo-${Date.now()}-${Math.random().toString(36).slice(2)}`;

  safeStorageSet("demoUserId", generated);
  return generated;
};

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
    "CarePlan.Read", "ClinicalInsight.Read", "Notification.Read", "SystemHealth.Read"
  ],
  System: [
    "Audit.Write", "Notification.Write", "SystemHealth.Read"
  ]
};

export const SecurityProvider = ({ children }: { children: ReactNode }) => {
  const [role, setRoleState] = useState<DemoRole>(() => {
    return (safeStorageGet("demoRole") as DemoRole) || "CareCoordinator";
  });
  const [userId] = useState(getOrCreateDemoUserId);

  const setRole = (newRole: DemoRole) => {
    safeStorageSet("demoRole", newRole);
    setRoleState(newRole);
    // Reload page to clear TanStack Query cache and reset state for the new role securely
    if (typeof window !== "undefined") window.location.reload();
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

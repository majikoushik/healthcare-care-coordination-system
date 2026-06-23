import { renderHook, act } from "@testing-library/react";
import { describe, expect, it, beforeEach } from "vitest";
import { SecurityProvider, useSecurity } from "./SecurityContext";
import type { ReactNode } from "react";

const wrapper = ({ children }: { children: ReactNode }) => (
  <SecurityProvider>{children}</SecurityProvider>
);

describe("SecurityContext — localStorage-safe behavior", () => {
  beforeEach(() => {
    localStorage.clear();
  });

  it("returns the default CareCoordinator role when no role is stored", () => {
    const { result } = renderHook(() => useSecurity(), { wrapper });
    expect(result.current.role).toBe("CareCoordinator");
  });

  it("reads the stored demo role from localStorage on mount", () => {
    localStorage.setItem("demoRole", "Auditor");
    const { result } = renderHook(() => useSecurity(), { wrapper });
    expect(result.current.role).toBe("Auditor");
  });

  it("persists the role to localStorage when setRole is called", () => {
    const { result } = renderHook(() => useSecurity(), { wrapper });
    // Intercept window.location.reload so the test doesn't crash
    const reloadMock = () => {};
    Object.defineProperty(window, "location", {
      value: { reload: reloadMock },
      writable: true,
    });

    act(() => {
      result.current.setRole("Admin");
    });

    expect(localStorage.getItem("demoRole")).toBe("Admin");
  });

  it("creates and persists a demoUserId on first access", () => {
    const { result } = renderHook(() => useSecurity(), { wrapper });
    const userId = result.current.userId;
    expect(typeof userId).toBe("string");
    expect(userId.length).toBeGreaterThan(0);
    expect(localStorage.getItem("demoUserId")).toBe(userId);
  });

  it("hasPermission returns true for known CareCoordinator permissions", () => {
    const { result } = renderHook(() => useSecurity(), { wrapper });
    expect(result.current.hasPermission("PatientProfile.Read")).toBe(true);
    expect(result.current.hasPermission("CarePlan.Write")).toBe(true);
  });

  it("hasPermission returns false for permissions outside the role", () => {
    localStorage.setItem("demoRole", "Patient");
    const { result } = renderHook(() => useSecurity(), { wrapper });
    expect(result.current.hasPermission("PatientProfile.Write")).toBe(false);
    expect(result.current.hasPermission("Audit.Read")).toBe(false);
  });
});

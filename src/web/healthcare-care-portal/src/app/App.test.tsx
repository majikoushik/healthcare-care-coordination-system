import { render, screen } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";
import { App } from "./App";
import { AppProviders } from "./providers";

// ---------------------------------------------------------------------------
// Mock all API modules so that no real HTTP calls are made during the test.
// The App smoke test only verifies that the portal shell renders correctly.
// ---------------------------------------------------------------------------

vi.mock("../core/api/patientApi", () => ({
  patientApi: { getPatients: vi.fn().mockResolvedValue([]) },
}));

vi.mock("../core/api/providerApi", () => ({
  providerApi: { getProviders: vi.fn().mockResolvedValue([]) },
}));

vi.mock("../core/api/appointmentApi", () => ({
  appointmentApi: { getAppointments: vi.fn().mockResolvedValue([]) },
}));

vi.mock("../core/api/carePlanApi", () => ({
  carePlanApi: { getCarePlans: vi.fn().mockResolvedValue([]) },
}));

vi.mock("../core/api/followUpTaskApi", () => ({
  followUpTaskApi: {
    getOverdue: vi.fn().mockResolvedValue([]),
    getDueToday: vi.fn().mockResolvedValue([]),
  },
}));

vi.mock("../core/api/notificationApi", () => ({
  notificationApi: { getNotifications: vi.fn().mockResolvedValue([]) },
}));

vi.mock("../core/api/auditApi", () => ({
  auditApi: { getAuditEvents: vi.fn().mockResolvedValue({ data: [] }) },
}));

vi.mock("../core/api/clinicalInsightApi", () => ({
  clinicalInsightApi: { getInsights: vi.fn().mockResolvedValue([]) },
}));

describe("App — portal shell smoke test", () => {
  it("renders the healthcare portal shell without crashing", () => {
    render(
      <AppProviders>
        <App />
      </AppProviders>
    );

    // The sidebar/header should be visible — these are stable UI landmarks.
    // 'Care Coordination' appears in both the brand and the dashboard heading.
    const matches = screen.getAllByText(/Care Coordination/i);
    expect(matches.length).toBeGreaterThan(0);
  });

  it("renders the synthetic data mode notice", () => {
    render(
      <AppProviders>
        <App />
      </AppProviders>
    );

    expect(screen.getByText(/Synthetic data mode/i)).toBeInTheDocument();
  });
});

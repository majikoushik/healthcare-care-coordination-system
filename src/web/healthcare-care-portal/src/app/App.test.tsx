import "@testing-library/jest-dom/vitest";
import { render, screen } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";
import { App } from "./App";
import { AppProviders } from "./providers";

vi.mock("../core/api/patientApi", () => ({
  patientApi: { getPatients: vi.fn().mockResolvedValue([]) }
}));

vi.mock("../core/api/providerApi", () => ({
  providerApi: { getProviders: vi.fn().mockResolvedValue([]) }
}));

vi.mock("../core/api/appointmentApi", () => ({
  appointmentApi: { getAppointments: vi.fn().mockResolvedValue([]) }
}));

vi.mock("../core/api/carePlanApi", () => ({
  carePlanApi: { getCarePlans: vi.fn().mockResolvedValue([]) }
}));

vi.mock("../core/api/followUpTaskApi", () => ({
  followUpTaskApi: {
    getOverdue: vi.fn().mockResolvedValue([]),
    getDueToday: vi.fn().mockResolvedValue([])
  }
}));

vi.mock("../core/api/notificationApi", () => ({
  notificationApi: { getNotifications: vi.fn().mockResolvedValue([]) }
}));

describe("App", () => {
  it("renders the healthcare operations portal shell", () => {
    render(
      <AppProviders>
        <App />
      </AppProviders>
    );

    expect(screen.getByText("Care Coordination Command Center")).toBeInTheDocument();
    expect(screen.getByText("Synthetic data mode")).toBeInTheDocument();
  });
});

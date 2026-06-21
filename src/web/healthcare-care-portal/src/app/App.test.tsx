import "@testing-library/jest-dom/vitest";
import { render, screen } from "@testing-library/react";
import { describe, expect, it } from "vitest";
import { App } from "./App";
import { AppProviders } from "./providers";

describe("App", () => {
  it("renders the healthcare operations portal shell", () => {
    render(
      <AppProviders>
        <App />
      </AppProviders>
    );

    expect(screen.getByText("Care Coordination Command Center")).toBeInTheDocument();
    expect(screen.getByText("Mock AI local mode")).toBeInTheDocument();
  });
});

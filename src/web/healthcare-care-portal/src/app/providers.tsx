import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import type { PropsWithChildren } from "react";
import { BrowserRouter } from "react-router-dom";
import { SecurityProvider } from "../core/security/SecurityContext";
import { ErrorBoundary } from "../shared/layout/ErrorBoundary";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      staleTime: 30_000
    }
  }
});

export function AppProviders({ children }: PropsWithChildren) {
  return (
    <ErrorBoundary>
      <SecurityProvider>
        <QueryClientProvider client={queryClient}>
          <BrowserRouter>{children}</BrowserRouter>
        </QueryClientProvider>
      </SecurityProvider>
    </ErrorBoundary>
  );
}

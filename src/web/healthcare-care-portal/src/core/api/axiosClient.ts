import axios, { type AxiosInstance } from "axios";
import { apiConfig } from "./apiConfig";

// ---------------------------------------------------------------------------
// Safe helpers — guarded for test environments where browser APIs may not
// be available (e.g. jsdom under Vitest without full browser globals).
// ---------------------------------------------------------------------------

const safeRandomUUID = (): string => {
  if (typeof crypto !== "undefined" && typeof crypto.randomUUID === "function") {
    return crypto.randomUUID();
  }
  // Fallback for environments where crypto.randomUUID is not available
  return `${Date.now()}-${Math.random().toString(36).slice(2)}`;
};

const safeLocalStorageGet = (key: string): string | null => {
  try {
    return typeof localStorage !== "undefined" ? localStorage.getItem(key) : null;
  } catch {
    return null;
  }
};

const safeLocalStorageSet = (key: string, value: string): void => {
  try {
    if (typeof localStorage !== "undefined") {
      localStorage.setItem(key, value);
    }
  } catch {
    // Ignore — storage unavailable in some test/private-browsing contexts
  }
};

const getOrCreateDemoUserId = (): string => {
  const existing = safeLocalStorageGet("demoUserId");
  if (existing) return existing;
  const generated = safeRandomUUID();
  safeLocalStorageSet("demoUserId", generated);
  return generated;
};

// ---------------------------------------------------------------------------
// Factory — creates a per-service Axios instance with shared interceptors.
// Each service has its own base URL so the portal can reach multiple backends
// without a gateway/proxy.
// ---------------------------------------------------------------------------
export const createServiceClient = (baseUrl: string): AxiosInstance => {
  const client = axios.create({
    baseURL: baseUrl,
    headers: {
      "Content-Type": "application/json",
    },
  });

  client.interceptors.request.use((config) => {
    config.headers[apiConfig.correlationHeaderName] = safeRandomUUID();

    const role = safeLocalStorageGet("demoRole") ?? "CareCoordinator";
    const userId = getOrCreateDemoUserId();

    config.headers["X-Demo-User-Role"] = role;
    config.headers["X-Demo-User-Id"] = userId;

    return config;
  });

  client.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response?.status === 401 || error.response?.status === 403) {
        console.warn("Authorization denied:", error.response.status);
      }
      return Promise.reject(error);
    }
  );

  return client;
};

// ---------------------------------------------------------------------------
// Convenience re-export: a default client targeting Patient.Api for any
// legacy imports that still reference `axiosClient` directly.
// Prefer using the named per-service clients exported from each *Api.ts file.
// ---------------------------------------------------------------------------
export const axiosClient = createServiceClient(apiConfig.patientApiBaseUrl);

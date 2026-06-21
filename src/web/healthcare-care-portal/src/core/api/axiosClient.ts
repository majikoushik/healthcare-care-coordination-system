import axios from "axios";
import { apiConfig } from "./apiConfig";

export const axiosClient = axios.create({
  baseURL: apiConfig.baseUrl,
  headers: {
    "Content-Type": "application/json"
  }
});

const getDemoUserId = () => {
  const existing = localStorage.getItem("demoUserId");
  if (existing) {
    return existing;
  }

  const generated =
    typeof crypto !== "undefined" && "randomUUID" in crypto
      ? crypto.randomUUID()
      : `demo-${Date.now()}-${Math.random().toString(36).slice(2)}`;

  localStorage.setItem("demoUserId", generated);
  return generated;
};

axiosClient.interceptors.request.use((config) => {
  config.headers[apiConfig.correlationHeaderName] = crypto.randomUUID();
  
  const role = localStorage.getItem("demoRole") || "CareCoordinator";
  const userId = getDemoUserId();
  
  config.headers["X-Demo-User-Role"] = role;
  config.headers["X-Demo-User-Id"] = userId;
  
  return config;
});

axiosClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401 || error.response?.status === 403) {
      // For a demo app, we'll just alert the user. In a real app we would redirect to a 403 Access Denied page.
      console.warn("Authorization denied:", error.response.status);
    }
    return Promise.reject(error);
  }
);

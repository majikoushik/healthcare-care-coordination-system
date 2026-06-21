import axios from "axios";
import { apiConfig } from "./apiConfig";

export const axiosClient = axios.create({
  baseURL: apiConfig.baseUrl,
  headers: {
    "Content-Type": "application/json"
  }
});

axiosClient.interceptors.request.use((config) => {
  config.headers[apiConfig.correlationHeaderName] = crypto.randomUUID();
  return config;
});

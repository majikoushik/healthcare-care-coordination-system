import { createServiceClient } from "./axiosClient";
import { apiConfig } from "./apiConfig";
import { Provider, RegisterProviderRequest, Specialty } from "../../features/providers/types";
import { ApiResponse } from "./patientApi";

const providerClient = createServiceClient(apiConfig.providerApiBaseUrl);

export const providerApi = {
  getProviders: async (): Promise<Provider[]> => {
    const response = await providerClient.get<ApiResponse<Provider[]>>("/api/v1/providers");
    return response.data.data;
  },

  getProviderById: async (id: string): Promise<Provider> => {
    const response = await providerClient.get<ApiResponse<Provider>>(`/api/v1/providers/${id}`);
    return response.data.data;
  },

  getProvidersBySpecialty: async (specialty: Specialty): Promise<Provider[]> => {
    const response = await providerClient.get<ApiResponse<Provider[]>>(`/api/v1/providers/specialty/${specialty}`);
    return response.data.data;
  },

  registerProvider: async (data: RegisterProviderRequest): Promise<Provider> => {
    const response = await providerClient.post<ApiResponse<Provider>>("/api/v1/providers", data);
    return response.data.data;
  },
};

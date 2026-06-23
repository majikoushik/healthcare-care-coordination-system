import { createServiceClient } from "./axiosClient";
import { apiConfig } from "./apiConfig";
import { Patient, RegisterPatientRequest } from "../../features/patients/types";

const patientClient = createServiceClient(apiConfig.patientApiBaseUrl);

export interface ApiResponse<T> {
  data: T;
  correlationId: string;
  timestamp: string;
}

export const patientApi = {
  getPatients: async (): Promise<Patient[]> => {
    const response = await patientClient.get<ApiResponse<Patient[]>>("/api/v1/patients");
    return response.data.data;
  },

  getPatientById: async (id: string): Promise<Patient> => {
    const response = await patientClient.get<ApiResponse<Patient>>(`/api/v1/patients/${id}`);
    return response.data.data;
  },

  registerPatient: async (data: RegisterPatientRequest): Promise<Patient> => {
    const response = await patientClient.post<ApiResponse<Patient>>("/api/v1/patients", data);
    return response.data.data;
  },
};

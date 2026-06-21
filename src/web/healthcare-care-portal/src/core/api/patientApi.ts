import { axiosClient } from "./axiosClient";
import { Patient, RegisterPatientRequest } from "../../features/patients/types";

export interface ApiResponse<T> {
    data: T;
    correlationId: string;
    timestamp: string;
}

export const patientApi = {
  getPatients: async (): Promise<Patient[]> => {
    const response = await axiosClient.get<ApiResponse<Patient[]>>("/api/v1/patients");
    return response.data.data;
  },

  getPatientById: async (id: string): Promise<Patient> => {
    const response = await axiosClient.get<ApiResponse<Patient>>(`/api/v1/patients/${id}`);
    return response.data.data;
  },

  registerPatient: async (data: RegisterPatientRequest): Promise<Patient> => {
    const response = await axiosClient.post<ApiResponse<Patient>>("/api/v1/patients", data);
    return response.data.data;
  }
};

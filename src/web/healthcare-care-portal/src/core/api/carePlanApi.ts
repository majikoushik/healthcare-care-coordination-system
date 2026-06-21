import { axiosClient } from "./axiosClient";
import { 
  CarePlanDocument, 
  CreateCarePlanRequest, 
  UpdateCarePlanStatusRequest,
  AddCarePlanGoalRequest,
  UpdateCarePlanGoalStatusRequest,
  AddCarePlanTaskRequest,
  UpdateCarePlanTaskStatusRequest
} from "../../features/care-plans/types";
import { ApiResponse } from "./patientApi";

export const carePlanApi = {
  getCarePlans: async (): Promise<CarePlanDocument[]> => {
    const response = await axiosClient.get<ApiResponse<CarePlanDocument[]>>("/api/v1/care-plans");
    return response.data.data;
  },

  getCarePlanById: async (id: string): Promise<CarePlanDocument> => {
    const response = await axiosClient.get<ApiResponse<CarePlanDocument>>(`/api/v1/care-plans/${id}`);
    return response.data.data;
  },

  getCarePlansByPatientId: async (patientId: string): Promise<CarePlanDocument[]> => {
    const response = await axiosClient.get<ApiResponse<CarePlanDocument[]>>(`/api/v1/patients/${patientId}/care-plans`);
    return response.data.data;
  },

  getCarePlansByProviderId: async (providerId: string): Promise<CarePlanDocument[]> => {
    const response = await axiosClient.get<ApiResponse<CarePlanDocument[]>>(`/api/v1/providers/${providerId}/care-plans`);
    return response.data.data;
  },

  createCarePlan: async (data: CreateCarePlanRequest): Promise<CarePlanDocument> => {
    const response = await axiosClient.post<ApiResponse<CarePlanDocument>>("/api/v1/care-plans", data);
    return response.data.data;
  },

  updateCarePlanStatus: async (id: string, data: UpdateCarePlanStatusRequest): Promise<CarePlanDocument> => {
    const response = await axiosClient.patch<ApiResponse<CarePlanDocument>>(`/api/v1/care-plans/${id}/status`, data);
    return response.data.data;
  },

  addGoal: async (id: string, data: AddCarePlanGoalRequest): Promise<CarePlanDocument> => {
    const response = await axiosClient.post<ApiResponse<CarePlanDocument>>(`/api/v1/care-plans/${id}/goals`, data);
    return response.data.data;
  },

  updateGoalStatus: async (id: string, goalId: string, data: UpdateCarePlanGoalStatusRequest): Promise<CarePlanDocument> => {
    const response = await axiosClient.patch<ApiResponse<CarePlanDocument>>(`/api/v1/care-plans/${id}/goals/${goalId}/status`, data);
    return response.data.data;
  },

  addTask: async (id: string, data: AddCarePlanTaskRequest): Promise<CarePlanDocument> => {
    const response = await axiosClient.post<ApiResponse<CarePlanDocument>>(`/api/v1/care-plans/${id}/tasks`, data);
    return response.data.data;
  },

  updateTaskStatus: async (id: string, taskId: string, data: UpdateCarePlanTaskStatusRequest): Promise<CarePlanDocument> => {
    const response = await axiosClient.patch<ApiResponse<CarePlanDocument>>(`/api/v1/care-plans/${id}/tasks/${taskId}/status`, data);
    return response.data.data;
  }
};

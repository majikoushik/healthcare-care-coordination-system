import { axiosClient } from "./axiosClient";
import { 
  ClinicalNoteInsight, 
  AnalyzeNoteRequest, 
  UpdateReviewStatusRequest 
} from "../../features/clinical-insights/types";
import { ApiResponse } from "./patientApi";

export const clinicalInsightApi = {
  getInsights: async (): Promise<ClinicalNoteInsight[]> => {
    const response = await axiosClient.get<ApiResponse<ClinicalNoteInsight[]>>("/api/v1/clinical-insights");
    return response.data.data;
  },

  getInsightById: async (id: string): Promise<ClinicalNoteInsight> => {
    const response = await axiosClient.get<ApiResponse<ClinicalNoteInsight>>(`/api/v1/clinical-insights/${id}`);
    return response.data.data;
  },

  getInsightsByPatientId: async (patientId: string): Promise<ClinicalNoteInsight[]> => {
    const response = await axiosClient.get<ApiResponse<ClinicalNoteInsight[]>>(`/api/v1/patients/${patientId}/clinical-insights`);
    return response.data.data;
  },

  getInsightsByCarePlanId: async (carePlanId: string): Promise<ClinicalNoteInsight[]> => {
    const response = await axiosClient.get<ApiResponse<ClinicalNoteInsight[]>>(`/api/v1/care-plans/${carePlanId}/clinical-insights`);
    return response.data.data;
  },

  analyzeNote: async (data: AnalyzeNoteRequest): Promise<ClinicalNoteInsight> => {
    const response = await axiosClient.post<ApiResponse<ClinicalNoteInsight>>("/api/v1/clinical-insights/analyze", data);
    return response.data.data;
  },

  updateReviewStatus: async (id: string, data: UpdateReviewStatusRequest): Promise<ClinicalNoteInsight> => {
    const response = await axiosClient.patch<ApiResponse<ClinicalNoteInsight>>(`/api/v1/clinical-insights/${id}/review-status`, data);
    return response.data.data;
  },

  getAiProviderStatus: async (): Promise<any> => {
    const response = await axiosClient.get("/api/v1/clinical-insights/ai-provider/status");
    return response.data;
  }
};

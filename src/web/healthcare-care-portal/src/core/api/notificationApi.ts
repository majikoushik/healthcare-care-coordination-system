import { createServiceClient } from "./axiosClient";
import { apiConfig } from "./apiConfig";
import { CreateNotificationRequest, NotificationRecord, SimulateSendResponse } from "../../features/notifications/types";
import type { ApiResponse } from "../types/api";

const notificationClient = createServiceClient(apiConfig.notificationApiBaseUrl);

export const notificationApi = {
  getNotifications: async (): Promise<NotificationRecord[]> => {
    const response = await notificationClient.get<ApiResponse<NotificationRecord[]>>("/api/v1/notifications");
    return response.data.data;
  },

  getNotificationById: async (id: string): Promise<NotificationRecord> => {
    const response = await notificationClient.get<ApiResponse<NotificationRecord>>(`/api/v1/notifications/${id}`);
    return response.data.data;
  },

  getNotificationsByPatient: async (patientId: string): Promise<NotificationRecord[]> => {
    const response = await notificationClient.get<ApiResponse<NotificationRecord[]>>(`/api/v1/patients/${patientId}/notifications`);
    return response.data.data;
  },

  getNotificationsByRelatedEntity: async (entityType: string, entityId: string): Promise<NotificationRecord[]> => {
    const response = await notificationClient.get<ApiResponse<NotificationRecord[]>>(`/api/v1/notifications/related/${entityType}/${entityId}`);
    return response.data.data;
  },

  createNotification: async (data: CreateNotificationRequest): Promise<NotificationRecord> => {
    const response = await notificationClient.post<ApiResponse<NotificationRecord>>("/api/v1/notifications", data);
    return response.data.data;
  },

  simulateSend: async (id: string): Promise<SimulateSendResponse> => {
    const response = await notificationClient.post<ApiResponse<SimulateSendResponse>>(`/api/v1/notifications/${id}/simulate-send`);
    return response.data.data;
  },
};

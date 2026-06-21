import { axiosClient } from './axiosClient';
import { FollowUpTask, CreateFollowUpTaskRequest, UpdateFollowUpTaskStatusRequest } from '../../features/follow-up-tasks/types';
import type { ApiResponse } from '../types/api';

const BASE_URL = '/api/v1/follow-up-tasks';

export const followUpTaskApi = {
    getTasks: async (): Promise<FollowUpTask[]> => {
        const response = await axiosClient.get<ApiResponse<FollowUpTask[]>>(BASE_URL);
        return response.data.data;
    },

    getTaskById: async (id: string): Promise<FollowUpTask> => {
        const response = await axiosClient.get<ApiResponse<FollowUpTask>>(`${BASE_URL}/${id}`);
        return response.data.data;
    },

    getTasksByPatientId: async (patientId: string): Promise<FollowUpTask[]> => {
        const response = await axiosClient.get<ApiResponse<FollowUpTask[]>>(`/api/v1/patients/${patientId}/follow-up-tasks`);
        return response.data.data;
    },

    getTasksByCarePlanId: async (carePlanId: string): Promise<FollowUpTask[]> => {
        const response = await axiosClient.get<ApiResponse<FollowUpTask[]>>(`/api/v1/care-plans/${carePlanId}/follow-up-tasks`);
        return response.data.data;
    },
    
    getDueToday: async (): Promise<FollowUpTask[]> => {
        const response = await axiosClient.get<ApiResponse<FollowUpTask[]>>(`${BASE_URL}/due-today`);
        return response.data.data;
    },

    getOverdue: async (): Promise<FollowUpTask[]> => {
        const response = await axiosClient.get<ApiResponse<FollowUpTask[]>>(`${BASE_URL}/overdue`);
        return response.data.data;
    },

    createTask: async (data: CreateFollowUpTaskRequest): Promise<FollowUpTask> => {
        const response = await axiosClient.post<ApiResponse<FollowUpTask>>(BASE_URL, data);
        return response.data.data;
    },

    updateTaskStatus: async (id: string, data: UpdateFollowUpTaskStatusRequest): Promise<FollowUpTask> => {
        const response = await axiosClient.patch<ApiResponse<FollowUpTask>>(`${BASE_URL}/${id}/status`, data);
        return response.data.data;
    }
};

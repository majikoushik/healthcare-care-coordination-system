import { axiosClient } from './axiosClient';
import type { ApiResponse } from '../types/api';
import { AuditEvent } from '../../features/audit/types';

export const auditApi = {
  getAuditEvents: async (): Promise<ApiResponse<AuditEvent[]>> => {
    const response = await axiosClient.get('/api/v1/audit-events');
    return response.data;
  },

  getAuditEventById: async (id: string): Promise<ApiResponse<AuditEvent>> => {
    const response = await axiosClient.get(`/api/v1/audit-events/${id}`);
    return response.data;
  },

  getAuditEventsByCorrelationId: async (correlationId: string): Promise<ApiResponse<AuditEvent[]>> => {
    const response = await axiosClient.get(`/api/v1/audit-events/correlation/${correlationId}`);
    return response.data;
  },

  getAuditEventsByPatientId: async (patientId: string): Promise<ApiResponse<AuditEvent[]>> => {
    const response = await axiosClient.get(`/api/v1/patients/${patientId}/audit-events`);
    return response.data;
  }
};

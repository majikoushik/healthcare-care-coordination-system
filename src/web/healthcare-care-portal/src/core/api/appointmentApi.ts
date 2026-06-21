import { axiosClient } from "./axiosClient";
import { Appointment, ScheduleAppointmentRequest, UpdateAppointmentStatusRequest } from "../../features/appointments/types";
import { ApiResponse } from "./patientApi";

export const appointmentApi = {
  getAppointments: async (): Promise<Appointment[]> => {
    const response = await axiosClient.get<ApiResponse<Appointment[]>>("/api/v1/appointments");
    return response.data.data;
  },

  getAppointmentById: async (id: string): Promise<Appointment> => {
    const response = await axiosClient.get<ApiResponse<Appointment>>(`/api/v1/appointments/${id}`);
    return response.data.data;
  },

  getAppointmentsByPatientId: async (patientId: string): Promise<Appointment[]> => {
    const response = await axiosClient.get<ApiResponse<Appointment[]>>(`/api/v1/patients/${patientId}/appointments`);
    return response.data.data;
  },

  getAppointmentsByProviderId: async (providerId: string): Promise<Appointment[]> => {
    const response = await axiosClient.get<ApiResponse<Appointment[]>>(`/api/v1/providers/${providerId}/appointments`);
    return response.data.data;
  },

  scheduleAppointment: async (data: ScheduleAppointmentRequest): Promise<Appointment> => {
    const response = await axiosClient.post<ApiResponse<Appointment>>("/api/v1/appointments", data);
    return response.data.data;
  },

  updateAppointmentStatus: async (id: string, data: UpdateAppointmentStatusRequest): Promise<Appointment> => {
    const response = await axiosClient.patch<ApiResponse<Appointment>>(`/api/v1/appointments/${id}/status`, data);
    return response.data.data;
  }
};

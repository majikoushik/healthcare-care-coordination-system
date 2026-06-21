export enum AppointmentType {
  Consultation = 0,
  FollowUp = 1,
  LabReview = 2,
  MedicationReview = 3,
  CarePlanReview = 4
}

export enum AppointmentStatus {
  Requested = 0,
  Scheduled = 1,
  CheckedIn = 2,
  Completed = 3,
  Cancelled = 4,
  NoShow = 5
}

export interface Appointment {
  id: string;
  patientId: string;
  providerId: string;
  appointmentDateTime: string;
  visitReason: string;
  type: AppointmentType;
  status: AppointmentStatus;
  notes: string;
  createdAt: string;
  updatedAt?: string;
}

export interface ScheduleAppointmentRequest {
  patientId: string;
  providerId: string;
  appointmentDateTime: string;
  visitReason: string;
  type: AppointmentType;
  notes?: string;
}

export interface UpdateAppointmentStatusRequest {
  status: AppointmentStatus;
  reason: string;
  updatedBy: string;
}

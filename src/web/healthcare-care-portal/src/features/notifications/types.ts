export enum NotificationChannel {
  EmailSimulation = 0,
  SmsSimulation = 1,
  PortalSimulation = 2
}

export enum NotificationStatus {
  Requested = 0,
  Queued = 1,
  SimulatedSent = 2,
  SimulatedFailed = 3,
  Cancelled = 4
}

export enum RecipientType {
  Patient = 0,
  Provider = 1,
  CareCoordinator = 2,
  Admin = 3,
  Auditor = 4,
  System = 5
}

export interface NotificationRecord {
  id: string;
  patientId?: string;
  providerId?: string;
  relatedEntityType: string;
  relatedEntityId: string;
  notificationType: string;
  channel: NotificationChannel;
  recipientType: RecipientType;
  recipientReference: string;
  subject: string;
  messageSummary: string;
  status: NotificationStatus;
  attemptCount: number;
  lastAttemptedAt?: string;
  sentAt?: string;
  failureReason?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateNotificationRequest {
  patientId?: string;
  providerId?: string;
  relatedEntityType: string;
  relatedEntityId: string;
  notificationType: string;
  channel: NotificationChannel;
  recipientType: RecipientType;
  recipientReference: string;
  subject: string;
  messageSummary: string;
}

export interface SimulateSendResponse {
  notificationId: string;
  status: NotificationStatus;
  channel: NotificationChannel;
  attemptCount: number;
  sentAt: string;
  message: string;
}

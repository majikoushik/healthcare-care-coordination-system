export enum FollowUpTaskType {
    General = 0,
    LabTest = 1,
    MedicationReview = 2,
    FollowUpAppointment = 3,
    LifestyleCounseling = 4,
    CarePlanReview = 5,
    PatientEducation = 6
}

export enum FollowUpTaskStatus {
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Overdue = 3,
    Cancelled = 4
}

export enum Priority {
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

export interface FollowUpTask {
    id: string;
    patientId: string;
    providerId?: string;
    carePlanId: string;
    clinicalInsightId?: string;
    appointmentId?: string;
    title: string;
    description: string;
    taskType: FollowUpTaskType;
    priority: Priority;
    status: FollowUpTaskStatus;
    dueDate: string;
    assignedTo: string;
    completedTimestamp?: string;
    completionNotes?: string;
    createdAt: string;
    updatedAt?: string;
}

export interface CreateFollowUpTaskRequest {
    patientId: string;
    providerId?: string;
    carePlanId: string;
    clinicalInsightId?: string;
    appointmentId?: string;
    title: string;
    description: string;
    taskType: FollowUpTaskType;
    priority: Priority;
    dueDate: string;
    assignedTo: string;
}

export interface UpdateFollowUpTaskStatusRequest {
    status: FollowUpTaskStatus;
    updatedBy: string;
    completionNotes?: string;
}

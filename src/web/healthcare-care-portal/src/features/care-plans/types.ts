export enum CarePlanStatus {
  Draft = 0,
  Active = 1,
  OnHold = 2,
  Completed = 3,
  Cancelled = 4
}

export enum GoalStatus {
  NotStarted = 0,
  InProgress = 1,
  Achieved = 2,
  NotAchieved = 3,
  Cancelled = 4
}

export enum TaskStatus {
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

export interface CarePlanGoal {
  goalId: string;
  description: string;
  targetDate?: string;
  status: GoalStatus;
  priority: Priority;
}

export interface CarePlanTask {
  taskId: string;
  taskTitle: string;
  taskDescription: string;
  dueDate?: string;
  status: TaskStatus;
  priority: Priority;
  assignedTo: string;
  completedTimestamp?: string;
}

export interface CarePlanDocument {
  id: string;
  patientId: string;
  providerId: string;
  relatedAppointmentId?: string;
  title: string;
  clinicalSummary: string;
  instructions: string;
  followUpDate?: string;
  status: CarePlanStatus;
  goals: CarePlanGoal[];
  tasks: CarePlanTask[];
  createdAt: string;
  updatedAt?: string;
}

export interface CreateCarePlanRequest {
  patientId: string;
  providerId: string;
  relatedAppointmentId?: string;
  title: string;
  clinicalSummary: string;
  instructions: string;
  followUpDate?: string;
  goals: any[];
  tasks: any[];
}

export interface UpdateCarePlanStatusRequest {
  status: CarePlanStatus;
  reason: string;
  updatedBy: string;
}

export interface AddCarePlanGoalRequest {
  description: string;
  targetDate?: string;
  priority: Priority;
}

export interface UpdateCarePlanGoalStatusRequest {
  status: GoalStatus;
  reason: string;
  updatedBy: string;
}

export interface AddCarePlanTaskRequest {
  taskTitle: string;
  taskDescription: string;
  dueDate?: string;
  priority: Priority;
  assignedTo: string;
}

export interface UpdateCarePlanTaskStatusRequest {
  status: TaskStatus;
  reason: string;
  updatedBy: string;
}

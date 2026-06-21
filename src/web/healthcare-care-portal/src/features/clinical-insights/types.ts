export enum AiProviderMode {
  Mock = 0,
  AzureAIReady = 1
}

export enum HumanReviewStatus {
  PendingReview = 0,
  Reviewed = 1,
  Approved = 2,
  Rejected = 3,
  RequiresClarification = 4
}

export enum EntityCategory {
  Unknown = 0,
  Symptom = 1,
  Condition = 2,
  Medication = 3,
  Test = 4,
  Procedure = 5,
  FollowUp = 6,
  Lifestyle = 7,
  RiskFactor = 8
}

export interface ExtractedEntity {
  entityId: string;
  text: string;
  category: EntityCategory;
  subcategory?: string;
  confidenceScore: number;
  offset?: number;
  length?: number;
  normalizedValue?: string;
}

export interface ClinicalNoteInsight {
  id: string;
  patientId: string;
  providerId: string;
  relatedCarePlanId?: string;
  relatedAppointmentId?: string;
  clinicalNoteText: string;
  extractedEntities: ExtractedEntity[];
  suggestedFollowUpTopics: string[];
  riskIndicators: string[];
  aiProviderName: string;
  aiProviderMode: AiProviderMode;
  humanReviewStatus: HumanReviewStatus;
  reviewedBy?: string;
  reviewedTimestamp?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface AnalyzeNoteRequest {
  patientId: string;
  providerId: string;
  relatedCarePlanId?: string;
  relatedAppointmentId?: string;
  clinicalNoteText: string;
}

export interface UpdateReviewStatusRequest {
  reviewStatus: HumanReviewStatus;
  reviewedBy: string;
  reviewNotes?: string;
}

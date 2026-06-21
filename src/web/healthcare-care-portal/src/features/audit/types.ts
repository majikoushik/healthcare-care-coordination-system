export interface AuditEvent {
  id: string;
  correlationId: string;
  eventType: string;
  entityType: string;
  entityId: string;
  patientId?: string;
  providerId?: string;
  actorType: string;
  actorId: string;
  action: string;
  outcome: string;
  summary: string;
  sourceService: string;
  severity: string;
  metadata?: Record<string, unknown>;
  createdAt: string;
}

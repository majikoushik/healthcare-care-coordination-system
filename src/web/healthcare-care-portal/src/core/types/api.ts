export type ApiResponse<T> = {
  data: T;
  correlationId: string;
  timestamp: string;
};

export type ServiceReadiness = {
  serviceName: string;
  domainBoundary: string;
  storageModel: string;
};

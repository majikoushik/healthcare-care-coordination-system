export enum Gender {
  Male = 0,
  Female = 1,
  Other = 2,
  PreferNotToSay = 3
}

export enum ConsentStatus {
  NotProvided = 0,
  Provided = 1,
  Withdrawn = 2
}

export interface Patient {
  id: string;
  fullName: string;
  dateOfBirth: string;
  gender: Gender;
  email: string;
  mobileNumber: string;
  address: string;
  emergencyContactName: string;
  emergencyContactNumber: string;
  consentStatus: ConsentStatus;
  createdAt: string;
  updatedAt?: string;
}

export interface RegisterPatientRequest {
  fullName: string;
  dateOfBirth: string;
  gender: Gender;
  email: string;
  mobileNumber: string;
  address: string;
  emergencyContactName: string;
  emergencyContactNumber: string;
  consentStatus: ConsentStatus;
}

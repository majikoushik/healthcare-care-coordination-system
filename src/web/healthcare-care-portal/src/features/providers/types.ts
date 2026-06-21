export enum Specialty {
  GeneralMedicine = 0,
  Cardiology = 1,
  Endocrinology = 2,
  Orthopedics = 3,
  Pediatrics = 4,
  Neurology = 5,
  Dermatology = 6
}

export enum AvailabilityStatus {
  Available = 0,
  Busy = 1,
  OnLeave = 2,
  Inactive = 3
}

export interface Provider {
  id: string;
  fullName: string;
  specialty: Specialty;
  email: string;
  mobileNumber: string;
  department: string;
  availabilityStatus: AvailabilityStatus;
  createdAt: string;
  updatedAt?: string;
}

export interface RegisterProviderRequest {
  fullName: string;
  specialty: Specialty;
  email: string;
  mobileNumber: string;
  department: string;
  availabilityStatus: AvailabilityStatus;
}

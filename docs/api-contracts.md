# API Contracts

## Patient.Api

Base path: `/api/v1/patients`

### `POST /` (Register Patient)
Registers a new synthetic patient profile.
- **Request Body**: `RegisterPatientRequest`
- **Response**: `201 Created` with `ApiResponse<PatientResponse>`
- **Errors**: `400 Bad Request` (Validation Problem Details)

### `GET /` (List Patients)
Fetches all registered synthetic patients.
- **Response**: `200 OK` with `ApiResponse<IEnumerable<PatientResponse>>`

### `GET /{id}` (Get Patient Details)
Fetches specific patient details by ID.
- **Response**: `200 OK` with `ApiResponse<PatientResponse>`
- **Errors**: `404 Not Found`

### `PUT /{id}` (Update Patient)
Updates specific patient profile.
- **Request Body**: `UpdatePatientRequest`
- **Response**: `200 OK` with `ApiResponse<PatientResponse>`
- **Errors**: `400 Bad Request`, `404 Not Found`

### `PATCH /{id}/consent-status` (Update Consent)
Updates the patient consent status.
- **Request Body**: `UpdateConsentStatusRequest`
- **Response**: `200 OK` with `ApiResponse<PatientResponse>`
- **Errors**: `400 Bad Request`, `404 Not Found`

## Provider.Api

Base path: `/api/v1/providers`

### `POST /` (Register Provider)
Registers a new synthetic provider profile.
- **Request Body**: `RegisterProviderRequest`
- **Response**: `201 Created` with `ApiResponse<ProviderResponse>`
- **Errors**: `400 Bad Request` (Validation Problem Details)

### `GET /` (List Providers)
Fetches all registered synthetic providers.
- **Response**: `200 OK` with `ApiResponse<IEnumerable<ProviderResponse>>`

### `GET /{id}` (Get Provider Details)
Fetches specific provider details by ID.
- **Response**: `200 OK` with `ApiResponse<ProviderResponse>`
- **Errors**: `404 Not Found`

### `GET /specialty/{specialty}` (Get Providers by Specialty)
Fetches all providers matching a given specialty.
- **Response**: `200 OK` with `ApiResponse<IEnumerable<ProviderResponse>>`

### `PUT /{id}` (Update Provider)
Updates a provider profile.
- **Request Body**: `UpdateProviderRequest`
- **Response**: `200 OK` with `ApiResponse<ProviderResponse>`
- **Errors**: `400 Bad Request`, `404 Not Found`

### `PATCH /{id}/availability-status` (Update Availability)
Updates the provider's availability status.
- **Request Body**: `UpdateAvailabilityStatusRequest`
- **Response**: `200 OK` with `ApiResponse<ProviderResponse>`
- **Errors**: `400 Bad Request`, `404 Not Found`

## Appointment.Api

Base path: `/api/v1/appointments`

### `POST /` (Schedule Appointment)
Schedules a new appointment.
- **Request Body**: `ScheduleAppointmentRequest`
- **Response**: `201 Created` with `ApiResponse<AppointmentResponse>`
- **Errors**: `400 Bad Request`

### `GET /` (List Appointments)
Fetches all appointments.
- **Response**: `200 OK` with `ApiResponse<IEnumerable<AppointmentResponse>>`

### `GET /{id}` (Get Appointment Details)
Fetches specific appointment details by ID.
- **Response**: `200 OK` with `ApiResponse<AppointmentResponse>`
- **Errors**: `404 Not Found`

### `GET /date/{date}` (Get Appointments by Date)
Fetches all appointments for a given date.
- **Response**: `200 OK` with `ApiResponse<IEnumerable<AppointmentResponse>>`

### `PUT /{id}` (Update Appointment)
Updates an appointment.
- **Request Body**: `UpdateAppointmentRequest`
- **Response**: `200 OK` with `ApiResponse<AppointmentResponse>`
- **Errors**: `400 Bad Request`, `404 Not Found`

### `PATCH /{id}/status` (Update Status)
Transitions the appointment status using strict business rules.
- **Request Body**: `UpdateAppointmentStatusRequest`
- **Response**: `200 OK` with `ApiResponse<AppointmentResponse>`
- **Errors**: `400 Bad Request` (Invalid Transition), `404 Not Found`

## CarePlan.Api

Base path: `/api/v1/care-plans`

### `POST /` (Create Care Plan)
Creates a new care plan document with optional embedded goals and tasks.
- **Request Body**: `CreateCarePlanRequest`
- **Response**: `201 Created` with `ApiResponse<CarePlanDocument>`

### `GET /` (List Care Plans)
Fetches all care plans.
- **Response**: `200 OK` with `ApiResponse<IEnumerable<CarePlanDocument>>`

### `GET /{id}` (Get Care Plan)
Fetches specific care plan document by ID.
- **Response**: `200 OK` with `ApiResponse<CarePlanDocument>`

### `PATCH /{id}/status` (Update Care Plan Status)
Transitions the status using strict business rules.
- **Request Body**: `UpdateCarePlanStatusRequest`
- **Response**: `200 OK` with `ApiResponse<CarePlanDocument>`
- **Errors**: `400 Bad Request` (Invalid Transition)

### `POST /{id}/goals` (Add Goal)
Appends a goal to the care plan document.
- **Request Body**: `AddCarePlanGoalRequest`
- **Response**: `200 OK` with `ApiResponse<CarePlanDocument>`

### `PATCH /{id}/goals/{goalId}/status` (Update Goal Status)
- **Request Body**: `UpdateCarePlanGoalStatusRequest`
- **Response**: `200 OK` with `ApiResponse<CarePlanDocument>`

### `POST /{id}/tasks` (Add Task)
Appends a task to the care plan document.
- **Request Body**: `AddCarePlanTaskRequest`
- **Response**: `200 OK` with `ApiResponse<CarePlanDocument>`

### `PATCH /{id}/tasks/{taskId}/status` (Update Task Status)
- **Request Body**: `UpdateCarePlanTaskStatusRequest`
- **Response**: `200 OK` with `ApiResponse<CarePlanDocument>`

## Common Response Format

```json
{
  "data": { ... },
  "correlationId": "b182062562ec4db2ba2a76f250953c89",
  "timestamp": "2026-06-21T10:00:00Z"
}
```

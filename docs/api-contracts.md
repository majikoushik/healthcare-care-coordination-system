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

## Common Response Format

```json
{
  "data": { ... },
  "correlationId": "b182062562ec4db2ba2a76f250953c89",
  "timestamp": "2026-06-21T10:00:00Z"
}
```

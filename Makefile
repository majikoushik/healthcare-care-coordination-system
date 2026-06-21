.PHONY: up down build-backend test-backend build-frontend test-frontend ci-test

up:
	docker compose up --build -d

down:
	docker compose down

build-backend:
	dotnet build src/services/Patient.Api/Patient.Api.csproj
	dotnet build src/services/Provider.Api/Provider.Api.csproj
	dotnet build src/services/Appointment.Api/Appointment.Api.csproj
	dotnet build src/services/CarePlan.Api/CarePlan.Api.csproj
	dotnet build src/services/ClinicalInsights.Api/ClinicalInsights.Api.csproj
	dotnet build src/services/Audit.Api/Audit.Api.csproj
	dotnet build src/services/Notification.Worker/Notification.Worker.csproj

test-backend:
	dotnet test tests/Patient.Api.Tests/Patient.Api.Tests.csproj
	dotnet test tests/Provider.Api.Tests/Provider.Api.Tests.csproj
	dotnet test tests/Appointment.Api.Tests/Appointment.Api.Tests.csproj
	dotnet test tests/CarePlan.Api.Tests/CarePlan.Api.Tests.csproj
	dotnet test tests/ClinicalInsights.Api.Tests/ClinicalInsights.Api.Tests.csproj
	dotnet test tests/Notification.Worker.Tests/Notification.Worker.Tests.csproj
	dotnet test tests/Audit.Api.Tests/Audit.Api.Tests.csproj
	dotnet test tests/Security.Tests/Security.Tests.csproj
	dotnet test tests/Observability.Tests/Observability.Tests.csproj

build-frontend:
	cd src/web/healthcare-care-portal && npm ci && npm run build

test-frontend:
	cd src/web/healthcare-care-portal && npm ci && npm test

ci-test: test-backend build-frontend

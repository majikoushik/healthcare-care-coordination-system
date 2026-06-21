.PHONY: up down build-backend test-backend build-frontend test-frontend ci-test

up:
	docker compose up --build -d

down:
	docker compose down

build-backend:
	dotnet build src/HealthcareCareCoordination.sln

test-backend:
	dotnet test tests/HealthcareCareCoordination.Tests.sln || dotnet test tests/

build-frontend:
	cd src/web/healthcare-care-portal && npm install && npm run build

test-frontend:
	cd src/web/healthcare-care-portal && npm install && npm test -- --run

ci-test: test-backend build-frontend

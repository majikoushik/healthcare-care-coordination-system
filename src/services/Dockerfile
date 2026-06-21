FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG SERVICE_PROJECT

# Copy solution and project files first to cache restore layer
COPY *.sln ./
COPY src/building-blocks/*/*.csproj ./
COPY src/services/*/*.csproj ./
# Ensure correct directory structure is recreated for restore
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done || true

# We will just copy everything for simplicity in a multi-project setup
# but it's better to explicitly copy and restore.
COPY . .
RUN dotnet restore $SERVICE_PROJECT
RUN dotnet publish $SERVICE_PROJECT -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ARG SERVICE_DLL
ENV SERVICE_DLL=$SERVICE_DLL
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Use a non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

COPY --from=build /app/publish .

CMD ["sh", "-c", "dotnet $SERVICE_DLL"]

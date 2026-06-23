FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
ARG SERVICE_PROJECT

COPY . .
RUN dotnet restore $SERVICE_PROJECT
RUN dotnet publish $SERVICE_PROJECT -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
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

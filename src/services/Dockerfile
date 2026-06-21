FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG SERVICE_PROJECT
COPY . .
RUN dotnet publish $SERVICE_PROJECT -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ARG SERVICE_DLL
ENV SERVICE_DLL=$SERVICE_DLL
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
CMD ["sh", "-c", "dotnet $SERVICE_DLL"]

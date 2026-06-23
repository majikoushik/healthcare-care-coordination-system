FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

ARG SERVICE_PROJECT

COPY . .

RUN dotnet restore "$SERVICE_PROJECT"

RUN dotnet publish "$SERVICE_PROJECT" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

ARG SERVICE_DLL
ENV SERVICE_DLL=$SERVICE_DLL

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

COPY --from=build /app/publish .

# Official .NET container images provide a built-in non-root user.
# Do not use adduser/useradd because minimal runtime images may not include them.
USER $APP_UID

CMD ["sh", "-c", "dotnet $SERVICE_DLL"]
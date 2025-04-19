# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG INVALIDATE_CACHE
WORKDIR /src

# Copy everything and restore/publish
COPY ./blazor ./blazor
WORKDIR /src/blazor
RUN dotnet publish -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
ARG INVALIDATE_CACHE
WORKDIR /app

# Copy published app and static files
COPY --from=build /app .
COPY --from=build /src/blazor/wwwroot ./wwwroot/

RUN chmod -R 755 ./wwwroot

ENTRYPOINT ["dotnet", "blazor.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG INVALIDATE_CACHE
WORKDIR /src

COPY ./blazor ./blazor
WORKDIR /src/blazor
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
ARG INVALIDATE_CACHE
WORKDIR /app

COPY --from=build /app .
COPY --from=build /src/blazor/wwwroot ./wwwroot/

RUN chmod -R 755 ./wwwroot

ENTRYPOINT ["dotnet", "blazor.dll"]

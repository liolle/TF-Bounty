# SDK containing the CLI
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS sdk
# Image optimized to run ASP.NET Core apps 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS aspnet

FROM sdk AS build
ARG INVALIDATE_CACHE
WORKDIR src
COPY ./api ./api
WORKDIR api
RUN dotnet publish -c release -o app api.csproj 

FROM aspnet AS final 
ARG INVALIDATE_CACHE
WORKDIR /
COPY --from=build ./src/api/app ./src/api/app
ENTRYPOINT ["dotnet", "./src/api/app/api.dll"]

#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

# First only copy project files to cache the restore without including all source files
COPY ["AspNetCoreTenant/AspNetCoreTenant.csproj", "AspNetCoreTenant/"]
COPY ["Mongo/Mongo.csproj", "Mongo/"]
COPY ["backend/Backend.csproj", "backend/"]
RUN dotnet restore "backend/Backend.csproj"

# Only copy these directories to save on recompilations
COPY ["AspNetCoreTenant/", "AspNetCoreTenant/"]
COPY ["Mongo/", "Mongo/"]
COPY ["backend/", "backend/"]

# Build
WORKDIR "/src/backend"
RUN dotnet build "Backend.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CorPool.BackEnd.dll"]
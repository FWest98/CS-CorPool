#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

# First only copy project files to cache the restore without including all source files
COPY ["AspNetCoreTenant/AspNetCoreTenant.csproj", "AspNetCoreTenant/"]
COPY ["Mongo/Mongo.csproj", "Mongo/"]
COPY ["Worker/Worker.csproj", "Worker/"]
RUN dotnet restore "Worker/Worker.csproj"

# Only copy these directories to save on recompilations
COPY ["AspNetCoreTenant/", "AspNetCoreTenant/"]
COPY ["Mongo/", "Mongo/"]
COPY ["Worker/", "Worker/"]

# Build
WORKDIR "/src/Worker"
RUN dotnet build "Worker.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Worker.dll"]
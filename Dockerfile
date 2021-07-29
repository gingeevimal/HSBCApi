#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://localhost:5000
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /
COPY ["Services/CAD/CAD.API/CAD.API.csproj", "Services/CAD/CAD.API/"]
COPY ["Services/CAD/CAD.Application/CAD.Application.csproj", "Services/CAD/CAD.Application/"]
COPY ["Services/CAD/CAD.Domain/CAD.Domain.csproj", "Services/CAD/CAD.Domain/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["Services/CAD/CAD.Infrastructure/CAD.Infrastructure.csproj", "Services/CAD/CAD.Infrastructure/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
RUN dotnet restore "Services/CAD/CAD.API/CAD.API.csproj"
COPY . .
WORKDIR "./Services/CAD/CAD.API"
RUN dotnet build "CAD.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CAD.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CAD.API.dll"]
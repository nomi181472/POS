#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 7080
EXPOSE 7081
EXPOSE 7082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Host/Auth/Auth.csproj", "Host/Auth/"]
#COPY ["Infrastructure/AuthInfra/AuthInfra.csproj", "Infrastructure/AuthInfra"]
RUN dotnet restore "./Host/Auth/Auth.csproj"
COPY . .
WORKDIR "/src/Host/Auth"
RUN dotnet build "./Auth.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Auth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.dll"]
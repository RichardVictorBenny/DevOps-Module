# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

# Copy everything into /src
COPY . .

# Restore NuGet packages for the solution
RUN dotnet restore "TDD.sln"

# Build the solution
RUN dotnet build "TDD.sln" -c $BUILD_CONFIGURATION -o /app/build

# Publish the web project
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "TDD.sln" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TDD.Web.dll"]
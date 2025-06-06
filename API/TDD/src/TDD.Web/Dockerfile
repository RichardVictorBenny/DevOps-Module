# # See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# # This stage is used when running from VS in fast mode (Default for Debug configuration)
# FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# USER $APP_UID
# WORKDIR /app
# EXPOSE 8080
# EXPOSE 8081


# # This stage is used to build the service project
# FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# ARG BUILD_CONFIGURATION=Development
# WORKDIR /src

# COPY ["TDD.Web.csproj", "src/TDD.Web/"]
# RUN dotnet restore "./src/TDD.Web/TDD.Web.csproj"
# COPY . .
# WORKDIR "/src/src/TDD.Web"
# RUN dotnet build "./TDD.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# # This stage is used to publish the service project to be copied to the final stage
# FROM build AS publish
# ARG BUILD_CONFIGURATION=Development
# RUN dotnet publish "./TDD.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# # This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "TDD.Web.dll"]

# # See https://aka.ms/customizecontainer

# Stage 1: Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# Only uncomment USER if you're defining $APP_UID elsewhere (otherwise remove this)
# USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src

# Copy solution and csproj files
COPY TDD.sln ./
COPY TDD.Web/TDD.Web.csproj TDD.Web/
COPY TDD.Shared/TDD.Shared.csproj TDD.Shared/

# Restore dependencies
RUN dotnet restore "TDD.sln"

# Copy everything else
COPY . .

# Build the main project
WORKDIR /src/TDD.Web
RUN dotnet build "TDD.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish project
FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "TDD.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TDD.Web.dll"]

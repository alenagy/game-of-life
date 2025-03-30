# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the solution file and the src folder from the root
# The context in docker-compose.yml will be the solution root
COPY *.sln ./
COPY src/ ./src/

# Restore dependencies for the API project (adjust the project path as needed)
RUN dotnet restore src/GameOfLife.API/GameOfLife.API.csproj

# Build and publish the API project
WORKDIR /app/src/GameOfLife.API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "GameOfLife.API.dll"]
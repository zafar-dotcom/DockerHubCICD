# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set working directory
WORKDIR /app

# Copy only the csproj file and restore
COPY ./mydockerapp/mydockerapp.csproj ./mydockerapp/

# ⬇️ Set the working directory to where the csproj file was copied
WORKDIR /app/mydockerapp

# Run restore inside the project directory
RUN dotnet restore

# Go back to /app and copy rest of the source
WORKDIR /app
COPY ./mydockerapp/. ./mydockerapp/

# Go back into the project folder to build
WORKDIR /app/mydockerapp
RUN dotnet publish -c Release -o /app/out

# STAGE 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "mydockerapp.dll"]

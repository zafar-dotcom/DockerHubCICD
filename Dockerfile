# Step 1: Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set working directory
WORKDIR /app

# Copy only the .csproj file first
COPY ./mydockerapp/mydockerapp.csproj ./mydockerapp/

# Set working directory to the project folder
WORKDIR /app/mydockerapp

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY ./mydockerapp/ .

# Publish the application
RUN dotnet publish -c Release -o /app/out

# Step 2: Run image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "mydockerapp.dll"]

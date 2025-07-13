# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Step 1: Set base workdir
WORKDIR /app

# Step 2: Copy only the project file first
COPY mydockerapp/mydockerapp.csproj ./mydockerapp/

# Step 3: Move into the project directory
WORKDIR /app/mydockerapp

# Step 4: Restore
RUN dotnet restore

# Step 5: Copy the full source code
COPY mydockerapp/. ./

# Step 6: Publish the app
RUN dotnet publish -c Release -o /app/out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose and run
EXPOSE 80
ENTRYPOINT ["dotnet", "mydockerapp.dll"]

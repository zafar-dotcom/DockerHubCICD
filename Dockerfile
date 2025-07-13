# Step 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

# Copy only the .csproj and restore
COPY mydockerapp/mydockerapp.csproj ./mydockerapp/
WORKDIR /app/mydockerapp
RUN dotnet restore

# Copy the rest of the source code and build
COPY mydockerapp/. ./
RUN dotnet publish -c Release -o /app/out

# Step 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "mydockerapp.dll"]

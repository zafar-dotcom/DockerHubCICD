# Use the .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore
COPY mydockerapp/mydockerapp.csproj ./mydockerapp/
RUN dotnet restore ./mydockerapp/mydockerapp.csproj

# Copy everything else and build
COPY mydockerapp/ ./mydockerapp/
WORKDIR /app/mydockerapp
RUN dotnet publish -c Release -o out

# Use the ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/mydockerapp/out ./

ENTRYPOINT ["dotnet", "mydockerapp.dll"]

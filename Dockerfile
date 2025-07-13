# Step 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./mydockerapp/ ./mydockerapp/
WORKDIR /src/mydockerapp
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# Step 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "mydockerapp.dll"]

# Step 1: Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# 👇 Update: copy csproj from subfolder
COPY ./mydockerapp/mydockerapp.csproj ./mydockerapp/
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/out

# Step 2: Run image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENV DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 80

ENTRYPOINT ["dotnet", "mydockerapp.dll"]

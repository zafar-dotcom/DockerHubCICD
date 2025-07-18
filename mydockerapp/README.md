
# 🚀 MyDockerApp - CI/CD with Docker Hub & Azure App Service

This repository demonstrates a complete CI/CD workflow for a .NET 8 web application using:

- **Docker** (build & containerize the app)
- **Docker Hub** (container registry)
- **GitHub Actions** (CI/CD automation)
- **Azure App Service (Linux)** (production deployment)

---

## 📦 Project Structure

```
mydockerapp/
  └── mydockerapp.csproj
Dockerfile
.github/
  └── workflows/
       └── docker-build.yml
README.md
```

---

## 🐳 Manual Docker Deployment (Local)

1. **Build Docker Image**  
   ```bash
   docker build -t mydockerapp .
   ```

2. **Run Docker Container**  
   ```bash
   docker run -d -p 8080:80 --name mydockerapp mydockerapp
   ```

3. **Tag & Push to Docker Hub**  
   ```bash
   docker tag mydockerapp zafar818/mydockerapp:latest
   docker login -u zafar818
   docker push zafar818/mydockerapp:latest
   ```

---

## ⚙️ CI/CD with GitHub Actions

### ✅ Features

- Auto-builds Docker image when changes are pushed to `main`
- Pushes to Docker Hub as:
  - `latest`
  - `commit hash`

### 📄 `.github/workflows/docker-build.yml`

```yaml
name: Docker Build and Push

on:
  push:
    branches:
      - main

jobs:
  build-and-push:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout source code
        uses: actions/checkout@v3

      - name: Set up Docker Buildx
        uses: docker/setup-buildx-action@v3

      - name: Log in to Docker Hub
        uses: docker/login-action@v3
        with:
          username: ${{ secrets.DOCKERHUB_USERNAME }}
          password: ${{ secrets.DOCKERHUB_TOKEN }}

      - name: Build and push Docker image
        uses: docker/build-push-action@v5
        with:
          context: .
          file: ./Dockerfile
          push: true
          tags: |
            zafar818/mydockerapp:latest
            zafar818/mydockerapp:${{ github.sha }}
```

### 🔐 GitHub Secrets Required

- `DOCKERHUB_USERNAME`: Your Docker Hub username
- `DOCKERHUB_TOKEN`: Docker Hub access token (from https://hub.docker.com/settings/security)

---

## ☁️ Azure App Service Deployment (Manual)

### 🔧 Prerequisites

- [Install Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- Login: `az login`

### 📜 Steps

```bash
az provider register --namespace Microsoft.Web

az group create --name MyDockerGroupCentral --location centralus

az appservice plan create   --name MyDockerPlan   --resource-group MyDockerGroupCentral   --is-linux   --sku F1   --location centralus

az webapp create   --resource-group MyDockerGroupCentral   --plan MyDockerPlan   --name DockerWebAppZee   --deployment-container-image-name zafar818/mydockerapp:latest
```

---

## 🔄 Enable Auto-Deploy from Docker Hub to Azure

```bash
az webapp config container set   --name DockerWebAppZee   --resource-group MyDockerGroupCentral   --docker-custom-image-name zafar818/mydockerapp:latest   --docker-registry-server-url https://index.docker.io

az webapp restart   --name DockerWebAppZee   --resource-group MyDockerGroupCentral
```

---

## 🌐 Access the Web App

To get your live web app URL:

```bash
az webapp show   --name DockerWebAppZee   --resource-group MyDockerGroupCentral   --query defaultHostName -o tsv
```

---

## ✅ Summary

| Component              | Tool/Service             |
|------------------------|--------------------------|
| Build & Containerize   | Docker                   |
| Image Hosting          | Docker Hub               |
| CI/CD Automation       | GitHub Actions           |
| Deployment Target      | Azure App Service (Linux)|
| Deployment Trigger     | Docker image update      |

---

## 📬 Need Help?

If you face issues with any step, feel free to open an [Issue](https://github.com/zafar818/mydockerapp/issues).

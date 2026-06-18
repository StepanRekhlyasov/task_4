# Сборка фронтенда (Vue + Vite)
ARG NODE_VERSION=24.12.0-alpine
FROM node:${NODE_VERSION} AS frontend-build
WORKDIR /app
COPY frontend/package.json frontend/package-lock.json ./
RUN npm ci
COPY frontend/ ./
RUN npm run build

# Сборка бэкенда (ASP.NET Core)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend-build
WORKDIR /app
COPY backend/*.csproj ./
RUN dotnet restore
COPY backend/ ./
RUN dotnet publish -c Release -o out

# Продакшен-образ: API + статика SPA на одном порту
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS production
WORKDIR /app
COPY --from=backend-build /app/out .
COPY --from=frontend-build /app/dist ./wwwroot
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "backend.dll"]

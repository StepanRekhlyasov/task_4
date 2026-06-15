# Stage 1: Build the C# Application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy remaining files and build release
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Run the compiled API
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose API port
EXPOSE 8080
ENTRYPOINT ["dotnet", "backend.dll"]
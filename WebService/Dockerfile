# Stage 1: Build the Blazor app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

COPY Directory.Packages.props ./
COPY WebService/WebService.csproj WebService/
RUN dotnet restore WebService/WebService.csproj

COPY WebService/ WebService/
WORKDIR /app/WebService
RUN dotnet publish -c $BUILD_CONFIGURATION -o out

# Stage 2: Serve with custom NGINX config
FROM nginx:alpine
COPY WebService/nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/WebService/out/wwwroot /usr/share/nginx/html

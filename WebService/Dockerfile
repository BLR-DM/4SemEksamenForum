# Stage 1: Build the Blazor app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Directory.Packages.props ./
COPY WebService/WebService.csproj WebService/
RUN dotnet restore WebService/WebService.csproj

COPY WebService/ WebService/
WORKDIR /src/WebService
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish

# Stage 2: Serve with custom NGINX config
FROM nginx:alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

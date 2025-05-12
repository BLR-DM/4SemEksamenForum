# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY WebService/*.csproj ./
RUN dotnet restore
COPY WebService/. ./
RUN dotnet publish -c Release -o out

# Stage 2: Serve with NGINX
FROM nginx:alpine
COPY --from=build /app/out/wwwroot /usr/share/nginx/html
# 4SemEksamenForum – Distributed Forum Platform

## Url https://blrforum.dk

![7ef14ae4-6b2c-41e9-95a0-7e104ce424e2](https://github.com/user-attachments/assets/59f1b805-7e82-4697-9238-be465b1c9c14)

## 🧐 About the Project

**4SemEksamenForum** is a scalable, microservice-based forum platform designed and implemented as part of our 4th semester project. It combines modern cloud-native technologies to enable asynchronous, event-driven communication and rich user interactions.

This system was developed with a strong focus on:
- Microservice communication patterns
- Scalable distributed architecture
- Content moderation and gamification
- Real-time interaction and data consistency

The project is built using .NET, Dapr, RabbitMQ, Redis, and deployed via Docker Compose. It is hosted on Oracle Cloud Infrastructure, where CI/CD pipelines automatically build and deploy updates.

---

## 🚀 Features

- **Microservice Architecture**: Services are independently developed and deployed.
- **Event-Driven Communication**: Uses Dapr Pub/Sub with RabbitMQ.
- **Dapr Sidecars**: Enables service discovery, pub/sub, state management, and observability.
- **Moderation Workflow**: AI-assisted content safety pipeline.
- **Point System**: Tracks user activity and gamifies engagement.
- **API Gateway**: Unified access through YARP reverse proxy with support for data aggregation.
- **Tracing & Observability**: Integrated Zipkin support.
- **Authentication & Authorization**: Secure user identity and access flow powered by Keycloak and JWT tokens.

---

## 🧹 Microservices

Each service runs in isolation with its own Dapr sidecar:

| Service                | Responsibility                               |
|------------------------|----------------------------------------------|
| `ContentService`       | Manages forum threads and posts              |
| `ContentSafetyService` | Performs moderation and content validation   |
| `SubscriptionService`  | Handles user subscriptions and events        |
| `VoteService`          | Manages upvotes/downvotes on content         |
| `PointService`         | Tracks and calculates user activity points   |
| `NotificationService`  | Collects notification data for every user    |
| `Gateway`              | Acts as the public API facade                |

---

## ⚙️ Infrastructure Overview

- **Message Broker**: RabbitMQ for reliable message delivery.
- **State Store**: Redis used with Dapr for persistence.
- **Tracing**: Zipkin collects and visualizes traces across services.
- **Service Invocation & Pub/Sub**: Combined strategy based on Dapr recommendations.
- **Cloud Hosting**: Deployed on Oracle Cloud Infrastructure with CI/CD.
- **Ingress & HTTPS:**: Handled by NGINX reverse proxy with certificates managed via Certbot.
- **Authentication**: OpenID Connect-based flow using Keycloak and JWT token validation.

---

## 📦 Running the Project

```bash
# Docker & Docker Compose
docker compose up -d
```
This command will start all backend services and infrastructure containers.

> **Note:** The Blazor WebAssembly frontend must be started separately using Visual Studio or another IDE (Select webservice as startup and run http). Ensure the Gateway is running so that the WebAssembly client can communicate with backend services.

Configuration files:
- `docker-compose.yml` – Service orchestration
- `docker-compose.override.yml` – Local dev ports & volumes
- `components/pubsub.yml` – Pub/Sub config (RabbitMQ)
- `components/statestore.yml` – Redis state management
- `dapr/config.yaml` – Tracing setup (Zipkin)

---

## 📚 Key Learnings

- Designing loosely coupled services using pub/sub
- Implementing moderation with external and custom AI pipelines
- Applying patterns like Backend-for-Frontend (BFF) and Service Invocation
- Exploring trade-offs between synchronous REST and asynchronous messaging
- Using Dapr building blocks to simplify distributed system complexity
- Deploying and managing services on a real-world cloud provider (OCI)
- Securing access using JWT and OpenID Connect (Keycloak)
- Managing HTTPS with automated TLS provisioning via Certbot and NGINX

---

## 🧑‍💼 Authors

Developed by: [@BLR-DM](https://github.com/BLR-DM)

# eCommerce Microservices Platform

A cloud-native eCommerce application built with **C#**, **ASP.NET Core**, and deployed to **Microsoft Azure** with full CI/CD pipelines via **Azure DevOps**.

## Architecture Overview

The platform is composed of three independently deployable microservices, each with its own data store, communicating asynchronously via RabbitMQ and routed through a dual-gateway setup.

```
External Client
      │
      ▼
Azure API Management  ←── External gateway
      │
      ▼
   Ocelot API Gateway  ←── Internal routing
      │
  ┌───┼───────────┐
  ▼   ▼           ▼
Users  Products  Orders
(PG)  (MySQL)  (MongoDB)
```

### Services

| Service | Port | Database | Description |
|---|---|---|---|
| UsersService | 9090 | PostgreSQL | User registration & authentication |
| ProductsService | 8080 | MySQL | Product catalog management |
| OrdersService | 8080 | MongoDB | Order processing & history |
| ApiGateway (Ocelot) | 8080 | — | Internal API routing |

### Infrastructure Components

- **RabbitMQ** – Async messaging between services (products.exchange, users.exchange)
- **Redis** – Distributed caching for the Orders service
- **Azure API Management** – External API gateway with policy enforcement
- **Azure Kubernetes Service (AKS)** – Container orchestration
- **Azure Container Registry** – Private image registry (`aacisterecommerceregistry.azurecr.io`)

## CI/CD Pipelines

Each microservice has a dedicated Azure DevOps pipeline that runs across **5 environments**: `dev → qa → uat → staging → prod`.

Each pipeline includes:
1. **Build** – Compile and containerize the service
2. **Test** – Run unit tests (xUnit)
3. **Deploy** – Push to AKS via Kubernetes manifests

## API Endpoints

All external requests route through Azure API Management at `https://aacister-ecommerce-api.azure-api.net/gateway`.

### Products

| Method | Endpoint | Description |
|---|---|---|
| GET | `/gateway/products` | List all products |
| GET | `/gateway/products/search/product-id/:ProductID` | Get product by ID |
| POST | `/gateway/products` | Create a product |
| PUT | `/gateway/products` | Update a product |
| DELETE | `/gateway/products/:ProductID` | Delete a product |

**POST / PUT Request Body:**
```json
{
  "productName": "string",
  "category": 3,
  "unitPrice": 0.00,
  "quantityInStock": 0
}
```

### Orders

| Method | Endpoint | Description |
|---|---|---|
| GET | `/gateway/orders` | List all orders |
| GET | `/gateway/Orders/search/orderid/:orderID` | Get order by ID |

## Project Structure

```
eCommerce/
├── ApiGateway/               # Ocelot internal gateway
├── UsersService/
│   ├── UsersService.API/
│   ├── UsersService.Core/
│   ├── UsersService.Infrastructure/
│   └── UsersUnitTests/
├── ProductsService/
│   ├── ProductsService.API/
│   ├── ProductsService.Business/
│   ├── ProductsService.DataAccess/
│   └── ProductsUnitTests/
├── OrdersService/
│   ├── OrdersService.API/
│   ├── OrdersService.Business/
│   ├── OrdersService.DataAccess/
│   └── OrdersUnitTests/
├── aks/                      # Kubernetes deployment manifests
├── mongodb-init/             # MongoDB seed data
├── mysql-init/               # MySQL schema & seed data
└── postgres-init/            # PostgreSQL schema & seed data
```

## Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Azure CLI (for cloud deployments)

### Run Locally with Docker Compose

```bash
docker-compose up --build
```

This starts all microservices, databases, RabbitMQ, and the API gateway. The Ocelot gateway will be accessible at `http://localhost:8080`.

### Deploy to Azure Kubernetes Service

Apply all manifests in the `aks/` directory:

```bash
kubectl apply -f aks/
```

Manifests include deployments and services for all microservices, databases, RabbitMQ, and Redis within the `ecommerce-namespace` namespace.

## Seed Data

The databases are pre-seeded for local development:

- **PostgreSQL** – 2 sample users (John Doe, Jane Smith)
- **MySQL** – 12 products across Electronics, Furniture, Accessories, and HomeAppliances categories
- **MongoDB** – 10 sample orders spanning both users

## Azure Resources

| Resource | Value |
|---|---|
| Azure DevOps Org | https://dev.azure.com/aacister/eCommerce |
| Azure Portal | https://portal.azure.com/#@aacistergmail.onmicrosoft.com |
| Container Registry | aacisterecommerceregistry.azurecr.io |
| API Management Gateway | https://aacister-ecommerce-api.azure-api.net |

## OpenAPI Documentation

Swagger JSON files are located within each microservice's respective directory.

## Fault Tolerance

All services use the **Polly** library to implement circuit breaker patterns for resilient inter-service communication.

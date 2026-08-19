# EloÍs Store

EloÍs Store is a simple e-commerce backend for a women's fashion store.

This monorepo uses .NET 10 with a direct structure based on controllers, services, models, repositories, and infrastructure.

## Stack

- C# / .NET 10
- ASP.NET Core Web API
- OpenAPI
- Scalar API Reference
- Entity Framework Core
- PostgreSQL via Docker Compose

## Run

```powershell
docker compose -f deploy/docker-compose.yml up --build
```

The store frontend will be available at:

```text
http://localhost:5173
```

Scalar API Reference will be available at:

```text
http://localhost:5001/scalar
```

The OpenAPI document is available at `/openapi/v1.json`.
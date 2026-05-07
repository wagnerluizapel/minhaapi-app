# People Management API — .NET 8

A production‑ready REST API built with .NET 8 Minimal APIs, designed with security, clean architecture, observability, audit logging, and cloud‑native deployment in mind.

---

## Technologies Used

.NET 8 Minimal APIs

Entity Framework Core 8

PostgreSQL (Neon)

Docker + Docker Compose

Swagger / OpenAPI 3

Serilog (structured logging)

JWT Authentication

Rate Limiting

Audit Logs

CORS Hardening

Input Sanitization

Security Headers

HttpClient + External Integration (OpenWeather API)

---

## Architecture Overview

/MinhaApi
 ├── Data/                → DbContext, Migrations, Seeds
 ├── Extensions/          → Endpoints (People, Auth, External)
 ├── Middleware/          → Audit, Security, Sanitization, RequestId
 ├── Models/              → Entities and DTOs
 ├── Repositories/        → Repository Pattern
 ├── Services/            → Business logic
 ├── Services/External/   → OpenWeather integration
 ├── Validators/          → FluentValidation
 ├── Program.cs           → Full application pipeline
 └── docker-compose.yml   → Local infrastructure

This structure ensures the project is:

easy to navigate

easy to maintain

easy to scale

easy to explain in interviews


# Request Pipeline:

Client → Kestrel → ForwardedHeaders
       → Serilog Logging
       → RequestId Middleware
       → Audit Middleware
       → Rate Limiting
       → CORS
       → Security Headers
       → Input Sanitization
       → Authentication (JWT)
       → Authorization
       → Endpoint (People, Auth, External)
       → Response
Each layer provides a specific production‑grade responsibility.

Cada etapa tem um propósito claro:
ForwardedHeaders → Suporte a proxies e Azure
Serilog → Logs estruturados
RequestId → Correlação entre logs
AuditMiddleware → Auditoria corporativa
RateLimiter → Proteção contra brute force
CORS → Segurança de origem
SecurityHeaders → Proteção OWASP
Sanitização → Remove scripts e payloads maliciosos
JWT → Autenticação forte
Authorization → Controle de acesso
Endpoints → Execução da lógica de negócio

# App layer:

🔹 Controllers? Não. Minimal APIs.
A API usa Minimal APIs, que deixam o código:

mais rápido
mais simples
mais direto
com menos boilerplate

🔹 Services
Contêm a lógica de negócio.
Exemplo: PessoaService, AuditService, OpenWeatherService.

🔹 Repositories
Isolam o acesso ao banco.
Exemplo: PessoaRepository.

🔹 DTOs
Evita expor entidades diretamente.
Exemplo: PessoaCreateDto, LoginRequest.

🔹 Validators
Validação profissional com FluentValidation.

🔹 Middleware
Onde mora a mágica de segurança e auditoria.


# Security:

| Layer | Protection |
| --- | --- |
| JWT | Strong authentication |
| Rate Limiting | Anti‑brute‑force |
| Audit Logs | Full traceability |
| Input Sanitization | Anti‑XSS |
| Security Headers | OWASP hardening |
| CORS | Cross‑origin protection |
| Payload Limits | Anti‑DoS |
| DTO Validation | Data integrity |
| Structured Logging | Observability |
| Azure App Service | Secure deployment |

This is not “basic security” — it’s enterprise‑grade security.


# External Integration — OpenWeather API:

Endpoint:

GET /externo/clima/{cidade}

Example response:

{
  "success": true,
  "data": {
    "cidade": "Melbourne",
    "temperatura": 15.6,
    "umidade": 82,
    "descricao": "fog"
  }
}


# Swagger Screenshots:

(Images embedded via /imagens/... — filenames listed at the end)

🔹 Swagger Overview
![Swagger Overview](./imagens/swagger-overview.png)

🔹 Login Endpoint
![Swagger Login](./imagens/swagger-login.png)

🔹 JWT Authentication
![Swagger Auth](./imagens/swagger-auth.png)

🔹 JWT Authorization
![Swagger Authorization](./imagens/swagger-authorization.png)

🔹 Returned Token
![Swagger Token](./imagens/swagger-token.png)

🔹 People Endpoints
![Swagger People](./imagens/swagger-pessoas.png)

🔹 GET /pessoas Execution
![Swagger GET Pessoas](./imagens/swagger-get-pessoas.png)

🔹 External Integration Endpoint
![Swagger Clima](./imagens/swagger-clima.png)

🔹 GET /externo/clima/{cidade} Execution
![Swagger GET Clima](./imagens/swagger-get-clima.png)


# Azure Deployment (App Service + Docker)

🔹 App Service Overview
![Azure Overview](./imagens/azure-overview.png)

🔹 Deployment Center (Container)
![Azure Deployment](./imagens/azure-deployment.png)

🔹 Log Stream — Container Startup
![Azure Log Startup](./imagens/azure-logstream-startup.png)

🔹 Log Stream — EF Migrations
![Azure Log Migrations](./imagens/azure-logstream-migrations.png)

🔹 App Service Plan
![Azure Plan](./imagens/azure-appservice-plan.png)

🔹 Healthcheck /health
![Azure Healthcheck](./imagens/azure-healthcheck.png)

🔹 Healthcheck Full /health
![Azure Healthcheck Full](./imagens/azure-healthcheck-full.png)


# Database (Neon PostgreSQL)

🔹 People Table
![Neon Pessoas](./imagens/neon-pessoas.png)

🔹 Users Table
![Neon Usuarios](./imagens/neon-usuarios.png)

🔹 Audit Logs
![Neon AuditLogs](./imagens/neon-auditlogs.png)

🔹 Audit Logs (Detailed)
![Neon AuditLogs Detailed](./imagens/neon-auditlogs-detalhado.png)


---

## Running Locally

1. Clone the repository
git clone https://github.com/wagnerluizapel/minhaapi-app.git

2. Run with Docker
docker compose up --build

3. Access Swagger
http://localhost:8080/swagger


---

## Conclusion:

This project demonstrates:

Clean architecture

Enterprise‑grade security

Full observability

Audit logging

External API integration

Real Azure deployment

Production PostgreSQL database

A complete, modern backend — perfect for portfolio and interviews.


---

## Expected Files in /imagens:

swagger-auth.png
swagger-authorization.png
swagger-clima.png
swagger-get-clima.png
swagger-pessoas.png
swagger-get-pessoas.png
swagger-overview.png
swagger-login.png
swagger-token.png

azure-overview.png
azure-deployment.png
azure-logstream-startup.png
azure-logstream-migrations.png
azure-appservice-plan.png
azure-healthcheck.png
azure-healthcheck-full.png

neon-auditlogs.png
neon-auditlogs-detalhado.png
neon-pessoas.png
neon-usuarios.png







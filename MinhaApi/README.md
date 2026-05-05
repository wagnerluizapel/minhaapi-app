# 🚀 Minha API — Gestão de Pessoas  
API REST construída em **.NET 8**, com foco em **segurança, arquitetura limpa, observabilidade, auditoria e boas práticas corporativas**.

Este projeto foi desenvolvido como parte de um roadmap profissional cobrindo desde fundamentos até práticas avançadas de produção.

---

## 📌 Tecnologias Utilizadas

- **.NET 8 Minimal APIs**
- **Entity Framework Core 8**
- **PostgreSQL**
- **Docker + Docker Compose**
- **Swagger / OpenAPI 3**
- **Serilog (logs estruturados)**
- **JWT Authentication**
- **Rate Limiting**
- **Audit Logs**
- **CORS Hardening**
- **Input Sanitization**
- **Security Headers**
- **HttpClient + Integração Externa (OpenWeather)**

---

## 🧱 Arquitetura

A API segue uma estrutura de pastas (arquitetura modular):

/MinhaApi
 ├── Data/                → DbContext, Migrations, configuração do EF Core
 ├── Extensions/          → Endpoints organizados por domínio (Pessoa, Auth, Externo)
 ├── Middleware/          → Auditoria, Segurança, Sanitização, RequestId
 ├── Models/              → Entities e DTOs
 ├── Repositories/        → Acesso a dados (Repository Pattern)
 ├── Services/            → Regras de negócio
 ├── Services/External/   → Integração com API externa (OpenWeather)
 ├── Validators/          → Validações com FluentValidation
 ├── Program.cs           → Pipeline completo da aplicação
 └── docker-compose.yml   → Infraestrutura Docker

Essa organização deixa o projeto:
fácil de navegar
fácil de manter
fácil de escalar
fácil de explicar em entrevistas

# Fluxo de requisicao:

Pipeline completo: 

Cliente → Kestrel → ForwardedHeaders
        → Serilog Logging
        → RequestId Middleware
        → Audit Middleware
        → Rate Limiting
        → CORS
        → Security Headers
        → Input Sanitization
        → Authentication (JWT)
        → Authorization
        → Endpoint (Pessoa, Auth, Externo)
        → Resposta

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


# Segurança Integrada na Arquitetura

A API aplica segurança em múltiplas camadas:

JWT para autenticação
Rate Limiting para proteção contra abuso
Audit Logs para rastreabilidade
Sanitização para evitar XSS
Security Headers para endurecimento OWASP
CORS restrito para evitar ataques cross-origin

Isso não é “segurança básica”.
É segurança corporativa.


# Integracao externa:

OpenWeather:

Services/External/OpenWeatherService.cs
Extensions/ExternalEndpoints.cs

Com:

HttpClient configurado

Timeout

Tratamento de erro

Logs estruturados

DTOs específicos


# Banco de Dados e Migrations:

A API usa:

EF Core 8

PostgreSQL

Migrations automáticas

Seeds automáticos

A tabela de auditoria (AuditLogs) é criada e mantida automaticamente.


# Infra estrutura com Docker:

O Dockerfile e o docker-compose:

constroem a imagem

expõem a porta 8080

configuram o Kestrel

conectam ao PostgreSQL

permitem deploy no Azure App Service

# Deploy no Azure:

A arquitetura foi pensada para rodar em:

Azure App Service (Linux)

Container Docker

Connection strings via App Settings

Healthcheck configurado

Logging integrado


---

## 🔐 Segurança

A API implementa um conjunto robusto de práticas OWASP:

### ✔ Autenticação JWT  
- Tokens assinados com chave secreta  
- Expiração configurada  
- Claims de email e role  

### ✔ Rate Limiting  
- Política global  
- Política específica para `/auth/login`  
- Proteção contra brute force  

### ✔ Audit Logs  
Registra automaticamente:
- usuário  
- endpoint  
- método  
- payload  
- timestamp  
- requestId  

### ✔ Sanitização de Entrada  
Remove:
- scripts  
- tags HTML  
- caracteres perigosos  

### ✔ Security Headers  
Inclui:
- X-Content-Type-Options  
- X-Frame-Options  
- Referrer-Policy  
- Content-Security-Policy (básica)  

### ✔ CORS Hardening  
Somente origens confiáveis.

---

## 🌦 Integração Externa — OpenWeather

Endpoint:

GET /externo/clima/{cidade}

Retorna:

```json
{
  "success": true,
  "data": {
    "temperatura": 28.5,
    "descricao": "céu limpo",
    "umidade": 60,
    "vento": 3.4
  }
}

Endpoints Principais:
Pessoas:
GET /pessoas
POST /pessoas
PUT /pessoas/{id}
DELETE /pessoas/{id}

Autenticação:
POST /auth/login

Integração Externa:
GET /externo/clima/{cidade}

Healthchecks:
GET /health
GET /ready

----------

⭐ Resumo da Segurança
A API implementa segurança em múltiplas camadas:

Camada	Proteção
JWT	Autenticação forte
Rate Limiting# ⭐ Resumo da Segurança
A API implementa segurança em múltiplas camadas:

Camada	Proteção
JWT	Autenticação forte
Rate Limiting	Anti-brute-force	Anti-brute-force
Audit Logs	Rastreabilidade
Sanitização	Anti-XSS
Security
Audit Logs	Rastreabilidade
Sanitização	Anti-XSS
Security Headers	OWASP
CORS	Anti-cross-origin
Payload Limits	Anti-DoS
DTO Validation	Integridade de dados
Logs Estruturados Headers	OWASP
CORS	Anti-cross-origin
Payload Limits	Anti-DoS
DTO Validation	Integridade de dados
Logs Estruturados	Observabilidade
Azure App Service	Deploy seguro


-----------------

☁️ Deploy Explicado (Azure + Docker)
O deploy desta API foi projetado para ser simples, seguro e totalmente automatizável.
A aplicação roda dentro de um container Docker, publicado no Azure App Service (Linux), com configuração de ambiente, healthchecks e logs integrados.

Abaixo está a explicação completa do processo.

🐳 1. Construção da Imagem Docker
A API utiliza um Dockerfile multi-stage, garantindo:

build limpo

imagem final leve

separação entre build e runtime

segurança (não expõe SDK no ambiente final)

Fluxo do Dockerfile:
Stage build

restaura pacotes

compila

publica em /app/publish

Stage final

usa imagem base aspnet:8.0

copia apenas os arquivos publicados

expõe porta 8080

inicia a aplicação

Subir localmente:
Code
docker compose up --build
🌐 2. Configuração do Azure App Service (Linux)
O deploy foi feito usando:

Azure App Service (Linux)

Container Docker

Plano Basic ou Standard

Registro de container público (Docker Hub)

Configurações essenciais:
Configuração	Valor
Porta exposta	8080
Healthcheck	/health
Startup Command	vazio (App Service detecta automaticamente)
App Settings	ConnectionStrings, JWT_SECRET, API keys
🔑 3. Variáveis de Ambiente (App Settings)
No Azure, todas as configurações sensíveis ficam em:

Code
Configuration → Application Settings
Incluindo:

ConnectionStrings__DefaultConnection

JWT__Secret

OpenWeather__ApiKey

ASPNETCORE_ENVIRONMENT=Production

Essas variáveis substituem automaticamente o appsettings.json.

❤️ 4. Healthcheck configurado
O App Service usa o endpoint:

Code
/health
para verificar se o container está saudável.

Isso garante:

restart automático se travar

zero downtime

monitoramento contínuo

📊 5. Logs no Azure
A API envia logs estruturados via Serilog para:

Console (capturado pelo Azure)

Log Stream

Application Insights (opcional)

Isso permite:

rastrear erros

monitorar performance

investigar auditorias

🔄 6. Fluxo completo do deploy
O processo de deploy funciona assim:

Code
Código → Docker Build → Docker Hub → Azure App Service → Container Start → Healthcheck → OK
Passo a passo:
Build da imagem:

Code
docker build -t wagnerapel/minhaapi:latest .
Push para o Docker Hub:

Code
docker push wagnerapel/minhaapi:latest
Azure App Service puxa a imagem automaticamente:

Code
DOCKER|wagnerapel/minhaapi:latest
Azure inicia o container

Healthcheck valida

API fica disponível publicamente

🔐 7. Segurança no Deploy
O deploy segue boas práticas:

Sem portas desnecessárias expostas

Sem secrets no código

Sem SDK no container final

HTTPS forçado pelo Azure

CORS restrito

Rate limiting ativo

Auditoria ativa

Sanitização ativa

🚀 8. Resultado Final
O deploy no Azure garante:

alta disponibilidade

escalabilidade

logs centralizados

segurança corporativa

facilidade de manutenção

custo baixo

------------

📸 Prints do Swagger
Abaixo estão alguns prints da documentação interativa da API, gerada automaticamente com Swagger + OpenAPI 3.

🔹 Tela inicial do Swagger
(inserir imagem aqui)


🔹 Endpoint de Login
(inserir imagem aqui)

🔹 Endpoints de Pessoas
(inserir imagem aqui)

🔹 Integração Externa — OpenWeather
(inserir imagem aqui)

🔹 Autorização JWT funcionando
(inserir imagem aqui)


# OAuth2 Provider

## 1) Visao geral do projeto
Authorization Server interno para microsservicos, responsavel por emitir tokens JWT para consumo entre servicos e por administracao de clients/usuarios via API.

## 2) Linguagens e tecnologias
- Backend: C# (.NET 8)
- Frontend: React + TypeScript + Vite
- Banco: MongoDB
- Autenticacao: OAuth2 + JWT (RS256 + JWKS)

## 3) Frameworks e bibliotecas principais
- ASP.NET Core
- MongoDB.Driver
- System.IdentityModel.Tokens.Jwt
- BCrypt.Net
- React Router
- Vite

## 4) Metodologia / Arquitetura
- Clean Architecture
- Separacao em Domain / Application / Infrastructure / Presentation / Api / Web
- Frontend desacoplado via API
- JWT assinado com RSA (RS256)
- Testes de integracao

## 5) Segurança
- JWT assinado com RSA (RS256).
- Chaves privadas nunca são versionadas no repositório.
- Chaves públicas expostas via JWKS para validação externa (`/.well-known/jwks.json`).
- Secrets e credenciais configurados exclusivamente via variáveis de ambiente.
- Senhas armazenadas utilizando BCrypt.
- Endpoints administrativos protegidos por policy (`scope=admin`).
- Refresh tokens com rotação automática.
- PKCE implementado no Authorization Code Flow.
- Separação entre tokens administrativos e tokens OAuth.

## 6) Design Patterns
- Clean Architecture (Domain / Application / Infrastructure / Presentation / Api / Web).
- Dependency Injection (ASP.NET Core container).
- Repository Pattern (persistência MongoDB).
- Provider / Factory (JWT, RSA, MongoDB).
- Strategy Pattern (OAuth flows: Client Credentials, Authorization Code + PKCE, Refresh Token).
- Singleton (RsaKeyProvider).
- Adapter Pattern (mapeamento Domain ⇄ MongoDB Documents).

## 7) Estrutura da solucao
- OAuth2.Api
- OAuth2.Application
- OAuth2.Domain
- OAuth2.Infrastructure
- OAuth2.Presentation
- OAuth2.Web

## 8) Como rodar localmente
Backend:
- Mongo rodando em `mongodb://localhost:27017`
- `dotnet run` (projeto `OAuth2.Api`)

Frontend:
- `npm install` (em `OAuth2.Web`)
- `npm run dev`

## 9) Variaveis de ambiente
- `ADMIN_EMAIL`
- `ADMIN_PASSWORD`
- `JWT_PRIVATE_KEY_PATH`
- `JWT_PUBLIC_KEY_PATH`

## 10) JWT RS256 + JWKS
Tokens sao assinados com RSA (RS256). A chave publica e exposta via JWKS para validacao por APIs internas.
- Endpoint: `/.well-known/jwks.json`

## 11) Flows suportados
- Client Credentials
- Authorization Code + PKCE
- Refresh Token

## 12) Testes
- `dotnet test`

## 13) Build frontend
O build do Vite gera os arquivos estaticos em `OAuth2.Web/wwwroot`.

## 14) Partner / autoria
Partner:
- Claudio Carvalho

## Documentacao
- [Manual do Provedor OAuth2](docs/Manual_OAuth2Provider.md) — guia operacional para admins e devs internos
- [Arquitetura](docs/Architecture.md) — visao geral da arquitetura e padroes do sistema
- [API Endpoints](docs/ApiEndpoints.md) — lista completa de endpoints e exemplos
- [Uso com Postman](docs/PostmanUsage.md) — instrucoes para testar os endpoints via Postman
- [Exemplos de API](docs/ApiUsageExamples.md) — exemplos curl para os principais endpoints
- [Guia rapido de Postman](docs/PostmanQuickStart.md) — configuracao rapida de ambiente para testes
- [Troubleshooting](docs/Troubleshooting.md) — problemas comuns e solucoes

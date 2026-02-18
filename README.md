# OAuth2 Provider - Authorization Server em .NET 8

Authorization Server OAuth2 para ambiente corporativo interno, construído com .NET 8, Clean Architecture e painel administrativo em React. O projeto implementa emissão e validação de tokens JWT RS256, Authorization Code com PKCE, Refresh Token com rotação e publicação de chave pública via JWKS.

## Contexto real do projeto

Este repositório simula um cenário real de microsserviços internos com autenticação centralizada, sem dependência de Identity Providers externos. O objetivo é oferecer controle de segurança, governança de acesso e administração de clients/usuários em uma arquitetura desacoplada e testável.

## Problemas resolvidos

- Emissão segura de tokens JWT assinados com RSA (RS256)
- Suporte a OAuth2 moderno:
  - Client Credentials
  - Authorization Code + PKCE (S256)
  - Refresh Token com rotação
- Segurança operacional com JWKS, PKCE obrigatório e expiração de tokens
- Administração centralizada de clients e usuários via UI

## Arquitetura

- **Backend:** C# (.NET 8) + ASP.NET Core
- **Persistência:** MongoDB (`MongoDB.Driver`)
- **Frontend:** React + TypeScript + Vite (desacoplado via API)
- **Organização:** Clean Architecture em projetos separados

### Camadas

- `OAuth2.Domain`
- `OAuth2.Application`
- `OAuth2.Infrastructure`
- `OAuth2.Presentation`
- `OAuth2.Api`
- `OAuth2.Web`

### Diagrama simplificado

```text
OAuth2.Web (React)
        |
        v
OAuth2.Api (Host ASP.NET Core)
        |
        v
OAuth2.Presentation -> OAuth2.Application -> OAuth2.Domain
        |
        v
OAuth2.Infrastructure (MongoDB, JWT, RSA, Stores)
```

### Padrões aplicados

- Dependency Injection
- Repository
- Provider/Factory
- Strategy (flows OAuth2)
- Singleton (`RsaKeyProvider`)
- Adapter (mapeamento Domain <-> Mongo Documents)

## Decisões Arquiteturais Importantes

- A solução adota Clean Architecture para isolar domínio, casos de uso e detalhes de infraestrutura.
- A autenticação usa JWT stateless com assinatura RS256 e distribuição de chave pública via JWKS.
- O Authorization Code exige PKCE (`S256`) para reduzir risco de interceptação e uso indevido do code.
- O MongoDB foi escolhido como persistência principal com suporte natural a documentos e TTL para tokens.
- A API em .NET e a UI em React são projetos separados, integrados exclusivamente por contratos HTTP.

## Segurança

- JWT assinado com **RS256**
- Chaves públicas expostas em **JWKS**
- PKCE obrigatório no Authorization Code (`S256`)
- Refresh Token com rotação (uso único)
- Senhas e secrets com **BCrypt**
- Endpoints administrativos protegidos por policy (`scope=admin`)

## OAuth2 Flows suportados

- Client Credentials
- Authorization Code + PKCE (S256)
- Refresh Token

## Como executar localmente

### Backend

- MongoDB em `mongodb://localhost:27017`
- Executar:
  - `dotnet run` no projeto `OAuth2.Api`

### Frontend

- No diretório `OAuth2.Web`:
  - `npm install`
  - `npm run dev`

### Variáveis de ambiente relevantes

- `ADMIN_EMAIL`
- `ADMIN_PASSWORD`
- `JWT_PRIVATE_KEY_PATH`
- `JWT_PUBLIC_KEY_PATH`

## JWT RS256 + JWKS

O servidor assina tokens com chave privada RSA. A validação externa usa a chave pública via:

- `/.well-known/jwks.json`

## Testes

O projeto possui testes de integração cobrindo os principais fluxos de autenticação/autorização e cenários administrativos.

- Execução: `dotnet test`

## Build do frontend

O build do Vite gera artefatos estáticos em:

- `OAuth2.Web/wwwroot`

## Documentação

- [Manual do Provedor OAuth2](docs/Manual_OAuth2Provider.md) - guia operacional para admins e devs internos
- [Arquitetura](docs/Architecture.md) - visão geral da arquitetura e padrões do sistema
- [API Endpoints](docs/ApiEndpoints.md) - lista completa de endpoints e exemplos
- [Uso com Postman](docs/PostmanUsage.md) - instruções para testar os endpoints via Postman
- [Exemplos de API](docs/ApiUsageExamples.md) - exemplos `curl` para os principais endpoints
- [Guia rápido de Postman](docs/PostmanQuickStart.md) - configuração rápida de ambiente para testes
- [Troubleshooting](docs/Troubleshooting.md) - problemas comuns e soluções

## Partner / autoria

- Claudio Carvalho


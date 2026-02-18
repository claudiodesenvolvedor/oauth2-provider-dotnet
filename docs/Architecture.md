# Architecture - OAuth2 Provider

## Visão geral da arquitetura
O projeto implementa um Authorization Server interno seguindo Clean Architecture. O backend e organizado em camadas com dependencias sempre apontando para o centro (Domain e Application). O frontend e uma SPA React desacoplada, consumindo a API via HTTP.

## Frontend (OAuth2.Web)

O projeto OAuth2.Web (React + Vite + TypeScript) **não faz parte da solution .NET por design**.

Motivos:
- O frontend não é compilado pelo MSBuild
- Evita incompatibilidade com versões do Visual Studio
- Permite uso de ferramentas próprias (Node, npm, Vite)
- Backend permanece enxuto e focado na API

O frontend é desenvolvido e executado de forma independente, consumindo a API via HTTP.


```
OAuth2.Web (React) -> OAuth2.Api (host) -> Presentation -> Application -> Domain
                                         -> Infrastructure -> Application/Domain
```

## Clean Architecture (camadas)
- **Domain**: entidades e regras basicas (User, Client, Token).
- **Application**: casos de uso, DTOs, interfaces e servicos.
- **Infrastructure**: MongoDB, JWT/RS256, stores e providers.
- **Presentation**: Controllers e contratos HTTP.
- **Api**: bootstrap da aplicacao e pipeline ASP.NET Core.
- **Web**: UI React + TypeScript (Vite).

## Fluxos de dependencia entre camadas
Regras principais:
- Domain nao depende de nenhuma camada.
- Application depende apenas de Domain.
- Infrastructure depende de Application e Domain.
- Presentation depende de Application.
- Api depende de Application, Infrastructure e Presentation.
- Web depende apenas de HTTP (API).

## Design patterns utilizados
- **Dependency Injection**: composicao via `AddApplication()` e `AddInfrastructure()`.
- **Repository Pattern**: contratos de persistencia e stores (Mongo).
- **Provider / Factory**: providers de chaves RSA e MongoDatabase.
- **Strategy**: fluxos OAuth (Client Credentials, Authorization Code + PKCE, Refresh Token).
- **Singleton**: `RsaKeyProvider` para cache das chaves.
- **Adapter**: mapeamento Domain ⇄ Documents (MongoDB).

## JWT + PKCE + Refresh Token no contexto da arquitetura
- **JWT** e geracao RS256 ficam na Infrastructure (`JwtTokenService`) e sao usados por servicos na Application.
- **PKCE** e validação do `code_verifier` ficam na Application (`AuthorizationCodeService`).
- **Refresh Token** segue o mesmo padrão: armazenamento via `IRefreshTokenStore` (Application) com implementação Mongo na Infrastructure.

Exemplo (fluxo de troca de code):
```
Presentation -> IAuthorizationCodeService (Application)
             -> IAuthorizationCodeStore (Infrastructure)
             -> ITokenService (Infrastructure)
```

## DI e provider de chaves RSA
- `IRsaKeyProvider` carrega chaves apenas uma vez e fica em cache.
- `IJwtKeyProvider` expõe JWKS baseado na chave publica.
- `JwtBearerOptionsSetup` configura validacao de tokens sem `BuildServiceProvider`.

## Integracao do React UI com o backend
- O React envia requests para a API usando `fetch` (`src/services/api.ts`).
- Token e salvo no `localStorage` e enviado no header `Authorization: Bearer`.
- Rotas protegidas via `PrivateRoute` no frontend.

## Racional de separacao de pastas/projetos
- **OAuth2.Domain**: entidades puras sem dependencia externa.
- **OAuth2.Application**: regras de negocio e contratos (interfaces/DTOs).
- **OAuth2.Infrastructure**: implementacoes de dados e seguranca.
- **OAuth2.Presentation**: Controllers e endpoints HTTP.
- **OAuth2.Api**: host ASP.NET Core (Program.cs, pipeline).
- **OAuth2.Web**: SPA React + Vite.

## Exemplos de codigo

### Dependencia por abstração (Application)
```csharp
public interface IClientStore
{
    Task<ClientRecord?> GetByClientIdAsync(string clientId, CancellationToken cancellationToken);
}
```

### Implementacao na Infrastructure
```csharp
public sealed class MongoClientStore : IClientStore
{
    // usa MongoDbContext para acessar a collection "clients"
}
```

### Configuracao do JWT (Api)
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsSetup>();
```

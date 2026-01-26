# Troubleshooting - OAuth2 Provider

Este documento descreve problemas comuns e como resolvê-los de forma prática.

## 1) Erro ao iniciar o backend (falha Mongo, exceptions)
**Sintoma**
- API não sobe, exceções no console ao iniciar.

**Causa provável**
- MongoDB não está rodando.
- Connection string inválida.
- Banco inexistente ou inacessível.

**Solução prática**
- Verifique se o Mongo está ativo: `mongodb://localhost:27017`.
- Confirme `MongoDb:ConnectionString` e `MongoDb:DatabaseName`.
- Ajuste as variáveis de ambiente e reinicie a API.

## 2) Backend não responde
**Sintoma**
- `http://localhost:xxxx` não responde ou dá timeout.

**Causa provável**
- Porta errada ou API não está rodando.
- Erro na pipeline (ex.: exceptions no startup).

**Solução prática**
- Execute `dotnet run` no projeto `OAuth2.Api`.
- Verifique o console para exceções.
- Confirme a porta no `launchSettings.json`.

## 3) Erro no login admin
**Sintoma**
- Login retorna 400/401 ou “Invalid credentials”.

**Causa provável**
- Usuário admin não foi seedado.
- `ADMIN_EMAIL`/`ADMIN_PASSWORD` incorretos.
- Usuário inativo.

**Solução prática**
- Confira as variáveis `ADMIN_EMAIL` e `ADMIN_PASSWORD`.
- Garanta que o seed executou (Mongo configurado corretamente).
- Ative o usuário no banco se necessário.

## 4) Token inválido / expiração
**Sintoma**
- API retorna 401 ou “invalid_token”.

**Causa provável**
- Token expirado.
- Assinatura inválida.
- Issuer/Audience inconsistentes.

**Solução prática**
- Gere novo token.
- Confirme `Jwt:Issuer` e `Jwt:Audience`.
- Verifique se a chave RS256 usada pela API é a mesma do Authorization Server.

## 5) Falhas de PKCE
**Sintoma**
- `invalid code_verifier` ou `missing code_verifier`.

**Causa provável**
- `code_verifier` ausente ou incorreto.
- `code_challenge` gerado com método diferente de S256.

**Solução prática**
- Gere `code_verifier` e `code_challenge` corretamente:
  - `code_challenge = Base64Url(SHA256(code_verifier))`
  - `code_challenge_method = S256`
- Envie `code_verifier` no `/oauth/token`.

## 6) Falhas de Refresh Token
**Sintoma**
- `invalid refresh_token` ou token expirado.

**Causa provável**
- Refresh token já foi usado (rotação).
- Refresh token expirado.

**Solução prática**
- Use o refresh token mais recente.
- Gere novo access token com o último refresh token válido.

## 7) Problemas com JWKS
**Sintoma**
- APIs não conseguem validar o token.
- Erro ao acessar `/.well-known/jwks.json`.

**Causa provável**
- Chaves RSA ausentes ou inválidas.
- Endpoint não publicado corretamente.

**Solução prática**
- Defina `JWT_PRIVATE_KEY_PATH` e `JWT_PUBLIC_KEY_PATH`.
- Verifique o endpoint `/.well-known/jwks.json`.

## 8) Erros no frontend (rota, private route)
**Sintoma**
- Redirecionamento constante para `/login`.
- Página em branco após login.

**Causa provável**
- Token ausente no `localStorage`.
- `PrivateRoute` bloqueando acesso por falta de token.

**Solução prática**
- Verifique se o login salvou `auth_token`.
- Confirme se o token existe no `localStorage`.

## 9) Dicas de logs
**Sintoma**
- Erros genéricos sem detalhe.

**Causa provável**
- Logs insuficientes.

**Solução prática**
- Consulte o console da API ao rodar localmente.
- Use logs estruturados no backend quando necessário.

## 10) Verificação de variáveis de ambiente
**Sintoma**
- Comportamento inconsistente entre ambientes.

**Causa provável**
- Variáveis ausentes ou incorretas.

**Solução prática**
- Verifique:
  - `ADMIN_EMAIL`
  - `ADMIN_PASSWORD`
  - `JWT_PRIVATE_KEY_PATH`
  - `JWT_PUBLIC_KEY_PATH`

## 11) Testes falhando
**Sintoma**
- `dotnet test` com falhas de autenticação ou Mongo.

**Causa provável**
- Configuração de testes diferente do ambiente.
- Seed não aplicado ou conflito de dependências.

**Solução prática**
- Rode `dotnet test` após garantir build limpo.
- Verifique overrides no `ApiFactory`.
- Confirme se o `TestAuthHandler` está ativo nos testes.

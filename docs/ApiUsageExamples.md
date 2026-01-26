# Exemplos de Uso da API (curl)

## Login Admin
```bash
curl -X POST http://localhost:xxxx/auth/login \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"admin@local\",\"password\":\"Admin123!\"}"
```
**Resposta:** token JWT administrativo em `accessToken`.

## Client Credentials
```bash
curl -X POST http://localhost:xxxx/oauth/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=client_credentials&client_id=XXX&client_secret=YYY&scope=api.read"
```
**Resposta:** access_token do client.

## Authorization Code + PKCE
1) Gere `code_verifier` e `code_challenge` (S256).
2) Autorize:
```bash
curl "http://localhost:xxxx/oauth/authorize?client_id=XXX&redirect_uri=https%3A%2F%2Fapp.exemplo.com%2Fcallback&response_type=code&scope=api.read&code_challenge=CHALLENGE&code_challenge_method=S256"
```
3) Troque o code:
```bash
curl -X POST http://localhost:xxxx/oauth/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=authorization_code&client_id=XXX&client_secret=YYY&code=CODE&redirect_uri=https%3A%2F%2Fapp.exemplo.com%2Fcallback&code_verifier=VERIFIER"
```
**Resposta:** access_token + refresh_token.

## Refresh Token
```bash
curl -X POST http://localhost:xxxx/oauth/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=refresh_token&client_id=XXX&client_secret=YYY&refresh_token=ZZZ"
```
**Resposta:** novo access_token e refresh_token.

## JWKS
```bash
curl http://localhost:xxxx/.well-known/jwks.json
```
**Resposta:** chave pública para validação de JWT.

## GET /users
```bash
curl http://localhost:xxxx/users \
  -H "Authorization: Bearer ADMIN_TOKEN"
```
**Resposta:** lista de usuários.

## POST /users
```bash
curl -X POST http://localhost:xxxx/users \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"novo@local\",\"password\":\"Senha123!\"}"
```
**Resposta:** usuário criado.

## GET /clients
```bash
curl http://localhost:xxxx/clients \
  -H "Authorization: Bearer ADMIN_TOKEN"
```
**Resposta:** lista de clients.

## POST /clients
```bash
curl -X POST http://localhost:xxxx/clients \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{\"scopes\":[\"api.read\"],\"redirectUris\":[\"https://app.exemplo.com/callback\"]}"
```
**Resposta:** client criado (client_secret aparece uma vez).

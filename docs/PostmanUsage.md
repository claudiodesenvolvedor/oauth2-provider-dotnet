# Uso com Postman - OAuth2 Provider

Este guia mostra como testar os endpoints da API via Postman.

## Autenticação — Login Admin
**Endpoint:** `POST /auth/login`  
**Headers:** `Content-Type: application/json`  
**Body (JSON):**
```json
{
  "email": "admin@local",
  "password": "Admin123!"
}
```

**Salvar token no Postman (Tests):**
```javascript
const json = pm.response.json()
pm.environment.set("adminToken", json.accessToken)
```

## Obter Token OAuth2 — Client Credentials
**Endpoint:** `POST /oauth/token`  
**Headers:** `Content-Type: application/x-www-form-urlencoded`  
**Body (x-www-form-urlencoded):**
```
grant_type=client_credentials
client_id=CLIENT_ID
client_secret=CLIENT_SECRET
scope=api.read
```

**Salvar token no Postman (Tests):**
```javascript
const json = pm.response.json()
pm.environment.set("clientToken", json.accessToken)
```

## Obter Token OAuth2 — Authorization Code + PKCE
1) Gerar `code_verifier` e `code_challenge` (S256).  
2) Chamar `/oauth/authorize` com `code_challenge`.  
3) Trocar `code` por token em `/oauth/token` com `code_verifier`.

**Authorize (GET /oauth/authorize)**  
Use o navegador ou uma chamada GET e capture o `code` do redirect.

**Token (POST /oauth/token)**  
Headers: `Content-Type: application/x-www-form-urlencoded`  
Body:
```
grant_type=authorization_code
client_id=CLIENT_ID
client_secret=CLIENT_SECRET
code=AUTH_CODE
redirect_uri=https://app.exemplo.com/callback
code_verifier=CODE_VERIFIER
```

## Refresh Token
**Endpoint:** `POST /oauth/token`  
**Headers:** `Content-Type: application/x-www-form-urlencoded`  
**Body:**
```
grant_type=refresh_token
client_id=CLIENT_ID
client_secret=CLIENT_SECRET
refresh_token=REFRESH_TOKEN
```

## Usuários — GET /users
**Endpoint:** `GET /users`  
**Headers:** `Authorization: Bearer {{adminToken}}`

## Criar Usuário — POST /users
**Endpoint:** `POST /users`  
**Headers:**  
- `Authorization: Bearer {{adminToken}}`  
- `Content-Type: application/json`

**Body (JSON):**
```json
{
  "email": "novo@local",
  "password": "Senha123!"
}
```

## Clients — GET /clients
**Endpoint:** `GET /clients`  
**Headers:** `Authorization: Bearer {{adminToken}}`

## Criar Client — POST /clients
**Endpoint:** `POST /clients`  
**Headers:**  
- `Authorization: Bearer {{adminToken}}`  
- `Content-Type: application/json`

**Body (JSON):**
```json
{
  "scopes": ["api.read"],
  "redirectUris": ["https://app.exemplo.com/callback"]
}
```

## Dicas de uso no Postman
- Crie um **Environment** com `host`, `adminToken` e `clientToken`.
- Use `{{host}}` nos endpoints, ex: `{{host}}/auth/login`.
- Salve tokens nos scripts de **Tests** para reutilizar em outras requisições.

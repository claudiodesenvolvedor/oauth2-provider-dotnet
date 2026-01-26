# API Endpoints - OAuth2 Provider

## Autenticação

### POST /auth/login
- **Método:** POST
- **URL:** `/auth/login`
- **Autenticação:** não
- **Parâmetros:** `email`, `password` (JSON)

**Request (curl)**
```bash
curl -X POST http://localhost:xxxx/auth/login \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"admin@local\",\"password\":\"Admin123!\"}"
```

**Response (200)**
```json
{
  "accessToken": "jwt",
  "expiresIn": 300
}
```

### POST /oauth/token (client_credentials)
- **Método:** POST
- **URL:** `/oauth/token`
- **Autenticação:** não
- **Parâmetros:** `grant_type=client_credentials`, `client_id`, `client_secret`, `scope`

**Request (curl)**
```bash
curl -X POST http://localhost:xxxx/oauth/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=client_credentials&client_id=XXX&client_secret=YYY&scope=api.read"
```

**Response (200)**
```json
{
  "accessToken": "jwt",
  "tokenType": "Bearer",
  "expiresIn": 300,
  "scope": "api.read",
  "refreshToken": null
}
```

### POST /oauth/token (authorization_code + PKCE)
- **Método:** POST
- **URL:** `/oauth/token`
- **Autenticação:** não
- **Parâmetros:** `grant_type=authorization_code`, `client_id`, `client_secret`, `code`, `redirect_uri`, `code_verifier`

**Request (curl)**
```bash
curl -X POST http://localhost:xxxx/oauth/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=authorization_code&client_id=XXX&client_secret=YYY&code=CODE&redirect_uri=https%3A%2F%2Fapp.exemplo.com%2Fcb&code_verifier=VERIFIER"
```

**Response (200)**
```json
{
  "accessToken": "jwt",
  "tokenType": "Bearer",
  "expiresIn": 300,
  "scope": "api.read",
  "refreshToken": "refresh"
}
```

### POST /oauth/token (refresh_token)
- **Método:** POST
- **URL:** `/oauth/token`
- **Autenticação:** não
- **Parâmetros:** `grant_type=refresh_token`, `client_id`, `client_secret`, `refresh_token`

**Request (curl)**
```bash
curl -X POST http://localhost:xxxx/oauth/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=refresh_token&client_id=XXX&client_secret=YYY&refresh_token=ZZZ"
```

**Response (200)**
```json
{
  "accessToken": "jwt",
  "tokenType": "Bearer",
  "expiresIn": 300,
  "scope": "api.read",
  "refreshToken": "refresh"
}
```

### GET /.well-known/jwks.json
- **Método:** GET
- **URL:** `/.well-known/jwks.json`
- **Autenticação:** não
- **Parâmetros:** nenhum

**Request (curl)**
```bash
curl http://localhost:xxxx/.well-known/jwks.json
```

**Response (200)**
```json
{
  "keys": [
    {
      "kty": "RSA",
      "use": "sig",
      "alg": "RS256",
      "kid": "key-id",
      "n": "base64url",
      "e": "AQAB"
    }
  ]
}
```

## Users

### GET /users
- **Método:** GET
- **URL:** `/users`
- **Autenticação:** Bearer (scope=admin)
- **Parâmetros:** nenhum

**Request (curl)**
```bash
curl http://localhost:xxxx/users \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

**Response (200)**
```json
[
  {
    "id": "user-id",
    "email": "admin@local",
    "isActive": true
  }
]
```

### GET /users/{id}
- **Método:** GET
- **URL:** `/users/{id}`
- **Autenticação:** Bearer (scope=admin)
- **Parâmetros:** `id`

**Request (curl)**
```bash
curl http://localhost:xxxx/users/USER_ID \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

**Response (200)**
```json
{
  "id": "user-id",
  "email": "admin@local",
  "isActive": true
}
```

### POST /users
- **Método:** POST
- **URL:** `/users`
- **Autenticação:** Bearer (scope=admin)
- **Parâmetros:** `email`, `password`

**Request (curl)**
```bash
curl -X POST http://localhost:xxxx/users \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"novo@local\",\"password\":\"Senha123!\"}"
```

**Response (201)**
```json
{
  "id": "user-id",
  "email": "novo@local",
  "isActive": true
}
```

### PUT /users/{id}
- **Método:** PUT
- **URL:** `/users/{id}`
- **Autenticação:** Bearer (scope=admin)
- **Parâmetros:** `isActive`

**Request (curl)**
```bash
curl -X PUT http://localhost:xxxx/users/USER_ID \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{\"isActive\":false}"
```

**Response (200)**
```json
{
  "id": "user-id",
  "email": "novo@local",
  "isActive": false
}
```

### DELETE /users/{id}
- **Método:** DELETE
- **URL:** `/users/{id}`
- **Autenticação:** Bearer (scope=admin)
- **Parâmetros:** `id`

**Request (curl)**
```bash
curl -X DELETE http://localhost:xxxx/users/USER_ID \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

**Response (204)**
```json
{}
```

## Clients

### GET /clients
- **Método:** GET
- **URL:** `/clients`
- **Autenticação:** Bearer (scope=admin)
- **Parâmetros:** nenhum

**Request (curl)**
```bash
curl http://localhost:xxxx/clients \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

**Response (200)**
```json
[
  {
    "clientId": "client-id",
    "scopes": ["api.read"],
    "redirectUris": ["https://app.exemplo.com/cb"]
  }
]
```

### GET /clients/{id}
- **Método:** GET
- **URL:** `/clients/{id}`
- **Autenticação:** Bearer (scope=admin)
- **Parâmetros:** `id`

**Request (curl)**
```bash
curl http://localhost:xxxx/clients/CLIENT_ID \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

**Response (200)**
```json
{
  "clientId": "client-id",
  "scopes": ["api.read"],
  "redirectUris": ["https://app.exemplo.com/cb"]
}
```

### POST /clients
- **Método:** POST
- **URL:** `/clients`
- **Autenticação:** Bearer (scope=admin)
- **Parâmetros:** `scopes`, `redirectUris`

**Request (curl)**
```bash
curl -X POST http://localhost:xxxx/clients \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{\"scopes\":[\"api.read\"],\"redirectUris\":[\"https://app.exemplo.com/cb\"]}"
```

**Response (201)**
```json
{
  "clientId": "client-id",
  "clientSecret": "secret",
  "scopes": ["api.read"],
  "redirectUris": ["https://app.exemplo.com/cb"]
}
```

### PUT /clients/{id}
- **Método:** PUT
- **URL:** `/clients/{id}`
- **Autenticação:** Bearer (scope=admin)
- **Parâmetros:** `scopes`, `redirectUris`

**Request (curl)**
```bash
curl -X PUT http://localhost:xxxx/clients/CLIENT_ID \
  -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{\"scopes\":[\"api.read\"],\"redirectUris\":[\"https://app.exemplo.com/cb\"]}"
```

**Response (200)**
```json
{
  "clientId": "client-id",
  "scopes": ["api.read"],
  "redirectUris": ["https://app.exemplo.com/cb"]
}
```

### DELETE /clients/{id}
- **Método:** DELETE
- **URL:** `/clients/{id}`
- **Autenticação:** Bearer (scope=admin)
- **Parâmetros:** `id`

**Request (curl)**
```bash
curl -X DELETE http://localhost:xxxx/clients/CLIENT_ID \
  -H "Authorization: Bearer ADMIN_TOKEN"
```

**Response (204)**
```json
{}
```

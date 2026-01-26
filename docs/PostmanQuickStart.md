# Postman Quick Start

## 1) Criar environment
Crie um environment no Postman com as seguintes variáveis:
- `host` (ex.: `http://localhost:xxxx`)
- `adminToken`
- `clientToken`

## 2) Login Admin (salvar token)
**Request:** `POST {{host}}/auth/login`  
**Body (JSON):**
```json
{
  "email": "admin@local",
  "password": "Admin123!"
}
```

**Tests (salvar token):**
```javascript
const json = pm.response.json()
pm.environment.set("adminToken", json.accessToken)
```

## 3) Client Credentials (salvar token)
**Request:** `POST {{host}}/oauth/token`  
**Body (x-www-form-urlencoded):**
```
grant_type=client_credentials
client_id=CLIENT_ID
client_secret=CLIENT_SECRET
scope=api.read
```

**Tests (salvar token):**
```javascript
const json = pm.response.json()
pm.environment.set("clientToken", json.accessToken)
```

# Manual OAuth2 Provider

## 1) Visao Geral do Sistema
Este sistema e um Authorization Server interno para emissao de tokens JWT para microsservicos. O acesso administrativo e feito por uma UI React, consumindo a API do backend em .NET 8.

## 2) Acesso ao Sistema
- URL da UI: http://localhost:xxxx
- Login Admin: email e senha de admin.

Seed inicial (via ENV):
- `ADMIN_EMAIL` (padrao: admin@local)
- `ADMIN_PASSWORD` (padrao: Admin123!)

## 3) Gerenciamento de Clients via UI
Tela `/clients`:
- Listagem de clients existentes.
- Criacao de novo client com scopes e redirect_uris (uma por linha ou separados por espaco).
- `client_secret` aparece apenas uma vez apos a criacao.

## 4) Gerenciamento de Usuarios via UI
Tela `/users`:
- Listar usuarios.
- Criar usuario admin.
- Ativar/Desativar usuarios.

Usuarios inativos nao conseguem logar.

## 5) Client Credentials Flow
O que e:
- Fluxo para integracao entre servicos sem usuario final.

Exemplo curl:
```bash
curl -X POST http://localhost:xxxx/oauth/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=client_credentials&client_id=XXX&client_secret=YYY&scope=api.read"
```

## 6) Authorization Code + PKCE Flow
O que e:
- Fluxo para autenticacao de usuarios, com protecao PKCE (S256).

Passos (authorize -> token):
1) Gerar code_verifier e code_challenge (S256).
2) Chamar /oauth/authorize para obter o code.
3) Trocar o code por token em /oauth/token usando code_verifier.

PKCE basico:
- `code_challenge = Base64Url(SHA256(code_verifier))`
- `code_challenge_method = S256`

## 7) Refresh Token
O que e:
- Token de longa duracao para renovacao do access_token.

Uso e rotacao:
- O refresh_token e invalidado apos uso e um novo e emitido.

Exemplo:
```bash
curl -X POST http://localhost:xxxx/oauth/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=refresh_token&client_id=XXX&client_secret=YYY&refresh_token=ZZZ"
```

## 8) JWKS
O que e:
- Endpoint publico que expõe a chave publica para validacao de tokens.

Endpoint:
- `/.well-known/jwks.json`

## 9) Boas Praticas de Seguranca
- Nao versionar chaves privadas.
- Armazenar client_secret com segurança.
- Rotacionar segredos periodicamente.
- Usar HTTPS sempre.
- Tokens com expiracao curta.

## 10) Troubleshooting Operacional
- Verificar variaveis de ambiente (ADMIN_EMAIL, ADMIN_PASSWORD, JWT_PRIVATE_KEY_PATH, JWT_PUBLIC_KEY_PATH).
- Validar conexao com MongoDB.
- Conferir logs da API em caso de 401/400.
- Confirmar existencia de usuarios ativos para login admin.

## 11) Referencias e Ferramentas Recomendadas
- RFC 6749 (OAuth2)
- RFC 7636 (PKCE)
- JWT (RFC 7519)
- Postman / Insomnia para testes de API

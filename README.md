# OAuth2 Authorization Server (Interno)

## Chaves RSA e JWKS (RS256)

### Gerar par de chaves RSA

Utilize o script PowerShell fornecido para gerar um par de chaves privada/pública fora do repositório:

```
.\tools\generate-rsa-keys.ps1 -OutputDir "C:\keys\oauth2"
```

O script criará os arquivos `private.pem` e `public.pem` no diretório especificado e retornará um valor de `kid`.

### Configurar armazenamento das chaves (sem segredos no repositório)

Defina uma das opções abaixo:

**Via caminho de arquivo:**
  - `JWT_PRIVATE_KEY_PATH`
  - `JWT_PUBLIC_KEY_PATH`
- Strings PEM (recomendado apenas com secret manager):
  - `JWT_PRIVATE_KEY_PEM`
  - `JWT_PUBLIC_KEY_PEM`

### Configurações obrigatórias (JwtSettings)

As seguintes configurações são obrigatórias:

- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:AccessTokenMinutes`
- `Jwt:KeyId` (deve corresponder ao `kid` publicado no JWKS)

⚠️ Nunca armazene chaves privadas ou públicas neste repositório.


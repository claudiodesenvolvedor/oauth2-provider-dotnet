# OAuth2 Authorization Server (Internal)

## RSA keys and JWKS (RS256)

### Generate RSA key pair

Use the provided PowerShell script to generate a private/public key pair outside the repository:

```
.\tools\generate-rsa-keys.ps1 -OutputDir "C:\keys\oauth2"
```

The script will create `private.pem` and `public.pem` in the specified folder and output a `kid` value.

### Configure key storage (no secrets in repo)

Set one of the following options:

- File paths:
  - `JWT_PRIVATE_KEY_PATH`
  - `JWT_PUBLIC_KEY_PATH`
- PEM strings (recommended only with secret manager):
  - `JWT_PRIVATE_KEY_PEM`
  - `JWT_PUBLIC_KEY_PEM`

### Required JwtSettings

The following settings are required in configuration:

- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:AccessTokenMinutes`
- `Jwt:KeyId` (must match the `kid` used for JWKS)

Do not store private/public key material in this repository.

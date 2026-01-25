param(
    [Parameter(Mandatory = $true)]
    [string]$OutputDir
)

if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir | Out-Null
}

$rsa = [System.Security.Cryptography.RSA]::Create(2048)

$privateBytes = $rsa.ExportPkcs8PrivateKey()
$publicBytes = $rsa.ExportSubjectPublicKeyInfo()

$privatePem = [System.Security.Cryptography.PemEncoding]::Write("PRIVATE KEY", $privateBytes)
$publicPem = [System.Security.Cryptography.PemEncoding]::Write("PUBLIC KEY", $publicBytes)

[System.IO.File]::WriteAllText((Join-Path $OutputDir "private.pem"), [string]$privatePem)
[System.IO.File]::WriteAllText((Join-Path $OutputDir "public.pem"), [string]$publicPem)

$keyId = [Guid]::NewGuid().ToString("N")
Write-Host "Generated keys in: $OutputDir"
Write-Host "KeyId (kid): $keyId"
Write-Host "Set env vars:"
Write-Host "  JWT_PRIVATE_KEY_PATH=$OutputDir\private.pem"
Write-Host "  JWT_PUBLIC_KEY_PATH=$OutputDir\public.pem"
Write-Host "  Jwt:KeyId=$keyId"

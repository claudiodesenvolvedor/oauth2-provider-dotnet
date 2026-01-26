namespace OAuth2.Application.DTOs.Auth;

public sealed record AdminLoginResponse(
    string AccessToken,
    int ExpiresIn);

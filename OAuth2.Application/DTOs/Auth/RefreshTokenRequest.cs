namespace OAuth2.Application.DTOs.Auth;

public sealed record RefreshTokenRequest(
    string ClientId,
    string ClientSecret,
    string RefreshToken);

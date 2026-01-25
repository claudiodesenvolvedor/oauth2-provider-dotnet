namespace OAuth2.Application.DTOs.Errors;

public sealed record ErrorResponse(
    string Code,
    string Message);

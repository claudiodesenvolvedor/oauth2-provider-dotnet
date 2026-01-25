namespace OAuth2.Application.DTOs.Errors;

public sealed record ValidationErrorResponse(
    string Code,
    string Message,
    IReadOnlyDictionary<string, string[]> Errors);

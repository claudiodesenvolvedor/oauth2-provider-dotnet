namespace OAuth2.Application.DTOs.Security;

public sealed record JwksResponse(
    IReadOnlyList<JwksKey> Keys);

public sealed record JwksKey(
    string Kty,
    string Use,
    string Alg,
    string Kid,
    string N,
    string E);

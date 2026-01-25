using Microsoft.AspNetCore.Mvc;
using OAuth2.Application.DTOs.Auth;
using OAuth2.Application.DTOs.Errors;
using OAuth2.Application.Interfaces.Services;

namespace OAuth2.Presentation.Controllers;

[ApiController]
[Route("oauth")]
public sealed class TokenController : ControllerBase
{
    private readonly IClientCredentialsService _clientCredentialsService;
    private readonly IAuthorizationCodeService _authorizationCodeService;
    private readonly IRefreshTokenService _refreshTokenService;

    public TokenController(
        IClientCredentialsService clientCredentialsService,
        IAuthorizationCodeService authorizationCodeService,
        IRefreshTokenService refreshTokenService)
    {
        _clientCredentialsService = clientCredentialsService;
        _authorizationCodeService = authorizationCodeService;
        _refreshTokenService = refreshTokenService;
    }

    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TokenResponse>> TokenAsync(
        [FromForm] TokenRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var grantType = ResolveValue(request.GrantType, "grant_type");
            var clientId = ResolveValue(request.ClientId, "client_id");
            var clientSecret = ResolveValue(request.ClientSecret, "client_secret");
            var scope = ResolveOptionalValue(request.Scope, "scope");
            var code = ResolveOptionalValue(request.Code, "code");
            var redirectUri = ResolveOptionalValue(request.RedirectUri, "redirect_uri");
        var refreshToken = ResolveOptionalValue(request.RefreshToken, "refresh_token");

            if (grantType == "client_credentials")
            {
                var clientRequest = new ClientCredentialsTokenRequest(
                    grantType,
                    clientId,
                    clientSecret,
                    scope);

                var response = await _clientCredentialsService
                    .IssueTokenAsync(clientRequest, cancellationToken);

                return new TokenResponse(
                    response.AccessToken,
                    response.TokenType,
                    response.ExpiresIn,
                    response.Scope,
                    null);
            }

            if (grantType == "authorization_code")
            {
                var authCodeRequest = new AuthorizationCodeTokenRequest(
                    clientId,
                    clientSecret,
                    code ?? string.Empty,
                    redirectUri ?? string.Empty);

                var response = await _authorizationCodeService
                    .ExchangeTokenAsync(authCodeRequest, cancellationToken);

                return new TokenResponse(
                    response.AccessToken,
                    response.TokenType,
                    response.ExpiresIn,
                    response.Scope,
                    response.RefreshToken);
            }

            if (grantType == "refresh_token")
            {
                var refreshRequest = new RefreshTokenRequest(
                    clientId,
                    clientSecret,
                    refreshToken ?? string.Empty);

                var response = await _refreshTokenService
                    .ExchangeAsync(refreshRequest, cancellationToken);

                return new TokenResponse(
                    response.AccessToken,
                    response.TokenType,
                    response.ExpiresIn,
                    response.Scope,
                    response.RefreshToken);
            }

            return BadRequest(new ErrorResponse("invalid_grant", "Unsupported grant_type."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponse("invalid_request", ex.Message));
        }
    }

    private string ResolveValue(string? current, string formKey)
    {
        if (!string.IsNullOrWhiteSpace(current))
        {
            return current;
        }

        return Request.Form[formKey].ToString();
    }

    private string? ResolveOptionalValue(string? current, string formKey)
    {
        if (!string.IsNullOrWhiteSpace(current))
        {
            return current;
        }

        var value = Request.Form[formKey].ToString();
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}

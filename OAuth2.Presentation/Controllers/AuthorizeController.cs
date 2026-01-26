using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using OAuth2.Application.DTOs.Auth;
using OAuth2.Application.Interfaces.Services;

namespace OAuth2.Presentation.Controllers;

[ApiController]
[Route("oauth")]
public sealed class AuthorizeController : ControllerBase
{
    private readonly IAuthorizationCodeService _authorizationCodeService;

    public AuthorizeController(IAuthorizationCodeService authorizationCodeService)
    {
        _authorizationCodeService = authorizationCodeService;
    }

    [HttpGet("authorize")]
    public async Task<IActionResult> AuthorizeAsync(
        [FromQuery(Name = "client_id")] string clientId,
        [FromQuery(Name = "redirect_uri")] string redirectUri,
        [FromQuery(Name = "scope")] string? scope,
        [FromQuery(Name = "state")] string? state,
        [FromQuery(Name = "code_challenge")] string? codeChallenge,
        [FromQuery(Name = "code_challenge_method")] string? codeChallengeMethod,
        CancellationToken cancellationToken)
    {
        var request = new AuthorizationCodeRequest(
            clientId,
            redirectUri,
            "code",
            scope,
            state,
            codeChallenge,
            codeChallengeMethod);

        var response = await _authorizationCodeService
            .CreateCodeAsync(request, cancellationToken);

        var redirectUrl = QueryHelpers.AddQueryString(
            response.RedirectUri,
            new Dictionary<string, string?>
            {
                ["code"] = response.Code,
                ["state"] = response.State
            });

        return Redirect(redirectUrl);
    }
}

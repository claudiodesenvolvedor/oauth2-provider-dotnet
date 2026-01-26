using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuth2.Application.DTOs.Clients;
using OAuth2.Application.Interfaces.Services;

namespace OAuth2.Presentation.Controllers;

[ApiController]
[Route("clients")]
[Authorize(Policy = "admin")]
public sealed class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ClientResponse>), StatusCodes.Status200OK)]
    public Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status501NotImplemented));
    }

    [HttpGet("{clientId}")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
    public Task<IActionResult> GetByClientIdAsync(
        string clientId,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status501NotImplemented));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status201Created)]
    public Task<IActionResult> CreateAsync(
        [FromBody] ClientCreateRequest request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status501NotImplemented));
    }

    [HttpPut("{clientId}")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
    public Task<IActionResult> UpdateAsync(
        string clientId,
        [FromBody] ClientUpdateRequest request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status501NotImplemented));
    }

    [HttpDelete("{clientId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> DeleteAsync(string clientId, CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status501NotImplemented));
    }
}

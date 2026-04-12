using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkipSmart.Application.Users.ChangeGroup;
using SkipSmart.Application.Users.ChangeSubgroups;
using SkipSmart.Application.Users.GetLoggedInUser;
using SkipSmart.Application.Users.LoginUser;
using SkipSmart.Application.Users.RegisterUser;

namespace SkipSmart.Api.Controllers.Users;

[ApiController]
[Route("api/core-service/v1/users")]
public class UsersController : ControllerBase {
    private readonly ISender _sender;

    public UsersController(ISender sender) {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken) {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure) {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody] LoginUserQuery query, CancellationToken cancellationToken) {
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure) {
            return Unauthorized(result.Error);
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken) {
        var result = await _sender.Send(new GetLoggedInUserQuery(), cancellationToken);

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("change-group")]
    public async Task<IActionResult> ChangeGroup([FromBody] ChangeGroupCommand command, CancellationToken cancellationToken) {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure) {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("change-subgroups")]
    public async Task<IActionResult> ChangeSubGroups([FromBody] ChangeSubgroupsCommand command, CancellationToken cancellationToken) {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure) {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkipSmart.Application.Users.ChangeGroup;
using SkipSmart.Application.Users.ChangeSubgroups;
using SkipSmart.Application.Users.GetLoggedInUser;
using SkipSmart.Application.Users.LoginUser;
using SkipSmart.Application.Users.RegisterUser;
using SkipSmart.Application.Users.SendNewVerificationEmail;
using SkipSmart.Application.Users.VerifyEmail;

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
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken) {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.LanguageSubgroup,
            request.FacultySubgroup,
            request.Password,
            request.GroupId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure) {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpPost("send-verification-email")]
    public async Task<IActionResult> SendVerificationEmail(CancellationToken cancellationToken) {
        var command = new SendNewVerificationEmailCommand();

        var result = await _sender.Send(command, cancellationToken);

        return Ok();
    }
    
    [Authorize]
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request, CancellationToken cancellationToken) {
        var command = new VerifyEmailCommand(request.EmailVerificationCode);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure) {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody] LoginUserRequest request, CancellationToken cancellationToken) {
        var command = new LoginUserQuery(request.Email, request.Password);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure) {
            return Unauthorized(result.Error);
        }
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken) {
        var query = new GetLoggedInUserQuery();

        var result = await _sender.Send(query, cancellationToken);
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpPatch("change-group")]
    public async Task<IActionResult> ChangeGroup([FromBody] ChangeGroupRequest request, CancellationToken cancellationToken) {
        var command = new ChangeGroupCommand(request.NewGroupId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure) {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpPatch("change-subgroups")]
    public async Task<IActionResult> ChangeSubGroups([FromBody] ChangeSubgroupsRequest request, CancellationToken cancellationToken) {
        var command = new ChangeSubgroupsCommand(request.NewLanguageSubgroup, request.NewFacultySubgroup);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure) {
            return BadRequest(result.Error);
        }
        
        return Ok();
    }
}
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkipSmart.Application.Groups.GetAllGroups;
using SkipSmart.Domain.Users;

namespace SkipSmart.Api.Controllers.Groups;

[ApiController]
[Route("api/core-service/v1/groups")]
public class GroupsController : ControllerBase {
    private readonly ISender _sender;
    
    public GroupsController(ISender sender) {
        _sender = sender;
    }
    
    [AllowAnonymous]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllGroups(CancellationToken cancellationToken) {
        var query = new GetAllGroupsQuery();
        
        var result = await _sender.Send(query, cancellationToken);
        
        return Ok(result.Value);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] SkipSmart.Application.Groups.CreateGroup.CreateGroupCommand request, CancellationToken cancellationToken) {
        var result = await _sender.Send(request, cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateGroup(Guid id, [FromBody] SkipSmart.Application.Groups.UpdateGroup.UpdateGroupCommand request, CancellationToken cancellationToken) {
        if (id != request.GroupId) return BadRequest("Id mismatch");
        var result = await _sender.Send(request, cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGroup(Guid id, CancellationToken cancellationToken) {
        var result = await _sender.Send(new SkipSmart.Application.Groups.DeleteGroup.DeleteGroupCommand(id), cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok();
    }
}
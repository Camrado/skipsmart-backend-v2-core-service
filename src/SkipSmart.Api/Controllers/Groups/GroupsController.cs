using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkipSmart.Application.Groups.GetAllGroups;

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
}
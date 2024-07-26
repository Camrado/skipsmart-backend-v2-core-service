using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkipSmart.Application.Courses.GetCoursesForGroup;

namespace SkipSmart.Api.Controllers.Courses;

[ApiController]
[Route("api/core-service/v1/courses")]
public class CoursesController : ControllerBase {
    private readonly ISender _sender;
    
    public CoursesController(ISender sender) {
        _sender = sender;
    }
    
    [Authorize(Policy = "EmailVerified")]
    [HttpGet("all")]
    public async Task<IActionResult> GetCoursesForGroup(CancellationToken cancellationToken) {
        var query = new GetCoursesForGroupQuery();
        
        var result = await _sender.Send(query, cancellationToken);
        
        if (result.IsFailure) {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
}
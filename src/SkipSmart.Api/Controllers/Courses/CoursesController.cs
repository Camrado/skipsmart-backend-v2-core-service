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
    
    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetCoursesForGroup(CancellationToken cancellationToken) {
        var query = new GetCoursesForGroupQuery();
        
        var result = await _sender.Send(query, cancellationToken);
        
        if (result.IsFailure) {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetCourses([FromQuery] Guid? groupId, CancellationToken cancellationToken) {
        var result = await _sender.Send(new SkipSmart.Application.Courses.GetCourses.GetCoursesQuery(groupId), cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] SkipSmart.Application.Courses.CreateCourse.CreateCourseCommand request, CancellationToken cancellationToken) {
        var result = await _sender.Send(request, cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] SkipSmart.Application.Courses.UpdateCourse.UpdateCourseCommand request, CancellationToken cancellationToken) {
        if (id != request.CourseId) return BadRequest("Id mismatch");
        var result = await _sender.Send(request, cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken cancellationToken) {
        var result = await _sender.Send(new SkipSmart.Application.Courses.DeleteCourse.DeleteCourseCommand(id), cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok();
    }
}
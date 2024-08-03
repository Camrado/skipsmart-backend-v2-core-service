using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkipSmart.Application.Attendances.GetAttendancesForDate;
using SkipSmart.Application.Attendances.GetAttendanceStatistics;
using SkipSmart.Application.Attendances.GetTimetableForGroup;
using SkipSmart.Application.Attendances.GetUnmarkedDates;
using SkipSmart.Application.Attendances.RecordAttendance;

namespace SkipSmart.Api.Controllers.Attendances;

[ApiController]
[Route("api/core-service/v1/attendances")]
public class AttendancesController : ControllerBase {
    private readonly ISender _sender;
    
    public AttendancesController(ISender sender) {
        _sender = sender;
    }
    
    [Authorize]
    [HttpGet("{date}")]
    public async Task<IActionResult> GetAttendancesForDate([FromRoute] string date, CancellationToken cancellationToken) {
        if (!DateOnly.TryParse(date, out var parsedDate)) {
            return BadRequest("Invalid date format. Please use 'YYYY-MM-DD'.");
        }
        
        var query = new GetAttendancesForDateQuery(parsedDate);
        
        var result = await _sender.Send(query, cancellationToken);
        
        if (result.IsFailure) {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpGet("statistics")]
    public async Task<IActionResult> GetAttendanceStatistics([FromQuery] Guid courseId, CancellationToken cancellationToken) {
        var query = new GetAttendanceStatisticsQuery(courseId);
        
        var result = await _sender.Send(query, cancellationToken);
        
        if (result.IsFailure) {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpGet("timetable/{date}")]
    public async Task<IActionResult> GetTimetable([FromRoute] string date, CancellationToken cancellationToken) {
        if (!DateOnly.TryParse(date, out var parsedDate)) {
            return BadRequest("Invalid date format. Please use 'YYYY-MM-DD'.");
        }
        
        var query = new GetTimetableForGroupQuery(parsedDate);
        
        var result = await _sender.Send(query, cancellationToken);
        
        if (result.IsFailure) {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpGet("unmarked-dates")]
    public async Task<IActionResult> GetUnmarkedDates(CancellationToken cancellationToken) {
        var query = new GetUnmarkedDatesQuery();
        
        var result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpPost("record-attendance")]
    public async Task<IActionResult> RecordAttendance([FromBody] RecordAttendanceRequest request, CancellationToken cancellationToken) {
        var command = new RecordAttendanceCommand(
            request.AttendanceDate,
            request.IsDateMarked,
            request.CourseId,
            request.HasAttended,
            request.Period);
        
        var result = await _sender.Send(command, cancellationToken);
        
        if (result.IsFailure) {
            return BadRequest(result.Error);
        }
        
        return Ok();
    }
}
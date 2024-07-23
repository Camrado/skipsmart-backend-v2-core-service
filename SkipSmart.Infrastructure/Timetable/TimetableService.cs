using System.Net.Http.Json;
using System.Web;
using Microsoft.Extensions.Options;
using SkipSmart.Application.Abstractions.Timetable;
using SkipSmart.Application.Attendances.GetTimetableForGroup;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Groups;
using SkipSmart.Domain.Users;

namespace SkipSmart.Infrastructure.Timetable;

internal sealed class TimetableService : ITimetableService {
    private readonly HttpClient _httpClient;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly string _key;
    
    public TimetableService(HttpClient httpClient, IGroupRepository groupRepository, IUserRepository userRepository) {
        _httpClient = httpClient;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        // TODO: Add Timetable API key to .env file
        _key = Environment.GetEnvironmentVariable("TIMETABLE_API_KEY") 
               ?? throw new ApplicationException("Timetable API key is missing");
    }
    
    public async Task<Result<IReadOnlyList<CourseTimetableForGroupResponse>>> GetTimetableForDate(Guid groupId, 
        DateOnly timetableDate, CancellationToken cancellationToken = default) 
    {
        var group = await _groupRepository.GetByIdAsync(groupId, cancellationToken);
        var edupageClassId = group.EdupageClassId;

        var queryParams = HttpUtility.ParseQueryString(string.Empty);
        queryParams["key"] = _key;
        queryParams["group_id"] = edupageClassId.ToString();
        queryParams["date"] = timetableDate.ToString("yyyy-MM-dd");
        
        var response = await _httpClient.GetAsync(
            $"timetable-for-date?{queryParams}", cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var responseData = await response.Content.ReadFromJsonAsync<IReadOnlyList<CourseTimetableForGroupResponse>>(cancellationToken);
        return Result.Success(responseData)!;
    }

    public async Task<Result<IReadOnlyList<DateOnly>>> GetWorkingDaysForRange(Guid userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default) {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        var group = await _groupRepository.GetByIdAsync(user.GroupId, cancellationToken);

        var queryParams = HttpUtility.ParseQueryString(string.Empty);
        queryParams["key"] = _key;
        queryParams["group_id"] = group.EdupageClassId.ToString();
        queryParams["language_subgroup"] = user.LanguageSubgroup.ToString(); 
        queryParams["faculty_subgroup"] = user.FacultySubgroup.ToString();
        queryParams["start_date"] = startDate.ToString("yyyy-MM-dd");
        queryParams["end_date"] = endDate.ToString("yyyy-MM-dd");
        
        var response = await _httpClient.GetAsync(
            $"working-days?{queryParams}", cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var responseData = await response.Content.ReadFromJsonAsync<IReadOnlyList<DateOnly>>(cancellationToken);
        return Result.Success(responseData)!;
    }
}
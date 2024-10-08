﻿using System.Net.Http.Json;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkipSmart.Application.Abstractions.Timetable;
using SkipSmart.Application.Attendances.GetTimetableForGroup;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Courses;
using SkipSmart.Domain.Groups;
using SkipSmart.Domain.Users;

namespace SkipSmart.Infrastructure.Timetable;

internal sealed class TimetableService : ITimetableService {
    private static readonly Error TimetableRequestFailed = new(
        "Timetable.TimetableRequestFailed",
        "Failed to acquire timetable data from the external service");
    private static readonly Error WorkingDaysRequestFailed = new(
        "Timetable.WorkingDaysRequestFailed",
        "Failed to acquire working days data from the external service");

    private static readonly Error RequestTimeout = new(
        "Timetable.RequestTimeout",
        "Request to the timetable service has timed out");
    
    private readonly HttpClient _httpClient;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly string _key;
    private readonly string _baseUrl;
    
    public TimetableService(HttpClient httpClient, IGroupRepository groupRepository, IUserRepository userRepository, ICourseRepository courseRepository) {
        _httpClient = httpClient;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _courseRepository = courseRepository;
        _key = Environment.GetEnvironmentVariable("TIMETABLE_API_KEY") 
               ?? throw new ApplicationException("Timetable API key is missing");
        _baseUrl = Environment.GetEnvironmentVariable("TIMETABLE_API_BASE_URL") 
                   ?? throw new ApplicationException("Timetable API base URL is missing");
    }
    
    public async Task<Result<List<TimetableResponse>>> GetTimetableForDate(Guid groupId, 
        DateOnly timetableDate, CancellationToken cancellationToken = default) 
    {
        try {
            var group = await _groupRepository.GetByIdAsync(groupId, cancellationToken);
            var edupageClassId = group.EdupageClassId;
            
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["key"] = _key;
            queryParams["group_id"] = edupageClassId.ToString();
            queryParams["date"] = timetableDate.ToString("yyyy-MM-dd");
            
            var response = await _httpClient.GetAsync(
                $"{_baseUrl}/timetable-for-date?{queryParams}", cancellationToken);
            response.EnsureSuccessStatusCode();
        
            var responseData = await response.Content.ReadFromJsonAsync<List<TimetableResponse>>(cancellationToken);

            if (responseData is null) {
                return Result.Failure<List<TimetableResponse>>(TimetableRequestFailed);
            }
            
            return responseData.ToList();
        } catch (HttpRequestException) {
            return Result.Failure<List<TimetableResponse>>(TimetableRequestFailed);
        } catch (TaskCanceledException) {
            return Result.Failure<List<TimetableResponse>>(RequestTimeout);
        }
    }

    public async Task<Result<IReadOnlyList<DateOnly>>> GetWorkingDaysForRange(Guid userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default) {
        try {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            var group = await _groupRepository.GetByIdAsync(user.GroupId, cancellationToken);
            var courses = (await _courseRepository.GetAllByGroupIdAsync(user.GroupId, cancellationToken)).Select(c => c.CourseName.Value);
            
            if (group.GroupName.Value.StartsWith("L1")) {
                courses = courses.Select(c => {
                    if (c == "English" || c == "French") {
                        return c + user.LanguageSubgroup;
                    }

                    return c;
                });
            }
            
            string coursesParam = string.Join(";", courses);

            var requestBody = new {
                key = _key,
                group_id = group.EdupageClassId.ToString(),
                language_subgroup = user.LanguageSubgroup.ToString(),
                faculty_subgroup = user.FacultySubgroup.ToString(),
                start_date = startDate.ToString("yyyy-MM-dd"),
                end_date = endDate.ToString("yyyy-MM-dd"),
                courses = coursesParam
            };  
        
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/working-days", requestBody, cancellationToken);
            response.EnsureSuccessStatusCode();
        
            var responseData = await response.Content.ReadFromJsonAsync<IReadOnlyList<DateOnly>>(cancellationToken);

            if (responseData is null) {
                return Result.Failure<IReadOnlyList<DateOnly>>(WorkingDaysRequestFailed);
            }
            
            return responseData.ToList();
        } catch (HttpRequestException) {
            return Result.Failure<IReadOnlyList<DateOnly>>(WorkingDaysRequestFailed);
        } catch (TaskCanceledException) {
            return Result.Failure<IReadOnlyList<DateOnly>>(RequestTimeout);
        }
    }
}
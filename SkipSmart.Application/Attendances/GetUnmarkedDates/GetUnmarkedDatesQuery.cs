using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Attendances.GetUnmarkedDates;

public sealed record GetUnmarkedDatesQuery() : IQuery<IReadOnlyList<DateOnly>>;
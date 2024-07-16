using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.MarkedDates.GetUnmarkedDates;

public sealed record GetUnmarkedDatesQuery() : IQuery<IReadOnlyList<DateOnly>>;
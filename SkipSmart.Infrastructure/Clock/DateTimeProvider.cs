using SkipSmart.Application.Abstractions.Clock;

namespace SkipSmart.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider {
    public DateTime UtcNow => DateTime.UtcNow;

    public DateOnly TodayInBaku {
        get {
            DateTime utcNow = DateTime.UtcNow;

            TimeZoneInfo bakuTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Azerbaijan Standard Time");

            DateTime bakuTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, bakuTimeZone);

            DateOnly bakuDate = DateOnly.FromDateTime(bakuTime);

            return bakuDate;
        }
    }

    public DateTime DateTimeInBaku {
        get {
            DateTime utcNow = DateTime.UtcNow;

            TimeZoneInfo bakuTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Azerbaijan Standard Time");

            DateTime bakuTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, bakuTimeZone);

            return bakuTime;
        }
    }

    public DateOnly SemesterStartDate {
        get {
            if (TodayInBaku < new DateOnly(2025, 1, 10)) {
                return new DateOnly(2024, 9, 2);
            }
            else {
                return new DateOnly(2025,1,20);
            }
        }
    }

    public DateOnly FirstSemesterStartDate => new DateOnly(2024, 9, 2);

    public DateOnly SecondSemesterStartDate => new DateOnly(2025, 1, 20);
}
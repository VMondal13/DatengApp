using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Extensions;

public static class DateTimeExtension
{
    public static int CalculateAge(this DateOnly dt)
    {
        return DateOnly.FromDateTime(DateTime.UtcNow).Year - dt.Year;
    }
}

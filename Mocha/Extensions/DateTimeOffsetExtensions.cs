// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Mocha.Extensions;

public static class DateTimeOffsetExtensions
{
    public static DateOnly ToAppDay(this DateTimeOffset instant)
    {
        var local = TimeZoneInfo.ConvertTime(instant, TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague"));
        return DateOnly.FromDateTime(local.DateTime);
    }

}

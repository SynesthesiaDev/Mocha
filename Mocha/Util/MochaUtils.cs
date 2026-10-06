// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using System.Security.Cryptography;
using System.Text;
using Innovative.Geometry;
using Innovative.SolarCalculator;
using QRCoder;
using Synesthesia.Utils.Extensions;

namespace Mocha.Util;

public static class MochaUtils
{
    private const int offset = 60;

    public static double Percentage(int current, int max) => (current.ToDouble() / max.ToDouble()) * 100.0;
    public static double Percentage(double current, int max) => (current / max.ToDouble()) * 100.0;
    public static double Percentage(long current, int max) => (current / max.ToDouble()) * 100.0;

    public static double CalculateSleepDuration(DateTimeOffset start, DateTimeOffset end)
    {
        TimeSpan duration = end - start;
        return duration.TotalHours;
    }

    public static (DateTime, DateTime) GetSolarTimes(double lat, double lon, DateTimeOffset time)
    {
        var solarTimes = new SolarTimes(time, new Angle(lat), new Angle(lon));
        var baseSunrise = solarTimes.Sunrise;
        var baseSunset = solarTimes.Sunset;

        var sunriseStart = baseSunrise.AddMinutes(offset);
        var sunsetStart = baseSunset.AddMinutes(offset);
        return (sunriseStart, sunsetStart);
    }

    public static string HashToken(string rawToken)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(rawToken);
        byte[] hashBytes = SHA256.HashData(inputBytes);

        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }

    public static bool VerifyToken(string rawToken, string storedHash)
    {
        string computedHash = HashToken(rawToken);

        byte[] a = Encoding.UTF8.GetBytes(computedHash);
        byte[] b = Encoding.UTF8.GetBytes(storedHash);

        return CryptographicOperations.FixedTimeEquals(a, b);
    }

    public static string GenerateSvgQrCode(string? textToEncode)
    {
        if (textToEncode == null) return string.Empty;

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(textToEncode, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new SvgQRCode(qrCodeData);

        return qrCode.GetGraphic(5);
    }

    public static double CalculateSunlightHours(
        DateTimeOffset sunrise,
        DateTimeOffset sunset,
        DateTimeOffset sleepStart,
        DateTimeOffset sleepEnd)
    {
        TimeSpan sunriseTod = sunrise.TimeOfDay;
        TimeSpan sunsetTod = sunset.TimeOfDay;

        TimeSpan awakeStart = sleepEnd.TimeOfDay;
        TimeSpan awakeEnd = sleepStart.TimeOfDay;

        if (awakeEnd <= awakeStart)
        {
            awakeEnd += TimeSpan.FromHours(24);
        }

        TimeSpan exposureStart = awakeStart > sunriseTod ? awakeStart : sunriseTod;
        TimeSpan exposureEnd = awakeEnd < sunsetTod ? awakeEnd : sunsetTod;

        return Math.Max(0, (exposureEnd - exposureStart).TotalHours);
    }
}

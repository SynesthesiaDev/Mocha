// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Codon.Codec;
using Mocha.Extensions;
using Mocha.Models.API;
using Mocha.Util;

namespace Mocha.Models;

/// <summary>
/// Represents a single report from Google Health (sent via Mocha android app)
/// </summary>
/// <param name="Steps"></param>
/// <param name="DistanceWalkedKm"></param>
/// <param name="CaloriesBurned"></param>
/// <param name="CardioPoints">One minute of any activity that is 3.0 to 5.9 METs = 1 point, One minute of any activity 6.0 METs or greater = 2 points.
/// This is the percentage of your estimated maximum heart rate. Your maximum heart rate is based on the formula 205.8 - (0.685 x [age])</param>
/// <param name="MoveMinutes"></param>
/// <param name="BpmAverage"></param>
/// <param name="SpO2Average">Peripheral capillary oxygen saturation (Blood Oxygen Percentage)</param>
/// <param name="ReportedTime">Time when the report was collected
/// (note that the time of a collection and time of being received may differ.
/// If the app doesn't currently have access to Wi-Fi or mobile data, it will queue reports up until it has)
/// </param>
public record HealthStatisticsReport(
    long Steps,
    double DistanceWalkedKm,
    double CaloriesBurned,
    int CardioPoints,
    int MoveMinutes,
    int BpmAverage,
    int SpO2Average,
    DateTimeOffset SleepStart,
    DateTimeOffset SleepEnd,
    DateTimeOffset ReportedTime
) : IApiMessage<HealthStatisticsReport>
{
    public static readonly HealthStatisticsReport DEFAULT = new HealthStatisticsReport(0, 0.0, 0.0, 0, 0, 0, 0, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now);

    public static readonly IBinaryCodec<HealthStatisticsReport> BINARY_CODEC = BinaryCodecs
        .For<HealthStatisticsReport>()
        .Field(BinaryCodecs.LONG, c => c.Steps)
        .Field(BinaryCodecs.DOUBLE, c => c.DistanceWalkedKm)
        .Field(BinaryCodecs.DOUBLE, c => c.CaloriesBurned)
        .Field(BinaryCodecs.INT, c => c.CardioPoints)
        .Field(BinaryCodecs.INT, c => c.MoveMinutes)
        .Field(BinaryCodecs.INT, c => c.BpmAverage)
        .Field(BinaryCodecs.INT, c => c.SpO2Average)
        .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY, c => c.SleepStart)
        .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY, c => c.SleepEnd)
        .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY, c => c.ReportedTime)
        .Build((steps, distancewalkedkm, caloriesburned, cardiopoints, moveminutes, bpmaverage, spo2average, sleepstart, sleepend, reportedtime) => new HealthStatisticsReport(steps, distancewalkedkm, caloriesburned, cardiopoints, moveminutes, bpmaverage, spo2average, sleepstart, sleepend, reportedtime));

    public static readonly Codec<HealthStatisticsReport> CODEC = StructCodec
        .For<HealthStatisticsReport>()
        .Field("Steps", Codecs.LONG, c => c.Steps)
        .Field("DistanceWalkedKm", Codecs.DOUBLE, c => c.DistanceWalkedKm)
        .Field("CaloriesBurned", Codecs.DOUBLE, c => c.CaloriesBurned)
        .Field("CardioPoints", Codecs.INT, c => c.CardioPoints)
        .Field("MoveMinutes", Codecs.INT, c => c.MoveMinutes)
        .Field("BpmAverage", Codecs.INT, c => c.BpmAverage)
        .Field("SpO2Average", Codecs.INT, c => c.SpO2Average)
        .Field("SleepStart", ExtraCodecs.DATE_TIME_OFFSET, c => c.SleepStart)
        .Field("SleepEnd", ExtraCodecs.DATE_TIME_OFFSET, c => c.SleepEnd)
        .Field("ReportedTime", ExtraCodecs.DATE_TIME_OFFSET, c => c.ReportedTime)
        .Build((steps, distancewalkedkm, caloriesburned, cardiopoints, moveminutes, bpmaverage, spo2average, sleepstart, sleepend, reportedtime) => new HealthStatisticsReport(steps, distancewalkedkm, caloriesburned, cardiopoints, moveminutes, bpmaverage, spo2average, sleepstart, sleepend, reportedtime));


    public readonly double SleepDuration = MochaUtils.CalculateSleepDuration(SleepStart, SleepEnd).Round(1);

    public static Codec<HealthStatisticsReport> Codec => CODEC;
}

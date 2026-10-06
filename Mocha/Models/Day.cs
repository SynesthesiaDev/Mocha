// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Codon.Optionals;
using Mocha.Util;
using Nocturne.Database.API;

namespace Mocha.Models;

/// <summary>
/// Represents a day. Wow, shocker
/// </summary>
/// <param name="User"></param>
/// <param name="OwnerDiscordId"></param>
/// <param name="Date"></param>
/// <param name="LatestHealthStatistics"></param>
/// <param name="CurrentlyFocusedTask"></param>
public record Day(
    Guid User,
    long OwnerDiscordId,
    DateOnly Date,
    HealthStatisticsReport LatestHealthStatistics,
    DateTimeOffset Sunrise,
    DateTimeOffset Sunset,
    Day.FocusedTask? CurrentlyFocusedTask
)
{
    public static readonly IBinaryCodec<Day> BINARY_CODEC = BinaryCodecs.For<Day>()
        .Field(BinaryCodecs.GUID, c => c.User)
        .Field(BinaryCodecs.LONG, c => c.OwnerDiscordId)
        .Field(ExtraCodecs.DATE_ONLY_BINARY, c => c.Date)
        .Field(HealthStatisticsReport.BINARY_CODEC, c => c.LatestHealthStatistics)
        .Field(FocusedTask.BINARY_CODEC.Optional(), c => Optional.Of<FocusedTask>(c.CurrentlyFocusedTask))
        .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY, c => c.Sunrise)
        .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY, c => c.Sunset)
        .Build((user, ownerdiscordid, date, latesthealthstatistics, currentlyfocusedtask, sunrise, sunset) => new Day(user, ownerdiscordid, date, latesthealthstatistics, sunrise, sunset, currentlyfocusedtask.ToNullableClass()));

    public static readonly NocturneCollection<DateOnly, Day> DATABASE_COLLECTION = Mocha.NOCTURNE_DATABASE.For(
        collectionKey: "days",
        schemaVersion: 0,
        keySerializer: ExtraCodecs.DATE_ONLY_KEY_SERIALIZER,
        NocturneSerializer.FromCodec(BINARY_CODEC),
        migrationStrategy: null
    );

    public record FocusedTask(Guid TaskId, TimeSpan TotalElapsed, DateTimeOffset? LastStartAt, bool Paused)
    {
        public static readonly IBinaryCodec<FocusedTask> BINARY_CODEC = BinaryCodecs.For<FocusedTask>()
            .Field(BinaryCodecs.GUID, c => c.TaskId)
            .Field(ExtraCodecs.TIME_SPAN_BINARY, c => c.TotalElapsed)
            .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY.Optional(), c => Optional.Of<DateTimeOffset>(c.LastStartAt))
            .Field(BinaryCodecs.BOOLEAN, c => c.Paused)
            .Build((taskid, totalelapsed, laststartat, paused) => new FocusedTask(taskid, totalelapsed, laststartat.ToNullableStruct(), paused));
    }

    public bool Is(Day another) => Date == another.Date;
    public bool Is(DateOnly date) => Date == date;

    public double AwakeSunlightHours => MochaUtils.CalculateSunlightHours(Sunrise, Sunset, LatestHealthStatistics.SleepStart, LatestHealthStatistics.SleepEnd);

    public static Day GetOrCreate(DateOnly day, User user)
    {
        return DATABASE_COLLECTION.FindOrAdd(day, date =>
        {
            var config = Mocha.CONFIG.Current.WeatherConfig;
            var solar = MochaUtils.GetSolarTimes(config.Latitude, config.Longitude, date.ToDateTime(TimeOnly.MinValue));
            return new Day(user.Guid, user.DiscordId, date, HealthStatisticsReport.DEFAULT, solar.Item1, solar.Item2, null);
        });
    }
}

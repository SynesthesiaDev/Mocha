// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Codon.Codec;
using Codon.Optionals;
using Mocha.Models.Migrations;
using Mocha.Util;
using Nocturne.Database.API;
using Nocturne.Database.Migrations;

namespace Mocha.Models;

public record MochaTask(
    Guid Id,
    Guid OwningUserId,
    string Title,
    string? Description,
    Guid? ProjectId,
    DateOnly? ScheduledDate,
    DateOnly? DueDate,
    MochaTaskStatus Status,
    EnergyLevel? EnergyLevel,
    DateTimeOffset CreatedAt,
    Guid? RepeatTemplateId,
    bool IsPrivacySensitive,
    DateTimeOffset? CompletedAt
)
{
    public static MochaTask Empty(User user) => new MochaTask
    (
        Guid.NewGuid(),
        user.Guid,
        "Test",
        "this task is just for testing!",
        null,
        null,
        null,
        MochaTaskStatus.Todo,
        Models.EnergyLevel.Medium,
        DateTimeOffset.Now,
        null,
        false,
        null
    );

    public User ResolvedUser => field ??= User.DB_COLLECTION.Find(OwningUserId);

    public static readonly IBinaryCodec<MochaTask> BINARY_CODEC = BinaryCodecs.For<MochaTask>()
        .Field(BinaryCodecs.GUID, c => c.Id)
        .Field(BinaryCodecs.GUID, c => c.OwningUserId)
        .Field(BinaryCodecs.STRING, c => c.Title)
        .Field(BinaryCodecs.STRING.Optional(), c => Optional.Of<string>(c.Description))
        .Field(BinaryCodecs.GUID.Optional(), c => c.ProjectId.ToOptional())
        .Field(ExtraCodecs.DATE_ONLY_BINARY.Optional(), c => Optional.Of<DateOnly>(c.ScheduledDate))
        .Field(ExtraCodecs.DATE_ONLY_BINARY.Optional(), c => Optional.Of<DateOnly>(c.DueDate))
        .Field(BinaryCodecs.Enum<MochaTaskStatus>(), c => c.Status)
        .Field(BinaryCodecs.Enum<EnergyLevel>().Optional(), c => Optional.Of<EnergyLevel>(c.EnergyLevel))
        .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY, c => c.CreatedAt)
        .Field(BinaryCodecs.GUID.Optional(), c => Optional.Of<Guid>(c.RepeatTemplateId))
        .Field(BinaryCodecs.BOOLEAN, c => c.IsPrivacySensitive)
        .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY.Optional(), c => Optional.Of<DateTimeOffset>(c.CompletedAt))
        .Build((id, owninguserid, title, description, projectid, scheduleddate, due, status, energy, createdat, repeat, privacySensitive, completedAt) => new MochaTask(id, owninguserid, title, description.ToNullableClass(), projectid.ToNullableStruct(), scheduleddate.ToNullableStruct(), due.ToNullableStruct(), status, energy.ToNullableStruct(), createdat, repeat.ToNullableStruct(), privacySensitive, completedAt.ToNullableStruct()));

    // Added IsPrivacySensitive
    public static readonly NocturneCollection<Guid, MochaTask> DATABASE_COLLECTION = Mocha.NOCTURNE_DATABASE.For(
        collectionKey: "tasks",
        schemaVersion: 1,
        keySerializer: KeySerializers.GUID,
        NocturneSerializer.FromCodec(BINARY_CODEC),
        migrationStrategy: IMigrationStrategy.Migrations()
            .Add(0, MochaTaskMigrations.MigrateFromV0)
            .Build()
    );
}

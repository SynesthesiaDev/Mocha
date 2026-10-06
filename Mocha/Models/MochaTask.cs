// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Codon.Codec;
using Codon.Optionals;
using Mocha.Util;
using Nocturne.Database.API;

namespace Mocha.Models;

public record MochaTask(
    Guid Id,
    Guid OwningUserId,
    string Title,
    string? Description,
    Guid? ProjectId,
    DateOnly? ScheduledDate,
    DateOnly? DueDate,
    bool Finished,
    EnergyLevel? EnergyLevel,
    DateTimeOffset CreatedAt,
    Guid? RepeatTemplateId
)
{
    public static readonly MochaTask SAMPLE_TASK = new MochaTask
    (
        Guid.NewGuid(),
        Guid.NewGuid(),
        "Test",
        "this task is just for testing!",
        null,
        null,
        null,
        false,
        Models.EnergyLevel.Medium,
        DateTimeOffset.Now,
        null
    );

    public User ResolvedUser => field ??= User.DB_COLLECTION.Find(OwningUserId);
    // public User ResolvedProject => field ??= User.DB_COLLECTION.Find(OwningUserId);

    public static readonly IBinaryCodec<MochaTask> BINARY_CODEC = BinaryCodecs.For<MochaTask>()
        .Field(BinaryCodecs.GUID, c => c.Id)
        .Field(BinaryCodecs.GUID, c => c.OwningUserId)
        .Field(BinaryCodecs.STRING, c => c.Title)
        .Field(BinaryCodecs.STRING.Optional(), c => Optional.Of<string>(c.Description))
        .Field(BinaryCodecs.GUID.Optional(), c => c.ProjectId.ToOptional())
        .Field(ExtraCodecs.DATE_ONLY_BINARY.Optional(), c => Optional.Of<DateOnly>(c.ScheduledDate))
        .Field(ExtraCodecs.DATE_ONLY_BINARY.Optional(), c => Optional.Of<DateOnly>(c.DueDate))
        .Field(BinaryCodecs.BOOLEAN, c => c.Finished)
        .Field(BinaryCodecs.Enum<EnergyLevel>().Optional(), c => Optional.Of<EnergyLevel>(c.EnergyLevel))
        .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY, c => c.CreatedAt)
        .Field(BinaryCodecs.GUID.Optional(), c => Optional.Of<Guid>(c.RepeatTemplateId))
        .Build((id, owninguserid, title, description, projectid, scheduleddate, due, finished, energy, createdat, repeat) => new MochaTask(id, owninguserid, title, description.ToNullableClass(), projectid.ToNullableStruct(), scheduleddate.ToNullableStruct(), due.ToNullableStruct(), finished, energy.ToNullableStruct(), createdat, repeat.ToNullableStruct()));

    public static readonly NocturneCollection<Guid, MochaTask> DATABASE_COLLECTION = Mocha.NOCTURNE_DATABASE.For(
        collectionKey: "tasks",
        schemaVersion: 0,
        keySerializer: KeySerializers.GUID,
        NocturneSerializer.FromCodec(BINARY_CODEC),
        migrationStrategy: null
    );
}

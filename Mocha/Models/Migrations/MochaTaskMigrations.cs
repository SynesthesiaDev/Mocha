// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Codon.Codec;
using Codon.Optionals;
using DotNetty.Buffers;
using Mocha.Util;

namespace Mocha.Models.Migrations;

public static class MochaTaskMigrations
{
    public static IByteBuffer MigrateFromV0(IByteBuffer buffer)
    {
        var v0 = MochaTaskV0.BINARY_CODEC.Read(buffer);
        var v1 = new MochaTaskV1(
            Id: v0.Id,
            OwningUserId: v0.OwningUserId,
            Title: v0.Title,
            Description: v0.Description,
            ProjectId: v0.ProjectId,
            ScheduledDate: v0.ScheduledDate,
            DueDate: v0.DueDate,
            Status: v0.Finished ? MochaTaskStatus.Done : MochaTaskStatus.Todo,
            EnergyLevel: v0.EnergyLevel,
            CreatedAt: v0.CreatedAt,
            RepeatTemplateId: v0.RepeatTemplateId,
            CompletedAt: null,
            IsPrivacySensitive: false
        );

        var newBuffer = Unpooled.Buffer();
        MochaTaskV1.BINARY_CODEC.Write(newBuffer, v1);
        return newBuffer;
    }

    public static IByteBuffer MigrateFromV1(IByteBuffer buffer)
    {
        var v1 = MochaTaskV1.BINARY_CODEC.Read(buffer);
        var v2 = new MochaTask(
            Id: v1.Id,
            OwningUserId: v1.OwningUserId,
            Title: v1.Title,
            Description: v1.Description,
            ProjectId: v1.ProjectId,
            ScheduledDate: v1.ScheduledDate,
            DueDate: v1.DueDate,
            EnergyLevel: v1.EnergyLevel,
            Status: v1.Status,
            CreatedAt: v1.CreatedAt,
            RepeatTemplateId: v1.RepeatTemplateId,
            CompletedAt: null,
            IsPrivacySensitive: false,
            ElapsedTime: TimeSpan.Zero
        );

        var newBuffer = Unpooled.Buffer();
        MochaTask.BINARY_CODEC.Write(newBuffer, v2);
        return newBuffer;
    }

    private record MochaTaskV0(
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
        public static readonly IBinaryCodec<MochaTaskV0> BINARY_CODEC = BinaryCodecs.For<MochaTaskV0>()
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
            .Build((id, owninguserid, title, description, projectid, scheduleddate, due, finished, energy, createdat, repeat) => new MochaTaskV0(id, owninguserid, title, description.ToNullableClass(), projectid.ToNullableStruct(), scheduleddate.ToNullableStruct(), due.ToNullableStruct(), finished, energy.ToNullableStruct(), createdat, repeat.ToNullableStruct()));
    }

    public record MochaTaskV1(
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

        public static readonly IBinaryCodec<MochaTaskV1> BINARY_CODEC = BinaryCodecs.For<MochaTaskV1>()
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
            .Build((id, owninguserid, title, description, projectid, scheduleddate, due, status, energy, createdat, repeat, privacySensitive, completedAt) => new MochaTaskV1(id, owninguserid, title, description.ToNullableClass(), projectid.ToNullableStruct(), scheduleddate.ToNullableStruct(), due.ToNullableStruct(), status, energy.ToNullableStruct(), createdat, repeat.ToNullableStruct(), privacySensitive, completedAt.ToNullableStruct()));
    }
}

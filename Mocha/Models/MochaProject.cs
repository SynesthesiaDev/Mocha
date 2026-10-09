// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Nocturne.Database.API;

namespace Mocha.Models;

public record MochaProject(Guid Id, Guid OwningUserId, string Name, string ColorHex)
{
    public static readonly IBinaryCodec<MochaProject> BINARY_CODEC = BinaryCodecs.For<MochaProject>()
        .Field(BinaryCodecs.GUID, c => c.Id)
        .Field(BinaryCodecs.GUID, c => c.OwningUserId)
        .Field(BinaryCodecs.STRING, c => c.Name)
        .Field(BinaryCodecs.STRING, c => c.ColorHex)
        .Build((id, owninguserid, name, colorhex) => new MochaProject(id, owninguserid, name, colorhex));

    public static readonly NocturneCollection<Guid, MochaProject> DB_COLLECTION = Mocha.NOCTURNE_DATABASE.For(
        collectionKey: "projects",
        schemaVersion: 0,
        keySerializer: KeySerializers.GUID,
        valueSerializer: NocturneSerializer.FromCodec(BINARY_CODEC),
        migrationStrategy: null
    );

    public User ResolvedUser => field ??= User.DB_COLLECTION.Find(OwningUserId);

    public bool IsValidHexCode = ColorHex.StartsWith('#') && ColorHex.Length == 7;

    public static List<MochaProject> FindAllForUser(User user) => [.. DB_COLLECTION.FindAllWhere(p => p.OwningUserId == user.Guid)];
}

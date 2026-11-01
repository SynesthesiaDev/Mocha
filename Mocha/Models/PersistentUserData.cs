// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Codon.Optionals;
using Nocturne.Database.API;

namespace Mocha.Models;

public record PersistentUserData(Guid UserId, Guid? FilterProjectId)
{
    public static readonly IBinaryCodec<PersistentUserData> BINARY_CODEC = BinaryCodecs.For<PersistentUserData>()
        .Field(BinaryCodecs.GUID, c => c.UserId)
        .Field(BinaryCodecs.GUID.Optional(), c => Optional.Of<Guid>(c.FilterProjectId))
        .Build((userid, filterProjectId) => new PersistentUserData(userid, filterProjectId.ToNullableStruct()));

    public static readonly NocturneCollection<Guid, PersistentUserData> DATABASE_COLLECTION = Mocha.NOCTURNE_DATABASE.For(
        collectionKey: "persistent_user_data",
        schemaVersion: 0,
        keySerializer: KeySerializers.GUID,
        valueSerializer: NocturneSerializer.FromCodec(BINARY_CODEC)
    );
}

// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Security.Cryptography;
using Codon.Binary;
using Codon.Codec;
using Mocha.Util;
using Nocturne.Database.API;

namespace Mocha.Models;

public record ApiToken(string Hash, DateTimeOffset Expiration, List<ApiToken.Scope> Scopes, Guid OwningUser)
{
    public static string GetRandom()
    {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(32);

        return Convert.ToBase64String(randomBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }

    public static readonly IBinaryCodec<ApiToken> BINARY_CODEC = BinaryCodecs.For<ApiToken>()
        .Field(BinaryCodecs.STRING, c => c.Hash)
        .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY, c => c.Expiration)
        .Field(BinaryCodecs.Enum<Scope>().List(), c => c.Scopes)
        .Field(BinaryCodecs.GUID, c => c.OwningUser)
        .Build((token, expiration, scopes, owninguser) => new ApiToken(token, expiration, scopes, owninguser));

    public static readonly Codec<ApiToken> CODEC = StructCodec
        .For<ApiToken>()
        .Field("Hash", Codecs.STRING, c => c.Hash)
        .Field("Expiration", ExtraCodecs.DATE_TIME_OFFSET, c => c.Expiration)
        .Field("Scopes", Codecs.Enum<Scope>().List(), c => c.Scopes)
        .Field("OwningUser", Codecs.GUID, c => c.OwningUser)
        .Build((hash, expiration, scopes, owninguser) => new ApiToken(hash, expiration, scopes, owninguser));


    public static readonly NocturneCollection<string, ApiToken> DB_COLLECTION = Mocha.NOCTURNE_DATABASE.For
    (
        collectionKey: "api_tokens",
        schemaVersion: 0,
        keySerializer: KeySerializers.STRING,
        NocturneSerializer.FromCodec(BINARY_CODEC),
        migrationStrategy: null
    );

    public bool HasScope(Scope scope) => !Expired && Scopes.Contains(scope);

    public bool Expired => Expiration <= DateTimeOffset.Now;

    public void Invalidate()
    {
        DB_COLLECTION.Delete(Hash);
    }

    public enum Scope
    {
        ReadTasks,
        WriteTasks,
        ReadHealth,
        WriteHealth,
        MobileLink
    }
}

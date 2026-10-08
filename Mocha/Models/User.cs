// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Codon.Codec;
using Mocha.Util;
using Nocturne.Database.API;

namespace Mocha.Models;

public record User(
    Guid Guid,
    long DiscordId,
    string Username,
    string DisplayName,
    string ProfileImageUrl,
    bool IsAdmin,
    DateTimeOffset LastLogin,
    Settings Settings
)
{
    public static readonly IBinaryCodec<User> BINARY_CODEC = BinaryCodecs.For<User>()
        .Field(BinaryCodecs.GUID, c => c.Guid)
        .Field(BinaryCodecs.LONG, c => c.DiscordId)
        .Field(BinaryCodecs.STRING, c => c.Username)
        .Field(BinaryCodecs.STRING, c => c.DisplayName)
        .Field(BinaryCodecs.STRING, c => c.ProfileImageUrl)
        .Field(BinaryCodecs.BOOLEAN, c => c.IsAdmin)
        .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY, c => c.LastLogin)
        .Field(Settings.BINARY_CODEC.Default(Settings.Default), c => c.Settings)
        .Build((guid, discordid, username, displayname, profileimageurl, isadmin, lastlogin, settings) => new User(guid, discordid, username, displayname, profileimageurl, isadmin, lastlogin, settings));

    public static readonly Codec<User> CODEC = StructCodec.For<User>()
        .Field("Guid", Codecs.GUID, c => c.Guid)
        .Field("DiscordId", Codecs.LONG, c => c.DiscordId)
        .Field("Username", Codecs.STRING, c => c.Username)
        .Field("DisplayName", Codecs.STRING, c => c.DisplayName)
        .Field("ProfileImageUrl", Codecs.STRING, c => c.ProfileImageUrl)
        .Field("IsAdmin", Codecs.BOOLEAN, c => c.IsAdmin)
        .Field("LastLogin", ExtraCodecs.DATE_TIME_OFFSET, c => c.LastLogin)
        .Field("Settings", Settings.CODEC, c => c.Settings)
        .Build((guid, discordid, username, displayname, profileimageurl, isadmin, lastlogin, settings) => new User(guid, discordid, username, displayname, profileimageurl, isadmin, lastlogin, settings));


    public static readonly NocturneCollection<Guid, User> DB_COLLECTION = Mocha.NOCTURNE_DATABASE.For
    (
        collectionKey: "users",
        schemaVersion: 0,
        keySerializer: KeySerializers.GUID,
        valueSerializer: NocturneSerializer.FromCodec(BINARY_CODEC),
        migrationStrategy: null
    );

    public IEnumerable<Day> FindAllDays() => Day.DATABASE_COLLECTION.FindAllWhere(d => d.User == Guid);
}

// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using DotNetty.Buffers;
using Mocha.Util;

namespace Mocha.Models.Migrations;

public static class UserMigrations
{
    public static IByteBuffer MigrateFromV0(IByteBuffer buffer)
    {
        var v0 = UserV0.BINARY_CODEC.Read(buffer);
        var v1 = new User(v0.Guid, v0.DiscordId, v0.Username, v0.DisplayName, v0.ProfileImageUrl, v0.IsAdmin, v0.LastLogin, v0.SettingsV0.MobileLinkSession, v0.SettingsV0.Sessions, false);

        var newBuffer = Unpooled.Buffer();
        User.BINARY_CODEC.Write(newBuffer, v1);

        return newBuffer;
    }

    private record UserV0(
        Guid Guid,
        long DiscordId,
        string Username,
        string DisplayName,
        string ProfileImageUrl,
        bool IsAdmin,
        DateTimeOffset LastLogin,
        SettingsV0 SettingsV0
    )
    {
        public static readonly IBinaryCodec<UserV0> BINARY_CODEC = BinaryCodecs.For<UserV0>()
            .Field(BinaryCodecs.GUID, c => c.Guid)
            .Field(BinaryCodecs.LONG, c => c.DiscordId)
            .Field(BinaryCodecs.STRING, c => c.Username)
            .Field(BinaryCodecs.STRING, c => c.DisplayName)
            .Field(BinaryCodecs.STRING, c => c.ProfileImageUrl)
            .Field(BinaryCodecs.BOOLEAN, c => c.IsAdmin)
            .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY, c => c.LastLogin)
            .Field(SettingsV0.BINARY_CODEC.Default(SettingsV0.Default), c => c.SettingsV0)
            .Build((guid, discordid, username, displayname, profileimageurl, isadmin, lastlogin, settings) => new UserV0(guid, discordid, username, displayname, profileimageurl, isadmin, lastlogin, settings));
    }

}

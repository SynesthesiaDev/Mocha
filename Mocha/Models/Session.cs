// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Codon.Codec;
using Mocha.Util;

namespace Mocha.Models;

public record Session(Guid Id, string DeviceId, string DeviceName, DeviceType DeviceType, string OsVersion, string AppVersion, string IpAddress, DateTimeOffset LastActivity, string SessionTokenHash, Guid UserId)
{

    public static readonly IBinaryCodec<Session> BINARY_CODEC = BinaryCodecs
        .For<Session>()
        .Field(BinaryCodecs.GUID, c => c.Id)
        .Field(BinaryCodecs.STRING, c => c.DeviceId)
        .Field(BinaryCodecs.STRING, c => c.DeviceName)
        .Field(BinaryCodecs.Enum<DeviceType>(), c => c.DeviceType)
        .Field(BinaryCodecs.STRING, c => c.OsVersion)
        .Field(BinaryCodecs.STRING, c => c.AppVersion)
        .Field(BinaryCodecs.STRING, c => c.IpAddress)
        .Field(ExtraCodecs.DATE_TIME_OFFSET_BINARY, c => c.LastActivity)
        .Field(BinaryCodecs.STRING, c => c.SessionTokenHash)
        .Field(BinaryCodecs.GUID, c => c.UserId)
        .Build((id, deviceid, devicename, devicetype, osversion, appversion, ip, lastactivity, sessiontoken, user) => new Session(id, deviceid, devicename, devicetype, osversion, appversion, ip, lastactivity, sessiontoken, user));

    public static readonly Codec<Session> CODEC = StructCodec
        .For<Session>()
        .Field("Id", Codecs.GUID, c => c.Id)
        .Field("DeviceId", Codecs.STRING, c => c.DeviceId)
        .Field("DeviceName", Codecs.STRING, c => c.DeviceName)
        .Field("DeviceType", Codecs.Enum<DeviceType>(), c => c.DeviceType)
        .Field("OsVersion", Codecs.STRING, c => c.OsVersion)
        .Field("AppVersion", Codecs.STRING, c => c.AppVersion)
        .Field("IpAddress", Codecs.STRING, c => c.IpAddress)
        .Field("LastActivity", ExtraCodecs.DATE_TIME_OFFSET, c => c.LastActivity)
        .Field("SessionTokenHash", Codecs.STRING, c => c.SessionTokenHash)
        .Field("UserId", Codecs.GUID, c => c.UserId)
        .Build((id, deviceid, devicename, devicetype, osversion, appversion, ip, lastactivity, sessiontoken, user) => new Session(id, deviceid, devicename, devicetype, osversion, appversion, ip, lastactivity, sessiontoken, user));
}

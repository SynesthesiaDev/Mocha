// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Codec;

namespace Mocha.Models.API.Messages;

public record RegisterMobileSessionRequest(string DeviceId, string DeviceName, DeviceType DeviceType, string OsVersion, string AppVersion)
    : IApiMessage<RegisterMobileSessionRequest>
{
    public static readonly Codec<RegisterMobileSessionRequest> CODEC = StructCodec
        .For<RegisterMobileSessionRequest>()
        .Field("DeviceId", Codecs.STRING, c => c.DeviceId)
        .Field("DeviceName", Codecs.STRING, c => c.DeviceName)
        .Field("DeviceType", Codecs.Enum<DeviceType>(), c => c.DeviceType)
        .Field("OsVersion", Codecs.STRING, c => c.OsVersion)
        .Field("AppVersion", Codecs.STRING, c => c.AppVersion)
        .Build((deviceid, devicename, devicetype, osversion, appversion) => new RegisterMobileSessionRequest(deviceid, devicename, devicetype, osversion, appversion));

    public static Codec<RegisterMobileSessionRequest> Codec => CODEC;
}

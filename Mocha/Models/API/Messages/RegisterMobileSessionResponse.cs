// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Codec;
using Mocha.Util;

namespace Mocha.Models.API.Messages;

public record RegisterMobileSessionResponse(string BearerToken, DateTimeOffset BearerExpiration, Guid SessionId, UserInfo UserInfo)
    : IApiMessage<RegisterMobileSessionResponse>
{
    public static readonly Codec<RegisterMobileSessionResponse> CODEC = StructCodec
        .For<RegisterMobileSessionResponse>()
        .Field("BearerToken", Codecs.STRING, c => c.BearerToken)
        .Field("BearerExpiration", ExtraCodecs.DATE_TIME_OFFSET, c => c.BearerExpiration)
        .Field("SessionId", Codecs.GUID, c => c.SessionId)
        .Field("UserInfo", Models.UserInfo.CODEC, c => c.UserInfo)
        .Build((bearertoken, bearerexpiration, sessionid, user) => new RegisterMobileSessionResponse(bearertoken, bearerexpiration, sessionid, user));


    public static Codec<RegisterMobileSessionResponse> Codec => CODEC;
}

// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Codon.Codec;
using Codon.Optionals;

namespace Mocha.Models;

public record SettingsV0(bool MobileAppLinked, Session? MobileLinkSession, List<Session> Sessions)
{
    public static SettingsV0 Default => new SettingsV0(false, null, []);

    public static readonly IBinaryCodec<SettingsV0> BINARY_CODEC = BinaryCodecs.For<SettingsV0>()
        .Field(BinaryCodecs.BOOLEAN, c => c.MobileAppLinked)
        .Field(Session.BINARY_CODEC.Optional(), c => c.MobileLinkSession.ToOptional())
        .Field(Session.BINARY_CODEC.List(), c => c.Sessions)
        .Build((mobileapplinked, mobile, sessions) => new SettingsV0(mobileapplinked, mobile.ToNullableClass(), sessions));

    public static readonly Codec<SettingsV0> CODEC = StructCodec.For<SettingsV0>()
        .Field("MobileAppLinked", Codecs.BOOLEAN, c => c.MobileAppLinked)
        .Field("MobileLinkSession", Session.CODEC.Optional(), c => c.MobileLinkSession.ToOptional())
        .Field("Sessions", Session.CODEC.List(), c => c.Sessions)
        .Build((mobileapplinked, mobile, sessions) => new SettingsV0(mobileapplinked, mobile.ToNullableClass(), sessions));
}

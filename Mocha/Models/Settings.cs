// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Binary;
using Codon.Codec;
using Codon.Optionals;

namespace Mocha.Models;

public record Settings(bool MobileAppLinked, Session? MobileLinkSession, List<Session> Sessions)
{
    public static Settings Default => new Settings(false, null, []);

    public static readonly IBinaryCodec<Settings> BINARY_CODEC = BinaryCodecs.For<Settings>()
        .Field(BinaryCodecs.BOOLEAN, c => c.MobileAppLinked)
        .Field(Session.BINARY_CODEC.Optional(), c => c.MobileLinkSession.ToOptional())
        .Field(Session.BINARY_CODEC.List(), c => c.Sessions)
        .Build((mobileapplinked, mobile, sessions) => new Settings(mobileapplinked, mobile.ToNullableClass(), sessions));

    public static readonly Codec<Settings> CODEC = StructCodec.For<Settings>()
        .Field("MobileAppLinked", Codecs.BOOLEAN, c => c.MobileAppLinked)
        .Field("MobileLinkSession", Session.CODEC.Optional(), c => c.MobileLinkSession.ToOptional())
        .Field("Sessions", Session.CODEC.List(), c => c.Sessions)
        .Build((mobileapplinked, mobile, sessions) => new Settings(mobileapplinked, mobile.ToNullableClass(), sessions));
}

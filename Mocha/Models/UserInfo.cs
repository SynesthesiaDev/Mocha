// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Codec;

namespace Mocha.Models;

public record UserInfo(string DisplayName, string ProfileImageUrl)
{
    public static readonly Codec<UserInfo> CODEC = StructCodec.For<UserInfo>()
        .Field("DisplayName", Codecs.STRING, c => c.DisplayName)
        .Field("ProfileImageUrl", Codecs.STRING, c => c.ProfileImageUrl)
        .Build((displayname, profileimageurl) => new UserInfo(displayname, profileimageurl));
}

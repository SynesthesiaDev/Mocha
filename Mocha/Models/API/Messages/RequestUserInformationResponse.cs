// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Codec;

namespace Mocha.Models.API.Messages;

public record RequestUserInformationResponse(string Username, string ProfilePictureUrl) : IApiMessage<RequestUserInformationResponse>
{
    public static Codec<RequestUserInformationResponse> Codec { get; } = StructCodec.For<RequestUserInformationResponse>()
        .Field("Username", Codecs.STRING, c => c.Username)
        .Field("ProfilePictureUrl", Codecs.STRING, c => c.ProfilePictureUrl)
        .Build((username, pfpUrl) => new RequestUserInformationResponse(username, pfpUrl));
}

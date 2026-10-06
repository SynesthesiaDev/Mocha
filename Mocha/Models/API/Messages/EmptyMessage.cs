// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Codec;

namespace Mocha.Models.API.Messages;

public record EmptyMessage : IApiMessage<EmptyMessage>
{
    public static readonly EmptyMessage INSTANCE = new EmptyMessage();
    public static readonly Codec<EmptyMessage> CODEC = new StructCodec<EmptyMessage>.StructCodec0P<EmptyMessage>(() => INSTANCE);

    public static Codec<EmptyMessage> Codec => CODEC;
}

// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using Codon.Codec;
using Codon.Codec.Json;
using Codon.Optionals;

namespace Mocha.Models.API;

public record ApiResponse<T>(string? Error, T? Message) where T : class, IApiMessage<T>
{
    public static readonly StructCodec<ApiResponse<T>> CODEC = StructCodec.For<ApiResponse<T>>()
        .Field("Error", Codecs.STRING.Optional(), r => r.Error.ToOptional())
        .Field("Message", T.Codec.Optional(), r => r.Message.ToOptional())
        .Build((error, message) => new ApiResponse<T>(error.ToNullableClass(), message.ToNullableClass()));

    public static ApiResponse<T> Decode(string message) => CODEC.Decode(JsonTranscoder.INSTANCE, message.ToJson());
}

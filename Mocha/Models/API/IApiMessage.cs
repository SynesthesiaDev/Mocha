// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Codec;

namespace Mocha.Models.API;

public interface IApiMessage<T> where T : IApiMessage<T>
{
    abstract static Codec<T> Codec { get; }
}

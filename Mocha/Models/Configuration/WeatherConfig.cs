// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Codec;
using Codon.Optionals;

namespace Mocha.Models.Configuration;

public record WeatherConfig(double Latitude, double Longitude, string? VisualCrossingApiKey)
{
    public static readonly Codec<WeatherConfig> CODEC = StructCodec.For<WeatherConfig>()
        .Field("Latitude", Codecs.DOUBLE, c => c.Latitude)
        .Field("Longitude", Codecs.DOUBLE, c => c.Longitude)
        .Field("VisualCrossingApiKey", Codecs.STRING.Optional(), c => Optional.Of<string>(c.VisualCrossingApiKey))
        .Build((latitude, longitude, visualCrossingApiKey) => new WeatherConfig(latitude, longitude, visualCrossingApiKey.ToNullableClass()));

    public static readonly WeatherConfig DEFAULT = new WeatherConfig(0.0, 0.0, null);

}

using Codon.Codec;
using Codon.Optionals;
using SynesthesiaDev.ConfigLibrary;

namespace Mocha.Models.Configuration;

public record Config(
    string Domain,
    int Port,
    WeatherConfig WeatherConfig,
    DiscordAuthSettings? DiscordAuthSettings
) : IConfigModel
{
    public static readonly Codec<Config> CODEC = StructCodec.For<Config>()
        .Field("Domain", Codecs.STRING, c => c.Domain)
        .Field("Port", Codecs.INT, c => c.Port)
        .Field("WeatherConfig", WeatherConfig.CODEC, c => c.WeatherConfig)
        .Field("DiscordAuthSettings", DiscordAuthSettings.CODEC.Optional(), c => Optional.Of<DiscordAuthSettings>(c.DiscordAuthSettings))
        .Build((domain, port, weather, discord) => new Config(domain, port, weather, discord.ToNullableClass()));

    public static readonly Config DEFAULT = new Config("0.0.0.0", 80, WeatherConfig.DEFAULT, DiscordAuthSettings.DEFAULT);
}

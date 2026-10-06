using Codon.Codec;

namespace Mocha.Models.Configuration;

public record DiscordAuthSettings(
    long AppId,
    long ClientId,
    string Secret
)
{
    public static readonly DiscordAuthSettings DEFAULT = new
    (
        1234567891011121314,
        1234567891011121314,
        "your secret here :3"
    );

    public static readonly StructCodec<DiscordAuthSettings> CODEC = StructCodec.For<DiscordAuthSettings>()
        .Field("AppId", Codecs.LONG, o => o.AppId)
        .Field("ClientId", Codecs.LONG, o => o.ClientId)
        .Field("Secret", Codecs.STRING, o => o.Secret)
        .Build((appId, clientId, secret) => new DiscordAuthSettings(appId, clientId, secret));
}

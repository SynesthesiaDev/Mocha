using Discord;
using Discord.Rest;
using Mocha.Models;
using Serilog;

namespace Mocha.Integrations;

public static class DiscordIntegration
{
    private static readonly DiscordRestClient discord_client = new DiscordRestClient();

    public static async Task<IntermediaryDiscordUser?> GetData(string accessToken)
    {
        try
        {
            await discord_client.LoginAsync(TokenType.Bearer, accessToken);
            var user = await discord_client.GetCurrentUserAsync();

            var id = user.Id;
            var username = user.Username;
            var profileImageUrl = user.GetAvatarUrl();
            var displayName = user.GlobalName;

            return new IntermediaryDiscordUser(id, username, displayName, profileImageUrl);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to fetch Discord data");
            return null;
        }
    }
}

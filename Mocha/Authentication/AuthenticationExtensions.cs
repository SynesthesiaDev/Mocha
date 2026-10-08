// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Security.Claims;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Mocha.Integrations;
using Mocha.Models;

namespace Mocha.Authentication;

public static class AuthenticationExtensions
{
    public const string DISCORD_IDENTIFIER = "urn:discord:id";
    public const string GUID_IDENTIFIER = "urn:mocha:guid";

    public static IServiceCollection AddDiscordAuthentication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = DiscordAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options => { options.LoginPath = "/login"; })
            .AddDiscord(options =>
            {
                options.ClientId = Mocha.CONFIG.Current.DiscordAuthSettings!.ClientId.ToString();
                options.ClientSecret = Mocha.CONFIG.Current.DiscordAuthSettings!.Secret;

                options.SaveTokens = true;
                options.Scope.Add("identify");
                options.ClaimActions.MapJsonKey(DISCORD_IDENTIFIER, "id");

                options.CorrelationCookie.SameSite = SameSiteMode.Lax;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

                options.Events.OnCreatingTicket = OnDiscordTicketCreated;
            });
        return serviceCollection;
    }

    private static async Task OnDiscordTicketCreated(OAuthCreatingTicketContext context)
    {
        var intermediary = await DiscordIntegration.GetData(context.AccessToken!);
        if (intermediary == null) throw new InvalidDataException("data is null");

        if (context.Principal?.Identity is not ClaimsIdentity identity) return;

        var user = User.DB_COLLECTION.FindFirstOrNull(u => u.DiscordId == (long)intermediary.Id);
        if (user == null)
        {
            var guid = Guid.NewGuid();
            user = new User(guid, (long)intermediary.Id, intermediary.Username, intermediary.DisplayName, intermediary.ProfileImageUrl, false, DateTimeOffset.Now, null, [], false);
        }

        User.DB_COLLECTION.Insert(user.Guid, user with
        {
            DisplayName = intermediary.DisplayName,
            ProfileImageUrl = intermediary.ProfileImageUrl,
            Username = intermediary.Username,
            LastLogin = DateTimeOffset.UtcNow
        });

        identity.AddClaim(new Claim(DISCORD_IDENTIFIER, user.DiscordId.ToString()));
        identity.AddClaim(new Claim(GUID_IDENTIFIER, user.Guid.ToString()));
    }
}

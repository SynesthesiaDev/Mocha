// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Mocha.Authentication;

namespace Mocha.Extensions;

public static class AuthenticationStateExtensions
{
    public static async Task<string> GetBodyString(this HttpRequest request)
    {
        var bodyStream = new StreamReader(request.Body);
        var bodyText = await bodyStream.ReadToEndAsync();
        return bodyText;
    }

    extension(double value)
    {
        public double Round(int digits = 2)
        {
            return Math.Round(value, digits);
        }
    }

    extension(AuthenticationState authState)
    {
        public long GetDiscordId()
        {
            var discordIdString = authState.User.FindFirstValue(AuthenticationExtensions.DISCORD_IDENTIFIER);
            return Convert.ToInt64(discordIdString);
        }

        public Guid? GetGuid()
        {
            var guidString = authState.User.FindFirstValue(AuthenticationExtensions.GUID_IDENTIFIER);
            if (guidString == null) return null;
            return Guid.Parse(guidString);
        }
    }
}

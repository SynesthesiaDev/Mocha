// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using Mocha.Extensions;
using Mocha.Models;
using Mocha.Models.API;
using Mocha.Models.API.Messages;
using Mocha.Util;

namespace Mocha.Endpoints;

public static class MobileAppLinkEndpoint
{
    public static void MapMobileLinkEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/requestUserInformation", async context =>
        {
            var request = await RequestHelper.DecodeRequest(context, EmptyMessage.Codec);
            if(request == null) return;

            if (request.AssertScope(ApiToken.Scope.MobileLink)) return;

            var user = request.ResolveUser()!;
            var response = new ApiResponse<RequestUserInformationResponse>(null, new RequestUserInformationResponse(user.DisplayName, user.ProfileImageUrl));

            context.Response.WriteResponse(response);
        });

        app.MapPost("/api/registerMobileSessionRequest", async context =>
        {
            var request = await RequestHelper.DecodeRequest(context, RegisterMobileSessionRequest.Codec);
            if(request == null) return;

            if (request.AssertScope(ApiToken.Scope.MobileLink)) return;

            var user = request.ResolveUser()!;

            request.InvalidateToken();
            var decoded = request.Decoded!;

            var bearerRaw = ApiToken.GetRandom();
            var expiration = DateTimeOffset.Now.AddYears(1);
            var hash = MochaUtils.HashToken(bearerRaw);
            var bearer = new ApiToken(hash, expiration, [ApiToken.Scope.WriteHealth], user.Guid);
            var sessionId = Guid.NewGuid();

            var ip = context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? string.Empty;

            var existing = user.Sessions.FirstOrDefault(s => s.DeviceId == decoded.DeviceId);
            if (existing != null)
            {
                user.Sessions.Remove(existing);
            }

            var session = new Session(sessionId, decoded.DeviceId, decoded.DeviceName, decoded.DeviceType, decoded.OsVersion, decoded.AppVersion, ip, DateTimeOffset.Now, hash, user.Guid);
            user.Sessions.Add(session);
            user = user with {MobileLinkSession = session };

            ApiToken.DB_COLLECTION.Insert(hash, bearer);
            User.DB_COLLECTION.Insert(user.Guid, user);

            var response = new ApiResponse<RegisterMobileSessionResponse>(null, new RegisterMobileSessionResponse(bearerRaw, expiration, sessionId, user));

            context.Response.WriteResponse(response);
        });
    }
}

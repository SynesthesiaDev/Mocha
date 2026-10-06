// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Mocha.Extensions;
using Mocha.Models;
using Mocha.Models.API;
using Serilog;

namespace Mocha.Endpoints;

public static class MobileHealthSyncEndpoint
{
    public static void MapMobileHealthSyncEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/mobile/sync", async context =>
        {
            var request = await RequestHelper.DecodeRequest(context, HealthStatisticsReport.Codec);
            if(request == null) return;

            if (request.AssertScope(ApiToken.Scope.WriteHealth)) return;
            if (request.AssertDecodedNotNull()) return;

            var user = request.ResolveUser()!;
            var day = Day.GetOrCreate(DateTimeOffset.UtcNow.ToAppDay(), user);
            var newDay = day with { LatestHealthStatistics = request.Decoded! };

            Day.DATABASE_COLLECTION.Insert(newDay.Date, newDay);
            Log.Information("Mobile Sync successful: {thing}", request.Decoded!);
        });
    }
}

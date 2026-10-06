// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Codon.Codec;
using Codon.Codec.Json;
using Mocha.Extensions;
using Mocha.Models.API.Messages;
using Mocha.Util;

namespace Mocha.Models.API;

public static class RequestHelper
{
    public static async Task<RequestData<T>?> DecodeRequest<T>(HttpContext context, Codec<T> codec) where T : class, IApiMessage<T>
    {
        var request = context.Request;

        var body = await request.GetBodyString();
        var token = request.Headers.Authorization.FirstOrDefault();
        ApiToken? apiToken = null;

        if (token != null)
        {
            var hash = MochaUtils.HashToken(token);
            apiToken = ApiToken.DB_COLLECTION.FindOrNull(hash);
        }

        T? apiMessage = null;

        try
        {
            if (typeof(T) != typeof(EmptyMessage))
            {
                apiMessage = codec.Decode(JsonTranscoder.INSTANCE, body.ToJson());
            }
        }
        catch (Exception e)
        {
            var response = new ApiResponse<T>($"Failed to decode payload: {e.Message}", null);
            context.Response.WriteResponse(response, StatusCodes.Status200OK);
            return null;
        }


        return new RequestData<T>(body, context, apiMessage, apiToken);
    }

    public record RequestData<T>(string Body, HttpContext Context, T? Decoded, ApiToken? Token) where T : class, IApiMessage<T>
    {
        public Guid? User => Token?.OwningUser;

        public User? ResolveUser() => User == null ? null : Models.User.DB_COLLECTION.FindOrNull(User.Value);

        public bool HasScope(ApiToken.Scope scope) => Token?.HasScope(scope) ?? false;

        public void InvalidateToken() => Token?.Invalidate();

        public bool AssertDecodedNotNull()
        {
            if (Decoded != null) return false;

            var response = new ApiResponse<T>($"Missing or malformed payload (expecting {typeof(T).Name} but got null)", null);
            Context.Response.WriteResponse(response, StatusCodes.Status400BadRequest);
            return true;
        }

        public bool AssertScope(ApiToken.Scope scope)
        {
            if (HasScope(scope)) return false;

            var response = new ApiResponse<T>($"Missing authentication token, missing authentication scope ({scope}), or expired authentication token", null);
            Context.Response.WriteResponse(response, StatusCodes.Status403Forbidden);
            return true;
        }
    }
}

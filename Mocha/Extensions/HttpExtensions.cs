// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text;
using System.Text.Json;
using Codon.Codec;
using Codon.Codec.Json;
using Mocha.Models.API;

namespace Mocha.Extensions;

public static class HttpExtensions
{
    extension(HttpResponse response)
    {
        public void WriteString(string text)
        {
            var data = Encoding.UTF8.GetBytes(text);
            response.Body.WriteAsync(data, 0, data.Length);
        }

        public void WriteJson(JsonElement json)
        {
            response.ContentType = "application/json";
            var data = Encoding.UTF8.GetBytes(json.ToStringMinified());
            response.Body.WriteAsync(data, 0, data.Length);
        }

        public void WriteResponse<T>(ApiResponse<T> value, int code = StatusCodes.Status200OK) where T : class, IApiMessage<T>
        {
            var encoded = ApiResponse<T>.CODEC.Encode(JsonTranscoder.INSTANCE, value);
            response.StatusCode = code;
            response.WriteJson(encoded);
        }
    }
}

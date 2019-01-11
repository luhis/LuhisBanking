using System;
using System.Net.Http;

namespace TrueLayerAccess
{
    public static class ResponseExtensions
    {
        public static void ThrowIfNotSuccess(this HttpResponseMessage m)
        {
            if (!m.IsSuccessStatusCode)
            {
                throw new ApplicationException($"invalid status code: {m.StatusCode}");
            }
        }
    }
}
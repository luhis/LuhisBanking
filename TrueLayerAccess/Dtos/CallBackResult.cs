using System.Collections.Generic;

namespace TrueLayerAccess.Dtos
{
    public class CallBackResult
    {
        public CallBackResult(string code, IReadOnlyList<string> scopes)
        {
            Code = code;
            Scopes = scopes;
        }

        public string Code { get; }

        public IReadOnlyList<string> Scopes { get; }
    }
}
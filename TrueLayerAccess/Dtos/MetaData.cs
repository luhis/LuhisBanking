using System.Collections.Generic;

namespace TrueLayerAccess.Dtos
{
    public class MetaData
    {
        public string client_id { get; set; }
        public string credentials_id { get; set; }
        public Provider provider { get; set; }
        public List<string> scopes { get; set; }
    }
}
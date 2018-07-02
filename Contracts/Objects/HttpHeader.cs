using System.Runtime.Serialization;

namespace FabricWCF.Common.Objects
{
    [DataContract]
    public class HttpHeader
    {
        
        public string Name { get; set; }

        
        public string Value { get; set; }
    }
}
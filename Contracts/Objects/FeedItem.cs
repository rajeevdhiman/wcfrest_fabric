using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace FabricWcf.ServiceContracts.Objects
{
    [DataContract]
    public class FeedItem
    {
        [DataMember]
        public IEnumerable<string> Authors { get; set; }

        [DataMember]
        public string BaseUri { get; set; }

        [DataMember]
        public IEnumerable<string> Categories { get; set; }

        [DataMember]
        public IEnumerable<string> Contributors { get; set; }

        [DataMember]
        public string Copyright { get; set; }

        [DataMember]
        public IEnumerable<string> ElementExtensions { get; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public IEnumerable<string> Links { get; set; }

        [DataMember]
        public DateTime PublishDate { get; set; }

        [DataMember]
        public string Summary { get; set; }

        [DataMember]
        public string Title { get; set; }
    }
}

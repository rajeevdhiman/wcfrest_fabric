using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FabricWCF.Common.Objects
{
    [DataContract]
    public class FeedItem
    {
        public FeedItem()
        {
            Authors = new List<string>();
            Categories = new List<string>();
            Contributors = new List<string>();
            ElementExtensions = new List<string>();
            Links = new List<string>();
        }

        [DataMember]
        public IList<string> Authors { get; set; }

        [DataMember]
        public string BaseUri { get; set; }

        [DataMember]
        public IList<string> Categories { get; set; }

        [DataMember]
        public IList<string> Contributors { get; set; }

        [DataMember]
        public string Copyright { get; set; }

        [DataMember]
        public IList<string> ElementExtensions { get; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public IList<string> Links { get; set; }

        [DataMember]
        public DateTime PublishDate { get; set; }

        [DataMember]
        public string Summary { get; set; }

        [DataMember]
        public string Title { get; set; }
    }
}
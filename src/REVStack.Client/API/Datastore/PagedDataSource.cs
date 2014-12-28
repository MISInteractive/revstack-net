using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RevStack.Client.API.Datastore
{
    [DataContract]
    public class PagedDataSource<T> where T : new()
    {
        [DataMember(Name="_data")]
        public IQueryable<T> Data { get; set; }
        [DataMember(Name="pagination")]
        public Pagination Pagination { get; set; }
    }

    [DataContract]
    public class Pagination
    {
        [DataMember(Name = "page")]
        public int Page { get; set; }
        [DataMember(Name = "limit")]
        public int Limit { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "page_count")]
        public int PageCount { get; set; }
        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }
    }
}

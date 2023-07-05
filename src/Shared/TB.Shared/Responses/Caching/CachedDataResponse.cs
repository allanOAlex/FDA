using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Responses.Common;

namespace TB.Shared.Responses.Caching
{
    public record CachedDataResponse<T> : Response
    {
        public T? Data { get; set; }
        public ICollection<T>? DataList { get; set; }
    }
}

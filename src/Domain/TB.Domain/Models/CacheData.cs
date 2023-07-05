using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB.Domain.Models
{
    public class CacheData<T>
    {
        public CacheData()
        {
                
        }

        public string? Key { get; set; }
        public T? Data { get; set; }
        public ICollection<T>? DataList { get; set; }


    }
}

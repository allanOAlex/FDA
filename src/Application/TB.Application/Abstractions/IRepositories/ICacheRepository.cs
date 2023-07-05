using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Domain.Models;

namespace TB.Application.Abstractions.IRepositories
{
    public interface ICacheRepository
    {
        Task<IQueryable<object>> GetAsync(string key);
        Task<bool> SetAsync(string key, object data);


    }
}

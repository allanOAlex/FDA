using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TB.Application.Abstractions.IRepositories;
using TB.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TB.Infrastructure.Implementations.Repositories
{
    internal sealed class CacheRepository : ICacheRepository
    {
        private readonly IMemoryCache cache;

        public CacheRepository(IMemoryCache Cache)
        {
            cache = Cache;
        }

        

        public async Task<IQueryable<object>> GetAsync(string key)
        {
            try
            {
                if (cache.TryGetValue(key, out object cachedData))
                {
                    try
                    {
                        return (IQueryable<object>)cachedData;
                    }
                    catch (InvalidCastException)
                    {
                        throw;
                    }
                }

                return await Task.FromResult(Enumerable.Empty<object>().AsQueryable());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<bool> SetAsync(string key, object data)
        {
            try
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(600))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(1200));

                if (data is IEnumerable<object> enumerableData)
                {
                    var queryableData = enumerableData.AsQueryable();

                    cache.Set(key, queryableData, cacheOptions);
                }
                else
                {
                    cache.Set(key, data, cacheOptions);
                }

                return Task.FromResult(true);

            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}

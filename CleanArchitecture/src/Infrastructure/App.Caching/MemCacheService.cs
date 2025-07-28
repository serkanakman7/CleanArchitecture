using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Contracts.Caching;
using Enyim.Caching;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;

namespace App.Caching
{
    public class MemCacheService : ICacheService
    {
        private readonly IMemcachedClient _memcachedClient;

        public MemCacheService(IMemcachedClient memcachedClient)
        {
            _memcachedClient = memcachedClient;
        }

        public async Task AddAsync<T>(string cacheKey, T value, TimeSpan exprTimeSpan)
        {
            await _memcachedClient.SetAsync(cacheKey, value, exprTimeSpan);
        }

        public async Task<T?> GetAsync<T>(string cacheKey)
        {
            return await _memcachedClient.GetValueAsync<T>(cacheKey);
        }

        public async Task RemoveAsync(string cacheKey)
        {
            await _memcachedClient.RemoveAsync(cacheKey);
        }
    }
}

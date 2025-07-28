using App.Application.Contracts.Caching;
using App.Caching;
using App.Domain.Options;

namespace App.API.Extensions
{
    public static class CachingExtension
    {
        public static IServiceCollection AddCachingExt(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddMemoryCache();
            //services.AddSingleton<ICacheService, CacheService>();

            var enyimMemCachedOption = configuration.GetSection(nameof(EnyimMemcachedOption)).Get<EnyimMemcachedOption>();

            services.AddEnyimMemcached(options =>
            {
                options.AddServer(enyimMemCachedOption.Address, enyimMemCachedOption.Port);
            });
            services.AddSingleton<ICacheService, MemCacheService>();

            return services;
        }
    }
}

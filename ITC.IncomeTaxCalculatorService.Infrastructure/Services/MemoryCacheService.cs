using ITC.IncomeTaxCalculatorService.ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ITC.IncomeTaxCalculatorService.Infrastructure.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _absoluteExpiration = TimeSpan.Zero;
        private readonly ILogger _logger;

        public MemoryCacheService(IMemoryCache memoryCache, IConfiguration config, ILogger<MemoryCacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;

            if (int.TryParse(config["MemoryCacheExpiration"], out var expiration))
            {
                _absoluteExpiration = TimeSpan.FromMinutes(expiration);
            }
            else
            {
                _logger.LogWarning($"Failed to parse memory cache expiration configuration value from \"{config["MemoryCacheExpiration"]}\"");
            }
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            if (_absoluteExpiration == TimeSpan.Zero)
            {
                value = default;
                return false;
            }

            return _memoryCache.TryGetValue(key, out value);
        }

        public void SetValue<T>(string key, T value)
        {
            if (_absoluteExpiration == TimeSpan.Zero)
                return;

            _memoryCache.Set(key, value, _absoluteExpiration);
        }
    }
}

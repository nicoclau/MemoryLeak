using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MemoryLeak
{
    public class MyWorker : BackgroundService
    {        
        private readonly IMemoryCache _memoryCache;

        private readonly ILogger<MyWorker> _logger;

        public MyWorker(ILogger<MyWorker> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
                
                var cacheEntryOptions = new MemoryCacheEntryOptions();
                
                _memoryCache.Set(Guid.NewGuid(), new byte[10000], cacheEntryOptions);
            }
        }
    }
}
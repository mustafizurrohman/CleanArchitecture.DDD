using Microsoft.Extensions.Caching.Memory;
using System.Threading;

namespace CleanArchitecture.DDD.Application.Services.ScrutorDemo.ServiceDecoration;

public class DataServiceCached : IDataService
{
    private MemoryCacheEntryOptions MemoryCacheEntryOptions { get; }
    private IDataService DataService { get; }
    private IMemoryCache MemoryCache { get; }

    public DataServiceCached(IMemoryCache memoryCache, IDataService dataService)
    {
        MemoryCacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(15));

        MemoryCache = Guard.Against.Null(memoryCache, nameof(memoryCache));
        DataService = Guard.Against.Null(dataService, nameof(dataService));
    }

    public async Task<IEnumerable<DemoData>> GetDemoData(int num)
    {
        var cacheKey = num.ToString();

        var demoData = new List<DemoData>();
        if (!MemoryCache.TryGetValue(cacheKey, out demoData!))
        {
            demoData = (await DataService.GetDemoData(num)).ToList();

            var demoDataCached = new List<DemoData>();

            foreach (var data in demoData)
            {
                data.Cached = true;
                demoDataCached.Add(data);
            }

            SetMemoryCache(cacheKey, demoDataCached);
        }

        return demoData;
    }

    private void SetMemoryCache(string cacheKey, object value)
    {
        MemoryCache.Set(cacheKey, value, MemoryCacheEntryOptions);
    }
}
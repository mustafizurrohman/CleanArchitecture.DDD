using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.DDD.Application.Services.ScrutorDemo.ServiceDecoration;

public class DataServiceCached : IDataService
{
    private MemoryCacheEntryOptions MemoryCacheEntryOptions { get; }
    private IDataService DataService { get; }
    private IMemoryCache MemoryCache { get; }

    public DataServiceCached(IMemoryCache memoryCache, IDataService dataService)
    {
        var timeSpan = TimeSpan.FromSeconds(15);
        
        MemoryCacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(15));

        MemoryCache = Guard.Against.Null(memoryCache, nameof(memoryCache));
        DataService = Guard.Against.Null(dataService, nameof(dataService));

        Log.Information($"DECORATED DataService CACHED- Initialized service... Cache valid until {DateTime.Now.Add(timeSpan).ToLocalDateTime()}");
    }

    public async Task<IEnumerable<DemoData>> GetDemoDataAsync(int num)
    {
        // Trivial implementation- Not suitable for production 
        var cacheKey = "DemoData";

        if (MemoryCache.TryGetValue(cacheKey, out List<DemoData>? demoData)) {
            Log.Information("Returning cached data ...");
            return demoData!;
        }

        demoData = (await DataService.GetDemoDataAsync(num)).ToList();
        
        var demoDataCached = demoData
            .Select(data => data.GetCachedVersion())
            .ToList();
        
        SetMemoryCache(cacheKey, demoDataCached);
        Log.Information("Returning real data ...");

        return demoData;
    }

    private void SetMemoryCache(string cacheKey, object value)
    {
        MemoryCache.Set(cacheKey, value, MemoryCacheEntryOptions);
    }
}
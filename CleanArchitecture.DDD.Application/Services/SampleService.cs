using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.DDD.Application.Services;

public class SampleService : ISampleService
{
    private readonly HttpClient _httpClient;

    public SampleService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task TestHttpClient()
    {

    }
}
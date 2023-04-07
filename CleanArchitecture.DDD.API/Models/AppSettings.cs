using Newtonsoft.Json;

namespace CleanArchitecture.DDD.API.Models;

// AppSettings myDeserializedClass = JsonConvert.DeserializeObject<AppSettings>(myJsonResponse);
public class AppSettings
{
    public Logging Logging { get; set; }
    public Serilog Serilog { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public HealthChecksUI HealthChecksUI { get; set; }
}

public class Args
{
    public string path { get; set; }
    public string rollingInterval { get; set; }
    public string outputTemplate { get; set; }
    public List<string> Enrich { get; set; }
}

public class ConnectionStrings
{
    public string DDD_Db { get; set; }
    public string HangfireDb { get; set; }
    public string Seq { get; set; }
}

public class HealthCheck
{
    public string Name { get; set; }
    public string Uri { get; set; }
}

public class HealthChecksUI
{
    public List<HealthCheck> HealthChecks { get; set; }
    public int EvaluationTimeOnSeconds { get; set; }
    public int MinimumSecondsBetweenFailureNotifications { get; set; }
}

public class Logging
{
    public LogLevel LogLevel { get; set; }
}


public class MinimumLevel
{
    public string Default { get; set; }
    public Override Override { get; set; }
}

public class Override
{
    public string Microsoft { get; set; }
    public string System { get; set; }
}

public class Serilog
{
    public List<object> Using { get; set; }
    public MinimumLevel MinimumLevel { get; set; }
    public List<string> Enrich { get; set; }
    public List<WriteTo> WriteTo { get; set; }
}

public class WriteTo
{
    public string Name { get; set; }
    public Args Args { get; set; }
}




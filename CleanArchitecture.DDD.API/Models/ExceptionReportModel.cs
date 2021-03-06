using System.Diagnostics.CodeAnalysis;

namespace CleanArchitecture.DDD.API.Models;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ExceptionReportModel
{
    public DateTime ExceptionTime { get; }
    public Exception? Exception { get; }
    public string SupportCode { get; }

    public ExceptionReportModel(Exception ex, string supportCode)
    {
        ExceptionTime = DateTime.Now;
        Exception = ex;
        SupportCode = supportCode;
    }
}
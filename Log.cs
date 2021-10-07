internal static partial class Log
{
    [LoggerMessage(EventId = 1023, Level = LogLevel.Critical, Message = "log #{LogNumber}")]
    public static partial void LogCritical_1_Arg_Generated(ILogger logger, long logNumber);

    [LoggerMessage(EventId = 22, Level = LogLevel.Information, Message = "Hello World!")]
    public static partial void HelloWorld(ILogger logger);

    [LoggerMessage(EventId = 10, Level = LogLevel.Information, Message = "Got weather forecast!")]
    public static partial void GotWeatherForecast(ILogger logger);

    [LoggerMessage(EventId = 0, EventName = "GotWeatherForecastWithSummary", Level = LogLevel.Information, Message = "Got weather forecast, and it's {Summary}!")]
    public static partial void GotWeatherForecastWithSummary(ILogger logger, string summary);
}
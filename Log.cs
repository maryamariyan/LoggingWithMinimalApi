internal static partial class Log
{
    [LoggerMessage(EventId = 22, Level = LogLevel.Information, Message = "Hello World!")]
    public static partial void HelloWorld(ILogger logger);

    #region Sample2
    [LoggerMessage(EventId = 1010, Level = LogLevel.Information, Message = "Resident {Name} aged {Age} years old from {Hometown} moved here {YearsSince} years with {NumSuitcases} suitcases!")]
    public static partial void NewResidentInformationGenerated(ILogger logger,
        string name, int age, string hometown, int yearsSince, long numSuitcases);

    #endregion

    #region SkippedBoilerplateCode
    // skip all boilerplate code below by using source generated logs via LoggerMessage attribute
    public static void NewResidentInformationUserDefined(ILogger logger, string name, int age, string hometown, int yearsSince, long numSuitcases) =>
        s_userDefinedPerformantLog(logger, name, age, hometown, yearsSince, numSuitcases, null);

    private static readonly Action<ILogger, string, int, string, int, long, Exception?> s_userDefinedPerformantLog =
        LoggerMessage.Define<string, int, string, int, long>(
            LogLevel.Information,
            new EventId(1012),
            "Resident {Name} aged {Age} years old from {Hometown} moved here {YearsSince} years with {NumSuitcases} suitcases!");
#endregion

    #region MoreLoggerMessageSamples
    [LoggerMessage(EventId = 111, Level = LogLevel.Information, Message = "Logged in #{ElapsedTime}")]
    public static partial void LoggedInElapsedTime(ILogger logger, string elapsedTime);

    [LoggerMessage(EventId = 1023, Level = LogLevel.Critical, Message = "log #{LogNumber}")]
    public static partial void LogCritical_1_Arg_Generated(ILogger logger, long logNumber);

    [LoggerMessage(EventId = 10, Level = LogLevel.Information, Message = "Got weather forecast!")]
    public static partial void GotWeatherForecast(ILogger logger);

    [LoggerMessage(EventId = 0, EventName = "GotWeatherForecastWithSummary", Level = LogLevel.Information, Message = "Got weather forecast, and it's {Summary}!")]
    public static partial void GotWeatherForecastWithSummary(ILogger logger, string summary);
    #endregion

    #region SkipEnabledCheckFeature
    public static void SkipEnabledCheckFalse(ILogger logger) =>
        s_SkipEnabledCheckFalse(logger, null);

    private static readonly Action<ILogger, Exception?> s_SkipEnabledCheckFalse =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1011),
            "Hello World",
            s_LogDefineOptionsSkipFalse);

    public static void SkipEnabledCheckTrue(ILogger logger) =>
        s_SkipEnabledCheckTrue(logger, null);

    private static readonly Action<ILogger, Exception?> s_SkipEnabledCheckTrue =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(1010),
            "Hello World",
            s_LogDefineOptions);

    private static LogDefineOptions s_LogDefineOptions = new LogDefineOptions() {  SkipEnabledCheck = true };
    private static LogDefineOptions s_LogDefineOptionsSkipFalse = new LogDefineOptions() {  SkipEnabledCheck = false };
    #endregion
}
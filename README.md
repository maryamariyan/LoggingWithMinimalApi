# Source generated logging with Minimal APIs in ASP.NET Core

Using .NET 6.0 RC2 and with the minimal API template:

```
> dotnet new webapi -minimal
```

This sample uses a custom console log formatter and also uses source generated logging methods partially defined in the static `Log` class.

### Links to documentation:

- .NET 6.0 feature: [Compile-time logging source generation](https://docs.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator)
- .NET 5.0 feature: [Console Log Formatting](https://docs.microsoft.com/en-us/dotnet/core/extensions/console-log-formatter)

### Sample usage in dotnet/aspnetcore 

Sample usage of logging source generator in aspnetcore repo:
https://github.com/dotnet/aspnetcore/blob/main/src/Servers/Kestrel/Transport.Quic/src/Internal/QuicLog.cs#L33-L42

The aspnetcore sample above shows that we could set `[LoggerMessage(..., SkipEnabledCheck = true)]` to guard expensive custom logic from running for cases where logging is disabled for that level. This flag tells source generator to skip adding an `IsEnabled` check during code generation by assuming the developer itself will add that flag around the log method themselves (as shown in the aspnet sample).

<details>
 <summary> Advanced Topics </summary>

### `SkipEnabledCheck` (6.0) feature:

Refer to source code [here](https://github.com/dotnet/runtime/blob/e5dd7150e6ced783159a8ae3adb77b899b1204db/src/libraries/Microsoft.Extensions.Logging.Abstractions/src/LoggerMessage.cs#L150-L161). This logic suggests that on every log call we are making sure that logging is enabled for that log level. We should be able to skip this flag check if the user code is already guarding the log call with an `IsEnabled` check (as seen in the aspnet code sample above).

To allow for deferring the `IsEnabled` check to user code, in .NET 6.0 we added `LogDefineOptions` for `LoggerMessage.Define` APIs, accepting a `SkipEnabledCheck` flag. By default this flag is `false` and the logging code will always do a `logger.IsEnabled(logLevel)` check before each log call, because there is no guarantee that log calls would be guarded by user code already. 

But with source generated code, since the source generator does this `IsEnabled` check then the `LogDefineOptions' flag is by default set to true. This can be skipped further by using `SkipEnabledCheck` flag on the `LoggerMessageAttribute` as seen in the aspnetcore sample.

### Benchmarks on Logging:

The benchmark report below explains more background into performance investigations done with logging during the 6.0 timeframe:
https://gist.github.com/maryamariyan/0bad4136655f344bf203274e5b7431b4#file-report-md

 </summary>

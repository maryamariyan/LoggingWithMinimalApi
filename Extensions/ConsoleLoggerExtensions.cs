using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Console.ExampleFormatters.Custom;

namespace Demo
{
    public static class ConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddCustomFormatter(this ILoggingBuilder builder, Action<CustomColorOptions> configure)
        {
            return builder.AddConsole(o => { o.FormatterName = "myCustomFormatter"; })
                .AddConsoleFormatter<CustomColorFormatter, CustomColorOptions>(configure);
        }
    }
}
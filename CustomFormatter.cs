using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Console.ExampleFormatters.Custom;

namespace Console.ExampleFormatters.Custom
{
    public class CustomColorOptions : SimpleConsoleFormatterOptions
    {
        public string? CustomPrefix { get; set; }
    }
 
    public sealed class CustomColorFormatter : ConsoleFormatter, IDisposable
    {
        private readonly IDisposable _optionsReloadToken;
        private CustomColorOptions _formatterOptions;

        private bool ConsoleColorFormattingEnabled =>
            _formatterOptions.ColorBehavior == LoggerColorBehavior.Enabled ||
            _formatterOptions.ColorBehavior == LoggerColorBehavior.Default &&
            System.Console.IsOutputRedirected == false;

        public CustomColorFormatter(IOptionsMonitor<CustomColorOptions> options)
            // Case insensitive
            : base("myCustomFormatter") =>
            (_optionsReloadToken, _formatterOptions) =
                (options.OnChange(ReloadLoggerOptions), options.CurrentValue);

        private void ReloadLoggerOptions(CustomColorOptions options) =>
            _formatterOptions = options;

        public override void Write<TState>(
            in LogEntry<TState> logEntry,
            IExternalScopeProvider scopeProvider,
            TextWriter textWriter)
        {
            if (logEntry.Exception is null)
            {
                // return;
            }

            string message =
                logEntry.Formatter(
                    logEntry.State, logEntry.Exception);

            if (message == null)
            {
                return;
            }

            CustomLogicGoesHere(textWriter);
            textWriter.Write(message);
        }

        private void CustomLogicGoesHere(TextWriter textWriter)
        {
            if (ConsoleColorFormattingEnabled)
            {
                textWriter.WriteWithColor(
                    _formatterOptions.CustomPrefix ?? string.Empty,
                    ConsoleColor.Black,
                    ConsoleColor.Green);
            }
            else
            {
                textWriter.Write(_formatterOptions.CustomPrefix);
            }
        }

        public void Dispose() => _optionsReloadToken?.Dispose();
    }
}
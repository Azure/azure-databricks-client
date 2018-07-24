using Microsoft.Extensions.CommandLineUtils;

namespace Microsoft.Azure.Databricks.Cli
{
    public static class ConsoleLogger
    {
        private static readonly AnsiConsole StdOut = AnsiConsole.GetOutput(true);
        private static readonly AnsiConsole StdErr = AnsiConsole.GetError(true);

        public static void WriteLineVerbose(string message)
        {
            StdOut.WriteLine(AnsiColor.Cyan + message + AnsiColor.Reset);
        }

        public static void WriteLineDebug(string message)
        {
            StdOut.WriteLine(AnsiColor.Reset + message + AnsiColor.Reset);
        }

        public static void WriteLineInfo(string message)
        {
            StdOut.WriteLine(AnsiColor.Green + message + AnsiColor.Reset);
        }

        public static void WriteLineError(string message)
        {
            StdErr.WriteLine(AnsiColor.Red + message + AnsiColor.Reset);
        }
    }
}
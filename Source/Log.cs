using System;
using System.Diagnostics;

namespace BCT.Source
{
    public static class Log
    {
        private static ConsoleColor prevForegroundColor;

        public static ulong WarningCount { get; private set; }

        public static ulong ErrorCount { get; private set; }

        public static void Error(string s, params object[] args)
        {
            ErrorCount++;
            WriteColorLine(s, ConsoleColor.Red, args);
        }

        public static void Warning(string s, params object[] args)
        {
            WarningCount++;
            WriteColorLine(s, ConsoleColor.DarkYellow, args);
        }

        public static void Info(string s, params object[] args)
        {
            WriteColorLine(s, ConsoleColor.Gray, args);
        }

        public static void Success(string s, params object[] args)
        {
            WriteColorLine(s, ConsoleColor.Green, args);
        }

        public static void VerboseInfo( string s )
        {
            // WriteColorLine(s, ConsoleColor.White);
        }

        private static void WriteColorLine(string s, ConsoleColor color, params object[] args)
        {
            prevForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(s, args);
            Console.ForegroundColor = prevForegroundColor;
        }
    }

    struct TimeScope : IDisposable
    {
        private readonly string message;
        private readonly Stopwatch sw;

        public TimeScope(string message)
        {
            this.message = message;
            sw = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            Log.Info(message, sw.ElapsedMilliseconds / 1000.0);
        }
    }

    // ReSharper disable InconsistentNaming
    class BCTInvalidOperation : InvalidOperationException
    // ReSharper restore InconsistentNaming
    {
        private readonly bool hasMessage;

        public BCTInvalidOperation( string message = null ) : base( message )
        {
            hasMessage = string.IsNullOrEmpty( message ) == false;
        }

        public bool HasMessage
        {
            get { return hasMessage; }
        }
    }
}
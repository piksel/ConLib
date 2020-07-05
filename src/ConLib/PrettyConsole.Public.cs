using System;
using System.Collections.Generic;
using ConLib.Console;

namespace ConLib
{
    public static partial class PrettyConsole
    {
        public static List<IConsoleFormatter> PrettyFormatters => Fmts;

        public static void Write(FormattableString fs, params ConCol?[] conCols)
        {
            foreach (var fmt in Fmts) fmt.Write(fs, conCols);
        }

        public static void WriteLine(FormattableString fs, params ConCol?[] conCols)
        {
            foreach (var fmt in Fmts) fmt.WriteLine(fs, conCols);
        }

        public static void WriteLine()
        {
            foreach (var fmt in Fmts) fmt.WriteLine();
        }

        public static void WriteColor(string text, ConCol color)
        {
            foreach (var fmt in Fmts) fmt.WriteColor(text, color);
        }

        public static void Write(Exception x, string? context = null)
        {
            foreach (var fmt in Fmts) fmt.Write(x, context);

        }
        public static void Write(TimeSpan elapsed)
        {
            foreach (var fmt in Fmts) fmt.Write(elapsed);

        }

        public static void PushGroup(string? type = null)
        {
            foreach (var fmt in Fmts) fmt.PushGroup(type);
        }

        public static void PopGroup(bool wasWrittenTo = true)
        {
            foreach (var fmt in Fmts) fmt.PopGroup(wasWrittenTo);
        }

        public static void WriteVersion<T>() => WriteVersion(typeof(T).Assembly.GetName().Version);

        public static void WriteVersion(Version? version = null)
        {
            version ??= typeof(PrettyConsole).Assembly.GetName().Version;
            Write($"v{version.Major}.{version.Minor}.{version.Build}");
        }
    }
}

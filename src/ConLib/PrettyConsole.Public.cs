using System;

namespace ConLib
{
    public static partial class PrettyConsole
    {
        static IConsoleFormatter fmt = new ConsoleFormatter(ConsoleOutWrapper, new ConsoleColorManager());

        public static void Write(FormattableString fs, params ConCol?[] conCols)
            => fmt.WriteFormat(fs.Format, fs.GetArguments(), conCols, fs.ArgumentCount);

        public static void WriteLine(FormattableString fs, params ConCol?[] conCols)
            => fmt.WriteFormat(fs.Format + Environment.NewLine, fs.GetArguments(), conCols, fs.ArgumentCount);

        public static void WriteLine()
            => fmt.WriteLine();

        
    }
}

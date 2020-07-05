using System;
using System.Collections.Generic;
using System.Text;
using ConLib.Console;

namespace ConLib
{
    public static partial class PrettyConsole
    {
        private static readonly List<IConsoleFormatter> Fmts
            = new List<IConsoleFormatter>()
            {
                new ColorFormatter(new ConsoleWriter())
            };

        private static readonly ConsoleWrapper ConsoleOutWrapper 
            = ConsoleWrapper.ForOut(msg => WriteColor(msg ?? "", ConCol.DarkGray));

        private static readonly ConsoleWrapper ConsoleErrWrapper 
            = ConsoleWrapper.ForError(msg => WriteColor(msg ?? "", ConCol.Red));

        private static void WriteFormat(string format, IEnumerable<object?> args, IEnumerable<ConCol?> conCols, int argCount)
        {
            foreach (var fmt in Fmts) fmt.WriteFormat(format, args, conCols, argCount);
        }
    }
}

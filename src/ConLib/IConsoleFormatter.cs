using System;
using System.Collections.Generic;

namespace ConLib
{
    interface IConsoleFormatter
    {
        void WriteFormat(string format, IEnumerable<object?> args, IEnumerable<ConCol?> conCols, int argCount);
        void Write(Exception x, string? v = null);
        void Write(TimeSpan elapsed);
        void WriteColor(string v, ConCol white);
        void Write(FormattableString fs, params ConCol?[] conCols);
        void WriteLine(FormattableString fs, params ConCol?[] conCols);
        void WriteLine();
    }
}

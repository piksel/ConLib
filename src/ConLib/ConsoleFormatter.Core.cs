using System;
using System.Collections.Generic;
using System.Linq;
using C = System.Console;

namespace ConLib
{
    public partial class ConsoleFormatter: IConsoleFormatter
    {
        public void WriteFormat(string format, IEnumerable<object?> args, IEnumerable<ConCol?> conCols, int argCount)
        {
            var argRanges = ParseInterpArgs(format, argCount);

            var initialColor = C.ForegroundColor;
            var colors = conCols.GetEnumerator();
            var allArgs = args.ToArray();

            Index regStart = 0;
            for (int i = 0; i < argCount; i++)
            {
                // Write non-colorized lead-in
                C.Write(format[regStart..argRanges[i].Start]);

                if (!colors.MoveNext() || !(colors.Current is ConCol color))
                {
                    if (allArgs[i] is object arg && !(arg is DBNull))
                    {
                        color = TypeColors.ColorFrom(arg);
                    }
                    else
                    {
                        allArgs[i] = allArgs[i] == null ? "<null>" : "<DBNull>";
                        color = TypeColors.Null;
                    }
                }

                C.ForegroundColor = color.ConsoleColor;

                // Write colorized value
                writer.Write(format[argRanges[i]], allArgs);

                C.ForegroundColor = initialColor;
                regStart = argRanges[i].End;

            }

            // Write non-colorized lead-out
            C.Write(format[regStart..]);
        }

        public void WriteColor(string text, ConCol color)
        {
            var initialColor = colorManager.GetColor();
            colorManager.SetColor(color);
            writer.Write(text);
            colorManager.SetColor(initialColor);
        }

        public static Range[] ParseInterpArgs(string format, int argCount)
        {
            var argRanges = new Range[argCount];

            var argStart = -1;
            int argNum = -1;
            for (int i = 0; i < format.Length; i++)
            {
                var cc = format[i];
                switch (cc)
                {
                    case '{':
                        if (argStart == -1 && format[i + 1] != '{' && (i == 0 || format[i - 1] != '{'))
                        {
                            argStart = i;
                        }
                        break;

                    case '}':
                        if (argStart != -1 && (i == format.Length - 1 || format[i + 1] != '}') && format[i - 1] != '}')
                        {
                            if (++argNum >= argCount)
                            {
                                throw new ArgumentOutOfRangeException(nameof(argCount), "Not enough arguments were supplied for format string");
                            }
                            argRanges[argNum] = new Range(argStart, i + 1);
                            argStart = -1;
                        }
                        break;
                }
            }

            return argRanges;
        }
    }
}

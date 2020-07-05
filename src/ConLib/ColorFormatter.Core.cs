using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ConLib
{
    public partial class ColorFormatter: IConsoleFormatter, IDisposable
    {
        private readonly ColorWriter _writer;

        public ColorFormatter(ColorWriter writer)
        {
            this._writer = writer;
        }

        public virtual void Write(FormattableString fs, params ConCol?[] conCols)
        {
            if (fs != null)
            {
                WriteFormat(fs.Format, fs.GetArguments(), conCols, fs.ArgumentCount);
            }
        }

        public virtual void WriteLine(FormattableString fs, params ConCol?[] conCols)
        {
            Write(fs, conCols);
            WriteLine();
        }

        public void WriteLine()
            => _writer.WriteLine();

        public void PushGroup(string? type) => _writer.PushGroup(type);

        public void PopGroup(bool wasWrittenTo) => _writer.PopGroup(wasWrittenTo);


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _writer.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void WriteFormat(string format, IEnumerable<object?> args, IEnumerable<ConCol?> conCols, int argCount)
        {
            var argRanges = ParseInterpArgs(format, argCount);

            var colors = conCols?.GetEnumerator();
            var allArgs = args.ToArray();

            Index regStart = 0;
            for (var i = 0; i < argCount; i++)
            {
                // Write non-colorized lead-in
                _writer.Write(format[regStart..argRanges[i].Start]);

                if (colors == null || !colors.MoveNext() || !(colors.Current is { } color))
                {
                    if (allArgs[i] is { } arg && !(arg is DBNull))
                    {
                        color = TypeColors.ColorFrom(arg);
                    }
                    else
                    {
                        allArgs[i] = allArgs[i] == null ? "<null>" : "<DBNull>";
                        color = TypeColors.Null;
                    }
                }

                _writer.PushColor(color);

                // Write colorized value
                _writer.Write(string.Format(CultureInfo.InvariantCulture, format[argRanges[i]], allArgs));

                _writer.PopColor();
                regStart = argRanges[i].End;

            }

            // Write non-colorized lead-out
            _writer.Write(format[regStart..]);
        }

        public void WriteColor(string s, ConCol color)
        {
            _writer.PushColor(color);
            _writer.Write(s);
            _writer.PopColor();
        }

        public static Range[] ParseInterpArgs(string format, int argCount)
        {
            var argRanges = new Range[argCount];

            var argStart = -1;
            var argNum = -1;
            for (var i = 0; i < format?.Length; i++)
            {
                var cc = format![i];
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
                                throw new ArgumentOutOfRangeException(nameof(argCount), 
                                    "Not enough arguments were supplied for format string");
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

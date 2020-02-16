using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ConLib
{
    public partial class ConsoleFormatter
    {
        private TextWriter writer;
        private IColorManager colorManager;

        public ConsoleFormatter(TextWriter writer, IColorManager colorManager)
        {
            this.writer = writer;
            this.colorManager = colorManager;
        }

        public void Write(FormattableString fs, params ConCol?[] conCols)
            => WriteFormat(fs.Format, fs.GetArguments(), conCols, fs.ArgumentCount);

        public void WriteLine(FormattableString fs, params ConCol?[] conCols)
            => WriteFormat(fs.Format + Environment.NewLine, fs.GetArguments(), conCols, fs.ArgumentCount);

        public void WriteLine()
            => writer.WriteLine();

        public void Write(TimeSpan ts)
        {
            var tc = TypeColors.Time;
            if (ts.Days > 0)
            {
                Write($"{ts.Days}d {ts.Hours}h {ts.Minutes}m {ts.Seconds}.{ts.Milliseconds:000}", tc, tc, tc, tc, tc);
            }
            else if (ts.Hours > 0)
            {
                Write($"{ts.Hours}h {ts.Minutes}m {ts.Seconds}.{ts.Milliseconds:000}", tc, tc, tc, tc);
            }
            else if (ts.Minutes > 0)
            {
                Write($"{ts.Minutes}m {ts.Seconds}.{ts.Milliseconds:000}s", tc, tc, tc);
            }
            else if (ts.Seconds > 0)
            {
                Write($"{ts.Seconds}.{ts.Milliseconds:000}s", tc, tc);
            }
            else
            {
                Write($"{ts.Milliseconds}ms", tc);
            }
        }

        public void Write(Exception x, string? context = null)
        {
            var initialColor = colorManager.GetColor();
            if (context is string)
            {
                WriteColor(context + " ", ConCol.Red);
            }

            var xt = x.GetType();

            Write($"{(xt.Namespace ?? "") + "."}{xt.Name}: {x.Message}\n", ConCol.DarkGray, ConCol.White, ConCol.Red);

            if (x.HelpLink is string url)
            {
                Write($"Help URL: {url}\n", ConCol.Blue);
            }

            var st = new StackTrace(x, fNeedFileInfo: true);

            foreach (var stackFrame in st.GetFrames())
            {
                if (stackFrame is StackFrame sf)
                {

                    var method = sf.GetMethod();
                    var methodType = method?.DeclaringType;
                    var methodParams = method?.GetParameters();

                    var ns = methodType?.Namespace ?? "<Unknown>";
                    var mclass = methodType?.Name ?? "<Unknown>";
                    if (mclass == "<>c")
                    {
                        var refType = methodType?.ReflectedType?.Name ?? "";
                        mclass = $"{refType}@";
                    }

                    var name = method?.Name ?? "<Unknown>";
                    if (name.Contains(">b__"))
                    {
                        name = $"$Func{name}";
                    }

                    colorManager.SetColor(ConCol.DarkGray);
                    Write($"  at {ns}.{mclass}.{name}(", ConCol.DarkGray, ConCol.White, ConCol.Yellow);
                    colorManager.SetColor(initialColor);

                    foreach (var mp in methodParams ?? new ParameterInfo[0])
                    {
                        var pt = mp.ParameterType;
                        Write($"{pt.Namespace}.{pt.Name} {mp.Name}", ConCol.DarkGray, ConCol.White, ConCol.Yellow);
                        if (mp.Position < methodParams?.Length - 1)
                        {
                            WriteColor(", ", ConCol.DarkGray);
                        }
                    }
                    WriteColor(")", ConCol.DarkGray);

                    if (sf.HasSource())
                    {
                        string path = "";
                        if (sf?.GetFileName() is string file)
                        {
                            path = (Path.GetDirectoryName(file) ?? "") + Path.DirectorySeparatorChar;
                            file = Path.GetFileName(file);

                        }
                        else
                        {
                            file = methodType?.Assembly.GetName().ToString() ?? "<Unknown>";
                        }

                        var line = sf?.GetFileLineNumber() ?? 0;
                        Write($" in {path}{file}{":"}{line}\n", ConCol.DarkGray, ConCol.White, ConCol.DarkGray, ConCol.Cyan);
                    }
                    else
                    {
                        WriteLine();
                    }
                }
                else
                {
                    Write($"  <Missing Stack Frame>\n", ConCol.DarkRed);
                    continue;
                }
            }

            colorManager.SetColor(initialColor);
        }
    }
}

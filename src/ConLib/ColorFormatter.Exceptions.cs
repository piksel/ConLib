using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using static ConLib.ConCol;

namespace ConLib
{
    partial class ColorFormatter
    {
        public virtual void Write(Exception x, string? context = null)
        {
            if (context is { })
            {
                WriteColor(context + " ", Red);
            }

            var xt = x?.GetType();

            Write($"{(xt?.Namespace ?? "") + "."}{xt?.Name}: {x?.Message}\n", 
                DarkGray, White, Red);

            if (x?.HelpLink is { } url)
            {
                Write($"Help URL: {url}\n", Blue);
            }

            var st = new StackTrace(x, fNeedFileInfo: true);

            _writer.PushGroup("stacktrace");

            foreach (var stackFrame in st.GetFrames())
            {
                if (stackFrame is { } sf)
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
                    if (name.Contains(">b__", StringComparison.InvariantCultureIgnoreCase))
                    {
                        name = $"$Func{name}";
                    }

                    _writer.PushColor(DarkGray);
                    Write($"at {ns}.{mclass}.{name}(", DarkGray, White, Yellow);
                    _writer.PopColor();

                    foreach (var mp in methodParams ?? Array.Empty<ParameterInfo>())
                    {
                        var pt = mp.ParameterType;
                        Write($"{pt.Namespace}.{pt.Name} {mp.Name}", 
                            DarkGray, White, Yellow);
                        if (mp.Position < methodParams?.Length - 1)
                        {
                            WriteColor(", ", DarkGray);
                        }
                    }
                    WriteColor(")", DarkGray);

                    if (sf.HasSource())
                    {
                        string path = "";
                        if (sf?.GetFileName() is { } file)
                        {
                            path = (Path.GetDirectoryName(file) ?? "") + Path.DirectorySeparatorChar;
                            file = Path.GetFileName(file);

                        }
                        else
                        {
                            file = methodType?.Assembly.GetName().ToString() ?? "<Unknown>";
                        }

                        var line = sf?.GetFileLineNumber() ?? 0;
                        Write($" in {path}{file}{":"}{line}\n", 
                            DarkGray, White, DarkGray, Cyan);
                    }
                    else
                    {
                        WriteLine();
                    }
                }
                else
                {
                    Write($"  <Missing Stack Frame>\n", DarkRed);
                }
            }

            _writer.PopGroup(st.FrameCount > 0);
        }
    }
}

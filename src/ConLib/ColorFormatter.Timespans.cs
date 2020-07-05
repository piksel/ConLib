using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ConLib
{
    public partial class ColorFormatter
    {
        public virtual void Write(TimeSpan ts)
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

    }
}

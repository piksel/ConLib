using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConLib
{
    public static partial class PrettyConsole 
    {
        public static async Task<bool> DoTaskAsync(string description, Func<Task> task, bool continueOnFail = false)
        {
            fmt.WriteColor($"{description}... ", ConCol.White);

            var stopWatch = Stopwatch.StartNew();
            try
            {
                await task();
                stopWatch.Stop();
                fmt.Write($"{"OK!"} (in ", ChoreOptions.SucceededColor);
                fmt.Write(stopWatch.Elapsed);
                fmt.Write($")\n");
                return true;
            }
            catch (Exception x)
            {
                stopWatch.Stop();
                fmt.Write($"{"Failed!"} (in ", ChoreOptions.FailedColor);
                fmt.Write(stopWatch.Elapsed);
                fmt.Write($")\n");
                fmt.Write(x);
                if (!continueOnFail)
                {
                    throw;
                }
                return false;
            }
        }

        public static bool DoTask(string name, Func<Task> task, bool continueOnFail = false)
            => DoTaskAsync(name, task, continueOnFail).Result;

    }
}
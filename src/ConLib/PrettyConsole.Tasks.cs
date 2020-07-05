using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConLib
{
    public static partial class PrettyConsole 
    {
        public static async Task<bool> DoTaskAsync(string description, Func<Task> task, bool continueOnFail = false)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            WriteColor($"{description}... ", ConCol.White);

            var stopWatch = Stopwatch.StartNew();
            try
            {
                await task.Invoke().ConfigureAwait(true);
                stopWatch.Stop();
                Write($"{"OK!"} (in ", ChoreOptions.SucceededColor);
                Write(stopWatch.Elapsed);
                Write($")\n");
                return true;
            }
            catch (Exception x)
            {
                stopWatch.Stop();
                Write($"{"Failed!"} (in ", ChoreOptions.FailedColor);
                Write(stopWatch.Elapsed);
                Write($")\n");
                Write(x);
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
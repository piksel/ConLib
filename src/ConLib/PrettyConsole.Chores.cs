using System;
using System.Diagnostics;

namespace ConLib
{
    public static partial class PrettyConsole
    {
        public static ChoreOptions ChoreOptions { get; set; } = new ChoreOptions();

        public static int Layer { get; private set; } = 0;
        public static string Indent { get; private set; } = "";

        private static readonly ConsoleWrapper ConsoleOutWrapper = ConsoleWrapper.ForOut;
        private static readonly ConsoleWrapper ConsoleErrWrapper = ConsoleWrapper.ForError;

        public static bool DoChore(string name, Action action, bool continueOnFail = false)
        {
            fmt.WriteFormat(ChoreOptions.StartedFormat, new[] { name, "started" }, ChoreOptions.StartedColors, ChoreOptions.StartedColors.Length);

            Indent = new string(' ', ++Layer * ChoreOptions.IndentAmount);

            ConsoleOutWrapper.Capture();
            ConsoleErrWrapper.Capture();

            ConsoleErrWrapper.ResetWrittenTo();

            var success = false;
            var stopWatch = new Stopwatch();

            try
            {
                stopWatch.Start();
                action.Invoke();
                stopWatch.Stop();
                success = !ConsoleErrWrapper.WasWrittenTo;
            }
            catch (Exception x)
            {
                stopWatch.Stop();
                fmt.Write(x, "Error while running job:");
            }

            Indent = new string(' ', --Layer * ChoreOptions.IndentAmount);
            if (Layer == 0)
            {
                ConsoleOutWrapper.Release();
                ConsoleErrWrapper.Release();
            }

            if (success)
            {
                fmt.WriteFormat(ChoreOptions.EndedFormat, new object[] { name, "succeeded" }, ChoreOptions.SucceededColors, ChoreOptions.SucceededColors.Length);

            }
            else
            {
                fmt.WriteFormat(ChoreOptions.EndedFormat, new object[] { name, "failed" }, ChoreOptions.FailedColors, ChoreOptions.FailedColors.Length);
            }

            fmt.Write(stopWatch.Elapsed);

            WriteLine($".\n");

            if (!success && !continueOnFail)
            {
                throw new Exception($"Task {name} failed! Aborting.");
            }

            return success;
        }

    }
}

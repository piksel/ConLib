using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ConLib
{
    public static partial class PrettyConsole
    {
        public static ChoreOptions ChoreOptions { get; set; } = new ChoreOptions();

        private static int layer = 0;


        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "By design")]
        public static bool DoChore(string name, Action action, bool continueOnFail = false)
        {
            WriteFormat(ChoreOptions.StartedFormat, new[] { name, "started" }, ChoreOptions.StartedColors, ChoreOptions.StartedColors.Count);

            PushGroup("job");

            ++layer;

            ConsoleOutWrapper.Capture();
            ConsoleErrWrapper.Capture();

            ConsoleErrWrapper.ResetWrittenTo();

            var success = false;
            var stopWatch = new Stopwatch();

            try
            {
                stopWatch.Start();
                action?.Invoke();
                stopWatch.Stop();
                success = !ConsoleErrWrapper.WasWrittenTo;
            }
            catch (Exception x)
            {
                stopWatch.Stop();
                Write(x, "Error while running job:");
            }

            // TODO: This is not correct, move this logic
            PopGroup(ConsoleErrWrapper.WasWrittenTo || ConsoleOutWrapper.WasWrittenTo);

            if (--layer == 0)
            {
                ConsoleOutWrapper.Release();
                ConsoleErrWrapper.Release();
            }

            if (success)
            {
                WriteFormat(ChoreOptions.EndedFormat, new object[] { name, "succeeded" }, ChoreOptions.SucceededColors, ChoreOptions.SucceededColors.Count);

            }
            else
            {
                WriteFormat(ChoreOptions.EndedFormat, new object[] { name, "failed" }, ChoreOptions.FailedColors, ChoreOptions.FailedColors.Count);
            }

            Write(stopWatch.Elapsed);

            WriteLine($".\n");

            if (!success && !continueOnFail)
            {
                 throw new Exception($"Task {name} failed! Aborting.");
            }

            return success;
        }

    }
}

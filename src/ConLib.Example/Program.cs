using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ConLib.HTML;
using static ConLib.ConCol;
using static ConLib.PrettyConsole;

namespace ConLib.Example
{
    [SuppressMessage("ReSharper", "StringLiteralAsInterpolationArgument")]
    internal class Program
    {
        private static void Section(string description, Action action)
        {
            WriteLine();
            WriteColor(description, White);
            WriteLine();

            PushGroup();
            action?.Invoke();
            PopGroup();
        }

        private static void Main(string[] args)
        {
            using var html = new HTMLWriter("output.html");
            PrettyFormatters.Add(new ColorFormatter(html));

            WriteLine($"ConLib Example");
            Write($"Using ConLib ");
            WriteVersion();
            WriteLine();

            Section($"Value highlighting (colors from type):", () =>
            {
                WriteLine($" String: {"banana"}");
                WriteLine($" Numbers: {93}, {Math.E:f4}, 0x{0x8b:x} ");
                WriteLine($" Booleans: {true} and {false}");
                WriteLine($" Time: {DateTime.Now:O} (DateTime) and {TimeSpan.FromSeconds(23)} (TimeSpan)");
                WriteLine($" Objects: {new Program()} and {new object()}");
                WriteLine($" Exception: {new ArgumentNullException("target")}");
                WriteLine($" Null: {null} (null) and {DBNull.Value} (DBNull)");
            });

            Section($"Explicit colors: ", () =>
            {
                WriteLine($" Number {3} is the {"only"} {true} {new Exception("YOLO!")}", 
                                   Green,        Blue,   Red,           Yellow);

            });

            Section($"Jobs:", () =>
            {
                DoChore("Foo", () =>
                {
                    WriteLine($"Output from within task");
                    Task.Delay(100).Wait();
                    WriteLine($"Some work has been done");
                    Task.Delay(100).Wait();
                    WriteLine($"Let's do some subtasks!");

                    DoChore("Subtask 1", () =>
                    {
                        WriteLine($"Output from within subtask");
                        Task.Delay(1000).Wait();
                        WriteLine($"Everything is done!");
                    });

                    DoChore("Subtask 2", () =>
                    {
                        WriteLine($"Subtask that is about to fail");
                        throw new ArgumentOutOfRangeException("PI", 4, $"Subtask failed!")
                        {
                            HelpLink = "https://lmgtfy.com/?q=how%20to%20code",
                        };
                    });
                }, continueOnFail: true);

                WriteLine($"At least we didn't throw in {"Main"}{"()"}!", Yellow, White);
            });

            Section($"Output capture and indentation: ", () =>
            {
                DoChore("Capture StdOut", () =>
                {
                    System.Console.Write("Output from System.Console.WriteLine(), which is correctly indented.");
                });

                DoChore("Capture StdErr", () =>
                {
                    System.Console.Error.WriteLine("Output from System.Console.Error.WriteLine(), which causes the job to fail.");
                }, continueOnFail: true);

                DoChore("Run external program", () =>
                {
                    //Exec("ping localhost");
                });

            });

            Section($"Tasks:", () =>
            {
                DoTask("Doing the dishes", () => Task.Delay(500));
                DoTask("Mopping the floors", () => Task.Delay(500));
                DoTask("Cooking dinner", () => Task.Delay(500).ContinueWith(t =>
                    throw new Exception("House is on fire!")
                ), continueOnFail: true);

                WriteLine();
                WriteLine($"At least we didn't throw in {"Main"}{"()"}!", Yellow, White);

            });
        }
    }
}

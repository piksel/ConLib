using System;
using System.Threading.Tasks;
using static ConLib.ConCol;
using static ConLib.PrettyConsole;

namespace ConLib.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine();
            WriteLine($"Value highlighting (colors from type): ");

            WriteLine($" String: {"banana"}");
            WriteLine($" Numbers: {93}, {Math.E:f4}, 0x{0x8b:x} ");
            WriteLine($" Booleans: {true} and {false}");
            WriteLine($" Time: {DateTime.Now} (DateTime) and {TimeSpan.FromSeconds(23)} (TimeSpan)");
            WriteLine($" Objects: {new Program()} and {new Object()}");
            WriteLine($" Exception: {new ArgumentNullException("target")}");
            WriteLine($" Null: {null} (null) and {DBNull.Value} (DBNull)");

            WriteLine();
            WriteLine($"Explicit colors: ");
            WriteLine($" Number {3} is the {"only"} {true} {new Exception("YOLO!")}", Green, Blue, Red, Yellow);

            WriteLine();
            WriteLine($"Jobs:");
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

            WriteLine();
            WriteLine($"Tasks:");

            DoTask("Doing the dishes", () => Task.Delay(500));
            DoTask("Mopping the floors", () => Task.Delay(500));
            DoTask("Cooking dinner", () => Task.Delay(500).ContinueWith(t =>
                throw new Exception("House is on fire!")
            ), continueOnFail: true);

            WriteLine();
            WriteLine($"\nAt least we didn't throw in {"Main"}{"()"}!", Yellow, White);
        }
    }
}

using System;
using System.IO;
using System.Text;

namespace ConLib
{
    internal class ConsoleWrapper: TextWriter
    {
        private TextWriter innerWriter;
        private Action<TextWriter> setWriter;
        private readonly bool isError;
        private bool isCapturing = false;
        private bool inStartingPos = true;

        public static ConsoleWrapper ForError => new ConsoleWrapper(Console.Error, Console.SetError);
        public static ConsoleWrapper ForOut => new ConsoleWrapper(Console.Out, Console.SetOut);

        public ConsoleWrapper(TextWriter innerWriter, Action<TextWriter> setWriter)
        {
            this.innerWriter = innerWriter;
            this.setWriter = setWriter;
        }

        public bool Capture()
        {
            if(isCapturing) 
            {
                return true;
            }

            setWriter(this);

            isCapturing = true;

            return false;
        }

        public bool Release()
        {
            if (!isCapturing)
            {
                return true;
            }

            setWriter(innerWriter);

            isCapturing = false;

            return false;
        }

        public bool WasWrittenTo { get; private set; } = false;

        public void ResetWrittenTo() => WasWrittenTo = false;

        public override Encoding Encoding => innerWriter.Encoding;

        public override void Write(string? value)
        {
            if(!isCapturing)
            {
                innerWriter.Write(value);
                return;
            }

            WasWrittenTo = true;

            var lines = (value ?? "").Split("\n");
            for(int i=0; i < lines.Length; i++)
            {
                if(inStartingPos)
                {
                    if (string.IsNullOrEmpty(lines[i]) && i == lines.Length - 1) continue;
                    innerWriter.Write(PrettyConsole.Indent);
                }
                innerWriter.Write(lines[i]);
                inStartingPos = false;

                if (i != lines.Length - 1)
                {
                    innerWriter.WriteLine();
                    inStartingPos = true;
                }
            }
        }

        public override void WriteLine(string? value)
            => Write(value is string ? value + "\n" : "\n");

        public override void WriteLine()
            => Write("\n");
    }
}
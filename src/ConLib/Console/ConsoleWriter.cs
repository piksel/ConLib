using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConLib.Console
{
    public class ConsoleWriter: ColorWriter
    {
        private readonly TextWriter _innerWriter = System.Console.Out;
        private bool inStartingPos = true;

        public int IndentAmount { get; set; } = 2;
        public string Indent { get; private set; } = "";

        public override Encoding Encoding => _innerWriter.Encoding;

        public override void Write(string? value)
        {
            var lines = (value ?? "").Split("\n");
            for (var i = 0; i < lines.Length; i++)
            {
                if (inStartingPos)
                {
                    if (string.IsNullOrEmpty(lines[i]) && i == lines.Length - 1) continue;
                    _innerWriter.Write(Indent);
                }

                _innerWriter.Write(lines[i]);
                inStartingPos = false;

                if (i != lines.Length - 1)
                {
                    _innerWriter.WriteLine();
                    inStartingPos = true;
                }
            }
        }

        public ConsoleWriter()
        {
            System.Console.ResetColor();
            ColorStack.Push(new ConCol(System.Console.ForegroundColor));
        }

        public override void PushColor(ConCol color)
        {
            base.PushColor(color);
            System.Console.ForegroundColor = color.ConsoleColor;
        }

        public override ConCol PopColor()
        {
            var color = base.PopColor();
            System.Console.ForegroundColor = ColorStack.Peek().ConsoleColor;
            return color;
        }

        public override void PushGroup(string? type)
        {
            base.PushGroup(type);
            Indent = new string(' ', Layer * IndentAmount);
        }

        public override void PopGroup(bool wasWrittenTo)
        {
            base.PopGroup(wasWrittenTo);
            Indent = new string(' ', Layer * IndentAmount);

            if (!inStartingPos && wasWrittenTo)
            {
                WriteLine();
            }
        }
    }
}

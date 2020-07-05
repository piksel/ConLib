using System;
using System.IO;
using System.Text;
using C = System.Console;

namespace ConLib
{
    internal class ConsoleWrapper: TextWriter
    {
        private readonly TextWriter _innerWriter;
        private readonly Action<TextWriter> _setWriter;
        private bool _isCapturing = false;

        private readonly Action<string?> _write;

        public static ConsoleWrapper ForError(Action<string?> writeAction) 
            => new ConsoleWrapper(C.Error, C.SetError, writeAction);

        public static ConsoleWrapper ForOut(Action<string?> writeAction) 
            => new ConsoleWrapper(C.Out, C.SetOut, writeAction);

        public ConsoleWrapper(TextWriter innerWriter, Action<TextWriter> setWriter, Action<string?> write)
        {
            _innerWriter = innerWriter;
            _setWriter = setWriter;
            _write = write;
        }

        public bool Capture()
        {
            if(_isCapturing) 
            {
                return true;
            }

            _setWriter(this);

            _isCapturing = true;

            return false;
        }

        public bool Release()
        {
            if (!_isCapturing)
            {
                return true;
            }

            _setWriter(_innerWriter);

            _isCapturing = false;

            return false;
        }

        public bool WasWrittenTo { get; private set; } = false;

        public void ResetWrittenTo() => WasWrittenTo = false;

        public override Encoding Encoding => _innerWriter.Encoding;

        public override void Write(string? value)
        {
            if(!_isCapturing)
            {
                _innerWriter.Write(value);
                return;
            }

            WasWrittenTo = true;

            _write?.Invoke(value);
        }

        public override void WriteLine(string? value)
            => Write(value is {} ? value + "\n" : "\n");

        public override void WriteLine()
            => Write("\n");
    }
}
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ConLib
{
    /*
             void PushGroup(string? type);
        void PopGroup();
        int Layer { get; }
     */

    public abstract class ColorWriter: TextWriter, IColorManager
    {
        protected readonly Stack<ConCol> ColorStack = new Stack<ConCol>();

        public virtual ConCol PeekColor() => ColorStack.Peek();

        public virtual void PushColor(ConCol color) => ColorStack.Push(color);

        public virtual ConCol PopColor() => ColorStack.Pop();

        public int Layer { get; private set; } = 0;

        public virtual void PushGroup(string? type) => ++Layer;

        public virtual void PopGroup(bool wasWrittenTo) => --Layer;

        public override void Write(char value) => Write(value.ToString(CultureInfo.InvariantCulture));

        public new abstract void Write(string? value);
    }
}
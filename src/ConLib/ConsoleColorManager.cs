using System;

namespace ConLib
{
    internal class ConsoleColorManager : IColorManager
    {
        public ConCol GetColor() => new ConCol(Console.ForegroundColor);

        public void SetColor(ConCol color) => Console.ForegroundColor = color.ConsoleColor;
    }
}

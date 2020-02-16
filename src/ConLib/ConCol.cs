using System;

namespace ConLib
{
    public struct ConCol
    {
        public ConsoleColor ConsoleColor { get; }

        public ConCol(ConsoleColor consoleColor)
        {
            this.ConsoleColor = consoleColor;
        }

        public static ConCol DefaultForeground = new ConCol(Console.ForegroundColor);

        /// <summary>The color black.</summary>
        public static readonly ConCol Black = new ConCol(ConsoleColor.Black);

        /// <summary>The color dark blue.</summary>
        public static readonly ConCol DarkBlue = new ConCol(ConsoleColor.DarkBlue);

        /// <summary>The color dark green.</summary>
        public static readonly ConCol DarkGreen = new ConCol(ConsoleColor.DarkGreen);

        /// <summary>The color dark cyan (dark blue-green).</summary>
        public static readonly ConCol DarkCyan = new ConCol(ConsoleColor.DarkCyan);

        /// <summary>The color dark red.</summary>
        public static readonly ConCol DarkRed = new ConCol(ConsoleColor.DarkRed);

        /// <summary>The color dark magenta (dark purplish-red).</summary>
        public static readonly ConCol DarkMagenta = new ConCol(ConsoleColor.DarkMagenta);

        /// <summary>The color dark yellow (ochre).</summary>
        public static readonly ConCol DarkYellow = new ConCol(ConsoleColor.DarkYellow);

        /// <summary>The color gray.</summary>
        public static readonly ConCol Gray = new ConCol(ConsoleColor.Gray);

        /// <summary>The color dark gray.</summary>
        public static readonly ConCol DarkGray = new ConCol(ConsoleColor.DarkGray);

        /// <summary>The color blue.</summary>
        public static readonly ConCol Blue = new ConCol(ConsoleColor.Blue);

        /// <summary>The color green.</summary>
        public static readonly ConCol Green = new ConCol(ConsoleColor.Green);

        /// <summary>The color cyan (blue-green).</summary>
        public static readonly ConCol Cyan = new ConCol(ConsoleColor.Cyan);

        /// <summary>The color red.</summary>
        public static readonly ConCol Red = new ConCol(ConsoleColor.Red);

        /// <summary>The color magenta (purplish-red).</summary>
        public static readonly ConCol Magenta = new ConCol(ConsoleColor.Magenta);

        /// <summary>The color yellow.</summary>
        public static readonly ConCol Yellow = new ConCol(ConsoleColor.Yellow);

        /// <summary>The color white.</summary>
        public static readonly ConCol White = new ConCol(ConsoleColor.White);
    }
}

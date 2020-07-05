using System;

namespace ConLib.Themes
{
    public partial class ColorTheme
    {
        public int Black { get; set; }
        public int Blue { get; set; }
        public int Cyan { get; set; }
        public int Green { get; set; }
        public int Purple { get; set; }
        public int Red { get; set; }
        public int White { get; set; }
        public int Yellow { get; set; }
        public int BrightBlack { get; set; }
        public int BrightBlue { get; set; }
        public int BrightCyan { get; set; }
        public int BrightGreen { get; set; }
        public int BrightPurple { get; set; }
        public int BrightRed { get; set; }
        public int BrightWhite { get; set; }
        public int BrightYellow { get; set; }

        public string this[ConsoleColor color] =>
            $"#{ColorFromConsole(color):x6}";

        public int ColorFromConsole(ConsoleColor color)
            => color switch
            {
                ConsoleColor.Black => Black,
                ConsoleColor.DarkBlue => Blue,
                ConsoleColor.DarkCyan => Cyan,
                ConsoleColor.DarkGreen => Green,
                ConsoleColor.DarkMagenta => Purple,
                ConsoleColor.DarkRed => Red,
                ConsoleColor.Gray => White,
                ConsoleColor.DarkYellow => Yellow,
                ConsoleColor.DarkGray => BrightBlack,
                ConsoleColor.Blue => BrightBlue,
                ConsoleColor.Cyan => BrightCyan,
                ConsoleColor.Green => BrightGreen,
                ConsoleColor.Magenta => BrightPurple,
                ConsoleColor.Red => BrightRed,
                ConsoleColor.White => BrightWhite,
                ConsoleColor.Yellow => BrightYellow,
                _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
            };

        public ColorTheme ToLightTheme() => new ColorTheme()
        {
            Black = BrightWhite,
            BrightBlack = White,
            White = BrightBlack,
            BrightWhite = Black,

            Blue = BrightBlue,
            Cyan = BrightCyan,
            Green = BrightGreen,
            Purple = BrightPurple,
            Red = BrightRed,
            Yellow = BrightYellow,

            BrightBlue = Blue,
            BrightCyan = Cyan,
            BrightGreen = Green,
            BrightPurple = Purple,
            BrightRed = Red,
            BrightYellow = Yellow
        };
    }
}

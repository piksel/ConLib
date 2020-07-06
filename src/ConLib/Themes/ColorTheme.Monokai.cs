using System;
using System.Collections.Generic;
using System.Text;

namespace ConLib.Themes
{
    public partial class ColorTheme
    {
        public static ColorTheme Monokai => new ColorTheme()
        {
            Black = 0x1f1f1f,
            Blue = 0x6699df,
            Cyan = 0xe69f66,
            Green = 0xa6e22e,
            Purple = 0xae81ff,
            Red = 0xf92672,
            White = 0xf8f8f2,
            Yellow = 0xe6db74,
            BrightBlack = 0x75715e,
            BrightBlue = 0x66d9ef,
            BrightCyan = 0xe69f66,
            BrightGreen = 0xa6e22e,
            BrightPurple = 0xae81ff,
            BrightRed = 0xf92672,
            BrightWhite = 0xf8f8f2,
            BrightYellow = 0xe6db74
        };

        public static ColorTheme MonokaiLight => new ColorTheme()
        {
            Black = 0xf8f8f2,
            BrightBlack = 0xaba896,
            White = 0x49483e,
            BrightWhite = 0x1f1f1f,

            BrightBlue = 0x004ac2, //0x6699df,
            BrightCyan = 0xbe5a09,
            BrightGreen = 0x009933, // 0xa6e22e,
            BrightPurple = 0x2b0079,
            BrightRed = 0xe60053,
            BrightYellow = 0xb4a204,

            Blue = 0x66d9ef,
            Cyan = 0xe69f66,
            Green = 0xa6e22e,
            Purple = 0xae81ff,
            Red = 0xf92672,
            Yellow = 0xe6db74
        };
    }
}

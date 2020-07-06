using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using ConLib.Themes;

namespace ConLib.HTML
{
    public sealed class HTMLWriter: ColorWriter, IDisposable
    {
        private readonly TextWriter _innerWriter;
        public override Encoding Encoding => _innerWriter.Encoding;

        private HTMLWriter(TextWriter innerWriter): base()
        {
            _innerWriter = innerWriter;

            WriteHeader();

            _innerWriter.Write(
                "<style>" +
                $":root{{padding:1rem;font-family:'Fira Code',monospace;color: {DarkTheme[ConsoleColor.Gray]};background:{DarkTheme[ConsoleColor.Black]}}}" +
                "section.clgroup{padding-left:1rem; " +
                "border-left: 1px transparent solid;border-radius:.5rem;}" +
                $"section.clgroup:hover{{background:#00000020;" +
                $"box-shadow: 0 0 10px #00000020;"+
                $"border-left-color: {DarkTheme[ConsoleColor.DarkGray]}80;}}"
            );

            WriteTheme(DarkTheme);


            _innerWriter.Write(
                "@media print, screen and (prefers-color-scheme: light) {"+
                $":root{{color:{LightTheme[ConsoleColor.Gray]};background:{LightTheme[ConsoleColor.Black]}}}" +
                $"section.clgroup:hover{{background:#ffffff20;" +
                $"box-shadow: 0 0 10px #ffffff20;" +
                $"border-left-color: {LightTheme[ConsoleColor.DarkGray]}80;}}"
                );

            WriteTheme(LightTheme);
            _innerWriter.Write("}");


            _innerWriter.WriteLine(
                "</style>  "
            );
        }

        private void WriteHeader()
        {
            var v = typeof(HTMLWriter).Assembly.GetName().Version;
            _innerWriter.WriteLine($"<!-- Generated at {DateTime.Now:O} by ConLib v{v.Major}.{v.Minor}.{v.Build} p1k.se/conlib -->");
        }

        private void WriteTheme(ColorTheme theme)
        {
            foreach (ConsoleColor c in Enum.GetValues(typeof(ConsoleColor)))
            {
                _innerWriter.Write(
                    $"c{c:D}{{color: {theme[c]};}}"
                );
            }
        }

        public HTMLWriter(Stream outputStream) : this(new StreamWriter(outputStream))
        {
            _outputStream = outputStream;
        }

        public HTMLWriter(string outputFile) : this(File.Open(outputFile, FileMode.Create)) { }

        public override ConCol PopColor()
        {
            var color = base.PopColor();
            _innerWriter.Write($"</c{color.ConsoleColor:D}>");

            return color;
        }

        public override void PushColor(ConCol color)
        {
            _innerWriter.Write($"<c{color.ConsoleColor:D}>");
            base.PushColor(color);
        }

        public override void PushGroup(string? type)
        {
            base.PushGroup(type);
            _innerWriter.WriteLine($"<section class=\"clgroup {(type is {}?$"clgroup-{type}":"")}\">");
        }

        public override void PopGroup(bool wasWrittenTo)
        {
            base.PopGroup(wasWrittenTo);
            _innerWriter.WriteLine("</section>");
        }

        public override void Write(char value)
            => Write(value.ToString(CultureInfo.InvariantCulture));

        public override void Write(string? value) 
            => _innerWriter.Write(WebUtility.HtmlEncode(value)
                    .Replace("\n", "<br/>", StringComparison.InvariantCulture));

        public override void WriteLine()
            => _innerWriter.WriteLine("<br/>");


        public override void WriteLine(string value)
            => _innerWriter.WriteLine($"{value}<br/>");

        public ColorTheme DarkTheme { get; set; } = ColorTheme.Monokai;
        public ColorTheme LightTheme { get; set; } = ColorTheme.MonokaiLight;

        private readonly Stream _outputStream = null!;

        public new void Dispose()
        {
            _innerWriter.Dispose();
            base.Dispose();
        }
    }
}

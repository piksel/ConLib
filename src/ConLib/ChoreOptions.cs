﻿namespace ConLib
{
    public class ChoreOptions
    {
        public ConCol?[] StartedColors => new ConCol?[] { NameColor, StartedColor };
        public ConCol?[] FailedColors => new ConCol?[] { NameColor, FailedColor };
        public ConCol?[] SucceededColors => new ConCol?[] { NameColor, SucceededColor };

        public string StartedFormat { get; set; } = "- Job {0} {1}.\n";
        public string EndedFormat { get; set; } = "- Job {0} {1} in ";

        public ConCol? NameColor { get; set; } = ConCol.Yellow;
        public ConCol? StartedColor { get; set; } = ConCol.Cyan;
        public ConCol? FailedColor { get; set; } = ConCol.Red; 
        public ConCol? SucceededColor { get; set; } = ConCol.Green; 
        public ConCol? DurationColor { get; set; } = ConCol.Blue;

        public int IndentAmount { get; set; } = 4;

    }
}

using System;

namespace ConLib
{
    public static class TypeColors
    {
        public static ConCol Null { get; set; } = ConCol.DarkGray;
        public static ConCol Number { get; set; } = ConCol.Cyan;
        public static ConCol String { get; set; } = ConCol.Yellow;
        public static ConCol Object { get; set; } = ConCol.Magenta;
        public static ConCol Exception { get; set; } = ConCol.Red;
        public static ConCol True { get; set; } = ConCol.Green;
        public static ConCol False { get; set; } = ConCol.Red;
        public static ConCol Time { get; set; } = ConCol.Blue;

        public static ConCol ColorFrom(object value)
        => Type.GetTypeCode(value.GetType()) switch
        {
            TypeCode.String => String,
            TypeCode.Char => String,
            TypeCode.Boolean => (bool)value ? True : False,
            TypeCode.DateTime => Time,
            TypeCode.DBNull => Null,
            TypeCode.Empty => Null,
            var tc when tc >= TypeCode.SByte && tc <= TypeCode.Decimal => Number,
            var _ when value.GetType() == typeof(TimeSpan) => Time,
            var _ when value.GetType().IsSubclassOf(typeof(Exception)) => Exception,
            TypeCode.Object => Object,
            _ => Object,
        };
    }
}

using System;


namespace CB.Model.Common
{
    public static class ConvertHelper
    {
        #region Methods
        public static bool? GetBoolean(object value)
            => value == null ? (bool?)null : Convert.ToBoolean(value);

        public static byte? GetByte(object value)
            => value == null ? (byte?)null : Convert.ToByte(value);

        public static char? GetChar(object value)
            => value == null ? (char?)null : Convert.ToChar(value);

        public static DateTime? GetDateTime(object value)
            => value == null ? (DateTime?)null : Convert.ToDateTime(value);

        public static short? GetInt16(object value)
            => value == null ? (short?)null : Convert.ToInt16(value);

        public static int? GetInt32(object value)
            => value == null ? (int?)null : Convert.ToInt32(value);

        public static long? GetInt64(object value)
            => value == null ? (long?)null : Convert.ToInt64(value);

        public static sbyte? GetSByte(object value)
            => value == null ? (sbyte?)null : Convert.ToSByte(value);

        public static string GetString(object value)
            => value == null ? null : Convert.ToString(value);

        public static ushort? GetUInt16(object value)
            => value == null ? (ushort?)null : Convert.ToUInt16(value);

        public static uint? GetUInt32(object value)
            => value == null ? (uint?)null : Convert.ToUInt32(value);

        public static ulong? GetUInt64(object value)
            => value == null ? (ulong?)null : Convert.ToUInt64(value);
        #endregion
    }
}
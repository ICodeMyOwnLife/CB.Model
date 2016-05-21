using System;


namespace CB.Model.Common
{
    public class FileCapacityHelper
    {
        #region Fields
        private const int ONE_GB = 1 << 30;
        private const int ONE_KB = 1 << 10;
        private const int ONE_MB = 1 << 20;
        #endregion


        #region  Properties & Indexers
        public static string Byte { get; set; } = "Byte";
        public static string GigaByte { get; set; } = "GB";
        public static string KiloByte { get; set; } = "KB";
        public static string MegaByte { get; set; } = "MB";
        #endregion


        #region Methods
        public static double ConvertToGigaBytes(double bytes)
        {
            return bytes / ONE_GB;
        }

        public static double ConvertToKiloBytes(double bytes)
        {
            return bytes / ONE_KB;
        }

        public static double ConvertToMegaBytes(double bytes)
        {
            return bytes / ONE_MB;
        }

        public static void NormalizeCapacity(double bytes, out double normalizedValue, out string normalizedUnit)
        {
            if (bytes > ONE_GB)
            {
                normalizedValue = ConvertToGigaBytes(bytes);
                normalizedUnit = GigaByte;
            }
            else if (bytes > ONE_MB)
            {
                normalizedValue = ConvertToMegaBytes(bytes);
                normalizedUnit = MegaByte;
            }
            else if (bytes > ONE_KB)
            {
                normalizedValue = ConvertToKiloBytes(bytes);
                normalizedUnit = KiloByte;
            }
            else
            {
                normalizedValue = bytes;
                normalizedUnit = Byte;
            }
        }

        public static string NormalizeCapacity(double bytes, string separtor = " ")
        {
            double value;
            string unit;
            NormalizeCapacity(bytes, out value, out unit);
            return $"{value.ToString("N")}{separtor}{unit}";
        }

        public static void NormalizeRate(double bytes, TimeSpan time, TimeSpan timeUnit, out double normalizedValue,
            out string normalizedUnit)
        {
            var rate = bytes * timeUnit.TotalMilliseconds / time.TotalMilliseconds;
            NormalizeCapacity(rate, out normalizedValue, out normalizedUnit);
        }
        #endregion
    }
}
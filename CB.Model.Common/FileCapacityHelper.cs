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
        public static void Normalize(double bytes, out double value, out string unit)
        {
            if (bytes > ONE_GB)
            {
                value = bytes / ONE_GB;
                unit = GigaByte;
            }
            else if (bytes > ONE_MB)
            {
                value = bytes / ONE_MB;
                unit = MegaByte;
            }
            else if (bytes > ONE_KB)
            {
                value = bytes / ONE_KB;
                unit = KiloByte;
            }
            else
            {
                value = bytes;
                unit = Byte;
            }
        }

        public static string Normalize(double bytes, string separtor = " ")
        {
            double value;
            string unit;
            Normalize(bytes, out value, out unit);
            return $"{value}{separtor}{unit}";
        }
        #endregion
    }
}
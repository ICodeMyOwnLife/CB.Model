using System;
using System.ComponentModel;
using System.Globalization;


namespace CB.Model.Common
{
    public class ProgressConverter: TypeConverter
    {
        #region Override
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(double?) || sourceType == typeof(string);

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            => destinationType == typeof(double?) || destinationType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            double? progress;

            var valueString = value as string;
            if (valueString != null)
            {
                double progressValue;
                progress = double.TryParse(valueString, out progressValue) ? progressValue : (double?)null;
            }
            else
            {
                progress = value as double?;
            }

            return new Progress(progress);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            var progress = (Progress)value;
            return destinationType == typeof(double?)
                       ? (object)(double?)progress : destinationType == typeof(string) ? progress.ToString() : null;
        }
        #endregion
    }
}
using System;

namespace NewPPT.Utils.YouTubeDownloader
{
    public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
    {
        private const string FileSizeFormat = "fs", SpeedFormat = "s";
        private const decimal OneKiloByte = 1024M;
        private const decimal OneMegaByte = OneKiloByte*1024M;
        private const decimal OneGigaByte = OneMegaByte*1024M;

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null || (!format.StartsWith(FileSizeFormat) && !format.StartsWith(SpeedFormat)))
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            if (arg is string)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            decimal size;

            try
            {
                size = Convert.ToDecimal(arg);
            }
            catch (InvalidCastException)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            string suffix;
            if (size > OneGigaByte)
            {
                size /= OneGigaByte;
                suffix = "GB";
            }
            else if (size > OneMegaByte)
            {
                size /= OneMegaByte;
                suffix = "MB";
            }
            else if (size > OneKiloByte)
            {
                size /= OneKiloByte;
                suffix = "KB";
            }
            else
            {
                suffix = "Bytes";
            }
            if (format.StartsWith(SpeedFormat)) suffix += "/sec";
            var postion = format.StartsWith(SpeedFormat) ? SpeedFormat.Length : FileSizeFormat.Length;
            var precision = format.Substring(postion);
            if (string.IsNullOrEmpty(precision)) precision = "2";
            return string.Format("{{0:N" + precision + "}}{1}", size, " " + suffix);
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof (ICustomFormatter)) return this;
            return null;
        }

        private static string DefaultFormat(string format, object arg, IFormatProvider formatProvider)
        {
            var formattableArg = arg as IFormattable;
            if (formattableArg != null)
            {
                return formattableArg.ToString(format, formatProvider);
            }
            return arg.ToString();
        }
    }
}
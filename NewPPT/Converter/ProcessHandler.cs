namespace WeCastConvertor.Converter
{
    public static class ProcessHandler
    {
        public static void OnStatusChanged(string message)
        {
            StatusChanged?.Invoke(message);
        }

        public static void OnProgressChanged(int value)
        {
            ProgressChanged?.Invoke(value);
        }

        public static void OnSizeChanged(string totalbytes, string totalbytesexpected)
        {
            SizeChanged?.Invoke(totalbytes, totalbytesexpected);
        }

        public delegate void ChangeStatus(string message);

        public delegate void ChangeProgress(int value);

        public delegate void ChangeUploadSize(string totalBytes, string totalBytesExpected);

        public static event ChangeStatus StatusChanged;
        public static event ChangeProgress ProgressChanged;
        public static event ChangeUploadSize SizeChanged;

       
    }
}
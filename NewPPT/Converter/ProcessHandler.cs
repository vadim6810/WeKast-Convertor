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

        public delegate void ChangeStatus(string message);

        public delegate void ChangeProgress(int value);

        public static event ChangeStatus StatusChanged;
        public static event ChangeProgress ProgressChanged;
    }
}
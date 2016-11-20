namespace WeCastConvertor.Converter
{
    public abstract class Converter
    {
        //private readonly ProcessHandler _processHandler = new ProcessHandler();

        //public ProcessHandler ProcessHandler
        //{
        //    get { return _processHandler; }
        //}

        public abstract string Convert(string file);

    }
}
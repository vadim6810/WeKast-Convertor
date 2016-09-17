using System.Text;

namespace WeCastConvertor.Converter
{
    internal class Encoder : UTF8Encoding
    {
        public override byte[] GetBytes(string s)
        {
            s = s.Replace("\\", "/");
            return base.GetBytes(s);
        }
    }
}
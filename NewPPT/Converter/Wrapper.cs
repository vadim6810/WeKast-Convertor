using System;
using System.Threading.Tasks;
using WeCastConvertor.Forms;

namespace WeCastConvertor.Converter
{
    internal class Wrapper
    {
        public static readonly string[] SupportedFormats = {".pptx", ".ppt"};
        //public static readonly string[] SupportedFormats = {".pptx", ".ppt", ".pdf", ".doc", ".docx"};

        public static Task<Converter> ConvertAsync(Presentation presentation)
        {
            return Task.Run(() =>
            {
                Console.WriteLine(@"Starting convert " + presentation.SourcePath);
                var converter = new Converter(presentation.SourcePath);
                presentation.EzsPath = converter.Convert();
                presentation.Convert = 100;
                Console.WriteLine(@"Presentations converted " + presentation.EzsPath);
                return converter;
            });
        }
    }
}

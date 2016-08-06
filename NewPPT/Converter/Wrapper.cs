using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCastConvertor.Forms;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Converter
{
    internal class Wrapper
    {
        public static Task ConvertAsync(Presentation presentation)
        {
            return Task.Run(() =>
            {
                Console.WriteLine(@"Starting convert " + presentation.SourcePath);
                Converter converter = new Converter(presentation.SourcePath);
                presentation.EzsPath = converter.Convert();
                presentation.Convert = 100;
                Console.WriteLine(@"Presentations converted " + presentation.EzsPath);
            });
        }
    }
}

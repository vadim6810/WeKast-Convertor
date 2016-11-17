using System.Collections.Generic;
using System.Drawing;
using Ghostscript.NET.Rasterizer;

namespace WeCastConvertor.Converter
{
    public class PdfConverter : IConverter
    {
        private const int DpiDefault = 150;

        public string Convert(string file)
        {
            var ezsWorker = new EzsWorker(file);
            var images = GetImages(file);
            ezsWorker.
            foreach (var image in images)
            {
                ezsWorker.AddSlide(image);
            }
            return ezsWorker.Save();
        }

        private static IEnumerable<Image> GetImages(string file)
        {
            var result = new LinkedList<Image>();
            using (var rasterizer = new GhostscriptRasterizer())
            {
                rasterizer.Open(file);
                for (var pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                {
                    result.AddLast(rasterizer.GetPage(DpiDefault, DpiDefault, pageNumber));
                }
            }
            return result;
        }
    }
}
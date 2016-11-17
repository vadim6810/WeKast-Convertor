using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            var bitmaps = images as IList<Bitmap> ?? images.ToList();
            ezsWorker.AddPrewew(bitmaps.First());
            foreach (var bitmap in bitmaps)
            {
                ezsWorker.AddSlide(bitmap);
            }
            return ezsWorker.Save();
        }

        private static IEnumerable<Bitmap> GetImages(string file)
        {
            var result = new LinkedList<Bitmap>();
            using (var rasterizer = new GhostscriptRasterizer())
            {
                rasterizer.Open(file);
                for (var pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                {
                    result.AddLast(new Bitmap(rasterizer.GetPage(DpiDefault, DpiDefault, pageNumber)));
                }
            }
            return result;
        }
    }
}
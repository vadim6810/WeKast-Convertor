using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using WeCastConvertor.Properties;
using static System.IO.Directory;

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
            ezsWorker.AddPrewew(bitmaps[0]);
            foreach (var bitmap in bitmaps)
            {
                ezsWorker.AddSlide(bitmap);
            }
            return ezsWorker.Save();
        }

        private static IEnumerable<Bitmap> GetImages(string file)
        {
            var result = new LinkedList<Bitmap>();
            Debug.WriteLine(GetCurrentDirectory());
            Debug.WriteLine(Environment.CurrentDirectory);
            var path = Path.GetFullPath(file);
            Stream fileStream = new FileStream(file, FileMode.Open);
            using (var rasterizer = new GhostscriptRasterizer())
            {
                var gvi = new GhostscriptVersionInfo(new Version(0, 0, 0),
                    @"gsdll32.dll", string.Empty, GhostscriptLicense.GPL);
                rasterizer.Open(fileStream, gvi, false);
                for (var pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                {
                    result.AddLast(new Bitmap(rasterizer.GetPage(DpiDefault, DpiDefault, pageNumber)));
                }
            }
            return result;
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
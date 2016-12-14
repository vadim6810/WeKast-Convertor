using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using static System.IO.Directory;

namespace WeCastConvertor.Converter
{
    public class PdfConverter : Converter
    {
        private const int DpiDefault = 96;

        public override string Convert(string file)
        {
            ProcessHandler.OnStatusChanged("Creating ezs...");
            var ezsWorker = new EzsWorker(file);
            ProcessHandler.OnStatusChanged("Converting...");
            var images = GetImages(file);
            var bitmaps = images as IList<Bitmap> ?? images.ToList();
            ezsWorker.AddPrewew(bitmaps[0]);
            ProcessHandler.OnStatusChanged("Saving...");
            int current = 0;
            foreach (var bitmap in bitmaps)
            {
                ezsWorker.AddSlide(bitmap);
                current++;
                ProcessHandler.OnProgressChanged((int) (100 * current /(float) bitmaps.Count));
            }
            return ezsWorker.Save();
        }

        private IEnumerable<Bitmap> GetImages(string file)
        {
            var result = new LinkedList<Bitmap>();
            Debug.WriteLine(GetCurrentDirectory());
            Debug.WriteLine(Environment.CurrentDirectory);
            var path = Path.GetFullPath(file);
            Stream fileStream = new FileStream(file, FileMode.Open);
            using (var rasterizer = new GhostscriptRasterizer( ))
            {
                //rasterizer.CustomSwitches.Add(" -background white ");
                var gvi = new GhostscriptVersionInfo(new Version(0, 0, 0),
                    @"gsdll32.dll", string.Empty, GhostscriptLicense.GPL);
                Debug.WriteLine(gvi.Version);
                rasterizer.Open(fileStream, gvi, false);
                for (var pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                {
                    result.AddLast(new Bitmap(rasterizer.GetPage(DpiDefault, DpiDefault, pageNumber)));
                    ProcessHandler.OnProgressChanged(100 * pageNumber / rasterizer.PageCount);
                }
            }
            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;

namespace WeCastConvertor.Converter
{
    public class PdfConverter : IConverter
    {
        private static readonly int DPI_DEFAULT = 144;

        public string Convert(string file)
        {
            List<Image> images = GetImages(file);
            return null;
        }

        private static List<Image> GetImages(string file)
        {
            List<Image> result = new List<Image>();
            using (var rasterizer = new GhostscriptRasterizer())
            {
                rasterizer.Open(file);//, localDllInfo, false);
                for (var pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                {
                    result.Add(rasterizer.GetPage(DPI_DEFAULT, DPI_DEFAULT, pageNumber));
                }
            }
            return result;
        }


        //public void LoadImage(string InputPDFFile, int PageNumber)
        //{
        //    var outImageName = Path.GetFileNameWithoutExtension(InputPDFFile);
        //    outImageName = outImageName + "_" + PageNumber + "_.jpg";


        //    var dev = new GhostscriptJpegDevice(GhostscriptJpegDeviceType.Jpeg);
        //    dev.GraphicsAlphaBits = GhostscriptImageDeviceAlphaBits.V_4;
        //    dev.TextAlphaBits = GhostscriptImageDeviceAlphaBits.V_4;
        //    dev.ResolutionXY = new GhostscriptImageDeviceResolution(290, 290);
        //    dev.InputFiles.Add(InputPDFFile);
        //    dev.Pdf.FirstPage = 1;
        //    dev.Pdf.LastPage = 10;
        //    dev.CustomSwitches.Add("-dDOINTERPOLATE");
        //    dev.OutputPath = @"d:/" + outImageName;
        //    try
        //    {
        //        dev.Process();
        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show(exception.Message);
        //    }
        //}

     
     
    }
}
using System;
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
            Sample_1();
            Test(file);
            //PdfReader.unethicalreading = true;

            ////SplitePDF(@"d:/access.pdf");
            //PdfReader reader = new FdfReader(@"d:/access.pdf");
            //var pages = reader.NumberOfPages;
            //reader.Close();
            //for (int page = 1; page <= pages; page++)
            //{
            //    LoadImage(@"d:/access.pdf", page);
            //}
            ////throw new NotImplementedException();
            return null;
        }

        private void Sample_1()
        {
            var gsVersions = GhostscriptVersionInfo.GetInstalledVersions();

            foreach (var gsv in gsVersions)
            {
                Console.WriteLine("Installed " + gsv.LicenseType + " Ghostscript " + gsv.Version);
            }

            var lastVersion = GhostscriptVersionInfo.GetLastInstalledVersion();

            Console.WriteLine("Ghostscript version used in this sample: " +
                              lastVersion.LicenseType + " Ghostscript " + lastVersion.Version);
        }

        private static void Test(string file)
        {
            var localGhostscriptDll = Path.Combine(Environment.CurrentDirectory, "gsdll32.dll");
            var localDllInfo = new GhostscriptVersionInfo(localGhostscriptDll);//@"d:\Users\Misteric\Source\Repos\WeKast-Convertor\NewPPT\gsdll32.dll");


            var desired_x_dpi = DPI_DEFAULT;
            var desired_y_dpi = DPI_DEFAULT;

            //var inputPdfPath = @"d:/access.pdf";

            var outputPath = @"d:/"; //Environment.CurrentDirectory;

            var _rasterizer = new GhostscriptRasterizer();

            _rasterizer.Open(file, localDllInfo, false);

            for (var pageNumber = 1; pageNumber <= _rasterizer.PageCount; pageNumber++)
            {
                var pageFilePath = Path.Combine(outputPath, "Page -" + pageNumber + ".png");

                var img = _rasterizer.GetPage(desired_x_dpi, desired_y_dpi, pageNumber);
                img.Save(pageFilePath, ImageFormat.Png);
            }

            _rasterizer.Close();
        }

        

        public void LoadImage(string InputPDFFile, int PageNumber)
        {
            var outImageName = Path.GetFileNameWithoutExtension(InputPDFFile);
            outImageName = outImageName + "_" + PageNumber + "_.jpg";


            var dev = new GhostscriptJpegDevice(GhostscriptJpegDeviceType.Jpeg);
            dev.GraphicsAlphaBits = GhostscriptImageDeviceAlphaBits.V_4;
            dev.TextAlphaBits = GhostscriptImageDeviceAlphaBits.V_4;
            dev.ResolutionXY = new GhostscriptImageDeviceResolution(290, 290);
            dev.InputFiles.Add(InputPDFFile);
            dev.Pdf.FirstPage = 1;
            dev.Pdf.LastPage = 10;
            dev.CustomSwitches.Add("-dDOINTERPOLATE");
            dev.OutputPath = @"d:/" + outImageName;
            try
            {
                dev.Process();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        //private void SplitePDF(string filepath)
        //{
        //    var currentPage = 1;
        //    //string filepath_New = filepath + "\\PDFDestination\\";

        //    // var encoding = new UTF8Encoding();
        //    //byte[] arrayofPassword = encoding.GetBytes(ExistingFilePassword);
        //    var reader = new PdfReader(filepath);
        //    reader.RemoveUnusedObjects();
        //    var pageCount = reader.NumberOfPages;
        //    var ext = Path.GetExtension(filepath);
        //    for (var i = 1; i <= pageCount; i++)
        //    {
        //        var reader1 = new PdfReader(filepath);
        //        var outfile = filepath.Replace(Path.GetFileName(filepath),
        //            Path.GetFileName(filepath).Replace(".pdf", "") + "_" + i + ext);
        //        reader1.RemoveUnusedObjects();
        //        var doc = new Document(reader.GetPageSizeWithRotation(currentPage));
        //        var pdfCpy = new PdfCopy(doc, new FileStream(outfile, FileMode.Create));
        //        doc.Open();
        //        for (var j = 1; j <= 1; j++)
        //        {
        //            var page = pdfCpy.GetImportedPage(reader1, currentPage);
        //            pdfCpy.SetFullCompression();
        //            pdfCpy.AddPage(page);
        //            currentPage += 1;
        //        }
        //        doc.Close();
        //        pdfCpy.Close();
        //        reader1.Close();
        //        reader.Close();
        //    }
        //}
        //[DllImport("gsdll32.dll", EntryPoint = "gsapi_new_instance")]
        //private static extern int gsapi_new_instance(out IntPtr pinstance, IntPtr caller_handle);

        //[DllImport("gsdll32.dll", EntryPoint = "gsapi_init_with_args")]
        //private static extern int gsapi_init_with_args(IntPtr instance, int argc, IntPtr argv);

        //[DllImport("gsdll32.dll", EntryPoint = "gsapi_exit")]
        //private static extern int gsapi_exit(IntPtr instance);

        //[DllImport("gsdll32.dll", EntryPoint = "gsapi_delete_instance")]
        //private static extern void gsapi_delete_instance(IntPtr instance);

        public void cctwdf()
        {
            ////Transform pdf to jpg
            //PdfToImage.PDFConvert pp = new PDFConvert();
            //pp.OutputFormat = "jpeg"; //format
            //pp.JPEGQuality = 100; //100% quality
            //pp.ResolutionX = 300; //dpi
            //pp.ResolutionY = 300;
            //pp.FirstPageToConvert = 1; //pages you want
            //pp.LastPageToConvert = 1;
            //pp.Convert(PATH_PDF + "report.pdf", PATH_IMAGE + "name.jpg");
        }

    }
}
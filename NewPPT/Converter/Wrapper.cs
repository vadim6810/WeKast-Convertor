using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using Presentation = WeCastConvertor.Forms.Presentation;

namespace WeCastConvertor.Converter
{
    internal class Wrapper
    {
        static readonly Dictionary<string, Converter> Converters = new Dictionary<string, Converter>()
        {
            {".ppt",new PptConverter()},
            {".pptx",new PptConverter()},
            {".pdf",new PdfConverter()},
             {".doc",new WordConverter()},
             {".docx",new WordConverter()}
        };

        //Queue<String> FileQu

        //private static readonly string[] SupportedFormats = { ".PPTX", ".PPT", ".PDF", ".DOC", ".DOCX" };
        private static readonly HashSet<string> SupportedFormats = new HashSet<string>{ ".PPTX", ".PPT", ".PDF", ".DOC", ".DOCX" };

        public static void Convert(Presentation presentation)
        {
            Console.WriteLine(@"Starting convert " + presentation.SourcePath);
            var extension = Path.GetExtension(presentation.SourcePath);
            if (extension == null || !Converters.ContainsKey(extension)) return;
            var converter = Converters[extension];
            try
            {
                presentation.EzsPath = converter.Convert(presentation.SourcePath);
                presentation.Convert = 100;
                Console.WriteLine(@"Presentations converted " + presentation.EzsPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (e.InnerException != null)
                    Console.WriteLine(e.InnerException);
                Console.WriteLine(e.StackTrace);
            }
        }

        public static void AddFiles(string[] files)
        {
            var images = files.Where(IsImage).OrderBy(Path.GetFileName).Select(Image.FromFile).ToArray();
            CreateImagePresantation(images);
            //foreach (var image in images)
            //{
            //    Console.WriteLine(image);
            //}
            var supportedFiles = files.Where(file => SupportedFormats.Contains(Path.GetExtension(file)?.ToUpper())).ToArray();
            foreach (var file in supportedFiles)
            {
                Console.WriteLine(file);
            }
            foreach (var file in files)
            {
                // Пропускаем неподдерживаемые форматы
                //if (!Array.Exists(Wrapper.SupportedFormats, s => s.Equals(Path.GetExtension(file)))) continue;

                Console.WriteLine(file);
                //AppendLog(file);
                var presentation = new Presentation { SourcePath = file };
                /*await*/
                //var convertionResult = Convert(presentation);
                //if (convertionResult)
                //{
                //    ShowStatus("Convertion successfull");
                //}
                //else
                //{
                //    ShowStatus("Convertion fail");
                //}
            }
        }

        private static void CreateImagePresantation(Image[] images)
        {
            

        }

        private static bool IsImage(string file)
        {
            try
            {
                System.Drawing.Image imgInput = System.Drawing.Image.FromFile(file);
                System.Drawing.Graphics gInput = System.Drawing.Graphics.FromImage(imgInput);
                System.Drawing.Imaging.ImageFormat thisFormat = imgInput.RawFormat;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}

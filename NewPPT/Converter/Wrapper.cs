using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Office.Interop.PowerPoint;
using Presentation = WeCastConvertor.Forms.Presentation;

namespace WeCastConvertor.Converter
{
    internal class Wrapper
    {
        static Dictionary<string, Converter> converters = new Dictionary<string, Converter>()
        {
            {".ppt",new PptConverter()},
            {".pptx",new PptConverter()},
            {".pdf",new PdfConverter()}
        };

        public static readonly string[] SupportedFormats = { ".pptx", ".ppt", ".pdf" };

        //public static Converter.ChangeStatus StatusChanged { get; set; }
        //public static Converter.ChangeProgress ProgressChanged { get; set; }

        //public static readonly string[] SupportedFormats = {".pptx", ".ppt", ".pdf", ".doc", ".docx"};

        public static Task<Converter> ConvertAsync(Presentation presentation)
        {
            return Task.Run(() =>
            {
                Console.WriteLine(@"Starting convert " + presentation.SourcePath);
                //var converter = new PptConverter(presentation.SourcePath);
                var converter = converters[Path.GetExtension(presentation.SourcePath)];
                //ProcessHandler.StatusChanged += StatusChanged;
                //ProcessHandler.ProgressChanged += ProgressChanged;
                presentation.EzsPath = converter.Convert(presentation.SourcePath);
                presentation.Convert = 100;
                Console.WriteLine(@"Presentations converted " + presentation.EzsPath);
                return converter;
            });
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using static Microsoft.Office.Core.MsoShapeType;
using WordApplication = Microsoft.Office.Interop.Word.Application;

namespace WeCastConvertor.Converter
{
    class WordConverter : Converter
    {
        private readonly WordApplication wordApp = new WordApplication();
        private EzsWorker ezsWorker;
        public override string Convert(string file)
        {
            String res = null;
            try
            {
                var document = wordApp.Documents.Open(file);
                if (ezsWorker == null)
                    ezsWorker = new EzsWorker(file);
                var pathToPdf = Path.Combine(ezsWorker.EzsTemp, Path.GetFileNameWithoutExtension(file) + ".pdf");
                Debug.WriteLine(pathToPdf);
                document.ExportAsFixedFormat(pathToPdf, WdExportFormat.wdExportFormatPDF);
                var pdfconverter = new PdfConverter(ezsWorker);
                pdfconverter.Convert(pathToPdf);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, @"Open file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return res;
        }
    }
}

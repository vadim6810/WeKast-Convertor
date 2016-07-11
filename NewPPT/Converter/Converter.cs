using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;

namespace NewPPT.Converter
{
    class Converter
    {
        public static void convert()
        {
            Application pw = new Application();
            string presName = @"d:\flip_split.pptx";
            Presentation pres = pw.Presentations.Open(presName);
            string vName = System.IO.Path.GetFileNameWithoutExtension(presName) + ".mp4";
            //pres.SlideShowSettings.
            pres.SaveAs(vName,PpSaveAsFileType.ppSaveAsMP4,MsoTriState.msoCTrue);
            while (false)
            {
                
            }
        }
    }


}

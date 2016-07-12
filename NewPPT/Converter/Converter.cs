using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using NewPPT.Utils;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

namespace NewPPT.Converter
{
    class Converter
    {
        private static FrmMain _form;
        public static void Convert(FrmMain frmMain)
        {
            _form = frmMain;
            Log("Start Power Point");
            Application pw = new Application();
            EventLogger el = new EventLogger(frmMain);

            string presName = @"d:\flip_split.pptx";
            Presentation pres = pw.Presentations.Open(presName);
            string vName = System.IO.Path.GetFileNameWithoutExtension(presName) + ".mp4";
            foreach (Slide slide in pres.Slides)
            {
                int steps = slide.PrintSteps; //MessageBox();
            }
            //pres.SlideShowSettings.
            //pres.SaveAs(vName,PpSaveAsFileType.ppSaveAsMP4,MsoTriState.msoCTrue);
            while (false)
            {
                
            }
        }

        private static void Log(string s) => _form.AppendLog(DateTime.Now.ToString("hh:mm:ss")+": "+ s);
     
    }


}

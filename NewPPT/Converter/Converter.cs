using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
        static EventLogger _el;
        LinkedList<int> _durations;
        static Application pw;

        public static void Convert(FrmMain frmMain)
        {
            _form = frmMain;
            pw = new Application();
            _el = new EventLogger(frmMain, pw);
            _el.AttachEvents();
            const string presName = @"d:\flip_split.pptx";
            var pres = pw.Presentations.Open(presName);
            string vName = System.IO.Path.GetFileNameWithoutExtension(presName) + ".mp4";
            Log("vName=" + vName);
            ParseExternalMedia(pres);
            SetNullTrnsactDurations(pres);
            ExportToMp4(pres);
            //pres.SlideShowSettings.
            //pres.SaveAs(vName,PpSaveAsFileType.ppSaveAsMP4,MsoTriState.msoCTrue);
            //pres.SlideShowSettings.Run();
            //while(pw.SlideShowWindows.Count >= 1)
            //{
            //    System.Windows.Forms.Application.DoEvents();
            //    Thread.Sleep(1000);
            //}
            while (false)
            {

            }
            _el.DetachEvents();
            //pres.Close();
            //Marshal.ReleaseComObject(pw); 
        }

        private static void ParseExternalMedia(Presentation pres)
        {
            foreach (Slide slide in pres.Slides)
            {
                var isFileDownloaded = false;
                var tempResFolderPath = Environment.GetEnvironmentVariable("TEMP");
                var slideShapes = slide.Shapes;
                foreach (Microsoft.Office.Interop.PowerPoint.Shape shape in slideShapes)
                {
                    if (shape.Type == MsoShapeType.msoEmbeddedOLEObject
                        || shape.Type == MsoShapeType.msoOLEControlObject
                        || shape.Type == MsoShapeType.msoLinkedOLEObject)
                    {
                        try
                        {
                            //ShockwaveFlashObjects.ShockwaveFlash objFlash = (ShockwaveFlashObjects.ShockwaveFlash)shape.OLEFormat.Object;
                            //downloadAndSaveMedia(objFlash.Movie, tempResFolderPath, destDirectory, slide);
                            isFileDownloaded = true;
                        }
                        catch (Exception e)
                        {
                        }
                        //string moviePath = objFlash.Movie;
                        //System.Windows.Forms.MessageBox.Show(" Slide  : " + moviePath);
                    }
                    //if (!isFileDownloaded)
                    //{
                    //    extractDataFromURLByHyperlink(destDirectory, slide);
                    //}

                    if (shape.Type == MsoShapeType.msoMedia &&
                        shape.MediaType == PpMediaType.ppMediaTypeMovie)
                    {
                        var isLocalFile = true;


                        var videoPath = string.Empty;
                        try
                        {
                            videoPath = shape.LinkFormat.SourceFullName; // contains the movie path 
                                                                         //get the path like this
                            //////////getTargetVideoFile(videoPath, destDirectory, slide.SlideIndex);
                            //   System.Windows.Forms.MessageBox.Show(" After downloading : " + videoPath);
                        }
                        catch (Exception e)
                        {
                            isLocalFile = false;
                            //System.Windows.Forms.MessageBox.Show(e.Message);
                        }
                        //or use 
                        //System.IO.File.Copy(shape.LinkFormat.SourceFullName, destDirectory); 
                        //to export them
                        // shape.OLEFormat.Object.

                        if (!isLocalFile)
                        {
                            var destination = tempResFolderPath + "\\v" + slide.SlideIndex + "\\";
                            if (!Directory.Exists(destination))
                                Directory.CreateDirectory(destination);
                            slide.PublishSlides(destination, true, true);
                            var filePaths = Directory.GetFiles(destination);
                            if (filePaths.Length != 0)
                            {
                                //for (var i = 0; i < filePaths.Length; i++)
                                //    extractMediaFromSlide(filePaths[i], destDirectory, slide.SlideIndex);
                            }
                        }
                        // SaveSlideAsWmvFile(destination, objPres, destDirectory);
                        //objPres.ExportAsFixedFormat("c:\\temp\\slide\\example.pdf", PpFixedFormatType.ppFixedFormatTypePDF,
                        //PpFixedFormatIntent.ppFixedFormatIntentScreen);
                        //pptPresentation.SaveAs("c:\\temp\\slide\\example.html", PpSaveAsFileType.ppSaveAsHTMLDual);
                        //pptPresentation.SaveAs("c:\\temp\\slide\\exampledual.html", PpSaveAsFileType.ppSaveAsHTMLDual);
                    }
                }
                //}

                
            }
            //var filePath = MediaDownloaderClass.downloadMediaFile(address, tempPath, targetYoutubeVideo);

        }

        private static void ExportToMp4(Presentation pres)
        {
            try
            {
                var strPath = @"D:\";//Path.GetTempPath();
                Log("Temp path is "+strPath);
                //Environment.GetFolderPath("Temp");
                var nFile = pres.Name;
                nFile = Path.GetFileNameWithoutExtension(pres.Name)+".mp4";
                Log("Presentation name is "+nFile);
                pres.SaveAs(Path.Combine(strPath, nFile), PpSaveAsFileType.ppSaveAsMP4, MsoTriState.msoTrue);
                while (pw.ActivePresentation.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusInProgress || pw.ActivePresentation.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusQueued)
                {
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(500);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private static void SetNullTrnsactDurations(Presentation pres)
        {
            int slidesCount = pres.Slides.Count;
            int[] SlideIdx = new int[slidesCount];
            for (int i = 0; i < slidesCount; i++) SlideIdx[i] = i + 1;
            var objSldRng = pres.Slides.Range(SlideIdx);
            var objSST = objSldRng.SlideShowTransition;
            objSST.AdvanceOnTime = MsoTriState.msoTrue;
            objSST.AdvanceTime = 0;
            //objSST.EntryEffect = PpEntryEffect.ppEffectBoxOut;
            //var master = slide.Master;
            //master.SlideShowTransition.AdvanceOnTime = MsoTriState.msoTrue;
            //Log(string.Format("slide{0} AdvenceOnTime : {1}",slide.SlideNumber,master.SlideShowTransition.AdvanceOnTime));
            //master.SlideShowTransition.AdvanceTime = 0;
            //Log(string.Format("slide{0} AdvenceTime : {1}", slide.SlideNumber,master.SlideShowTransition.AdvanceTime));
            //master.SlideShowTransition.;
            //foreach (Effect anim in slide.TimeLine.MainSequence)
            //{
            //    anim.Timing.Duration = 0;
            //}
        }

        private static void Log(string s) => _form.AppendLog(DateTime.Now.ToString("hh:mm:ss") + ": " + s);

    }


}

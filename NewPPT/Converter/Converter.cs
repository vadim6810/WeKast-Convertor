using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using WeCastConvertor.Utils;
using Shape = Microsoft.Office.Interop.PowerPoint.Shape;

namespace WeCastConvertor.Converter
{
    internal class Converter
    {
        private static FrmMain _form;
        private static EventLogger _el;
        private static Application _pw;
        private LinkedList<int> _durations;

        public static void Convert(FrmMain frmMain)
        {
            _form = frmMain;
            _pw = new Application();
            _el = new EventLogger(frmMain, _pw);
            _el.AttachEvents();
            const string presName = @"d:\flip_split.pptx";
            var pres = _pw.Presentations.Open(presName);
            var vName = Path.GetFileNameWithoutExtension(presName) + ".mp4";
            Log("vName=" + vName);
            ParseExternalMedia(pres);
            SetNullTrnsactDurations(pres);
            ExportToMp4(pres);
            _el.DetachEvents();
            pres.Close();
            _pw.Quit();
        }

        private static void ParseExternalMedia(Presentation pres)
        {
            foreach (Slide slide in pres.Slides)
            {
                var isFileDownloaded = false;
                var tempResFolderPath = Environment.GetEnvironmentVariable("TEMP");
                var slideShapes = slide.Shapes;
                foreach (Shape shape in slideShapes)
                {
                    if (shape.Type == MsoShapeType.msoEmbeddedOLEObject
                        || shape.Type == MsoShapeType.msoOLEControlObject
                        || shape.Type == MsoShapeType.msoLinkedOLEObject)
                    {
                        Log(string.Format("SLide{0} shapeid={1} shapeType=",slide.SlideNumber,shape.Id,shape.Type));
                        try
                        {
                            //ShockwaveFlashObjects.ShockwaveFlash objFlash = (ShockwaveFlashObjects.ShockwaveFlash)shape.OLEFormat.Object;
                            //downloadAndSaveMedia(objFlash.Movie, tempResFolderPath, destDirectory, slide);
                            isFileDownloaded = true;
                        }
                        catch (Exception exception)
                        {
                            Log(exception.Message);
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
                var strPath = @"D:\"; //Path.GetTempPath();
                Log("Temp path is " + strPath);
                //Environment.GetFolderPath("Temp");
                var nFile = Path.GetFileNameWithoutExtension(pres.Name) + ".mp4";
                Log("Presentation name is " + nFile);
                pres.SaveAs(Path.Combine(strPath, nFile), PpSaveAsFileType.ppSaveAsMP4, MsoTriState.msoTrue);
                while (_pw.ActivePresentation.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusInProgress ||
                       _pw.ActivePresentation.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusQueued)
                {
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(500);
                }
            }
            catch (Exception exception)
            {
                Log(exception.Message);
            }
        }

        private static void SetNullTrnsactDurations(Presentation pres)
        {
            var slidesCount = pres.Slides.Count;
            var slideIndex = new int[slidesCount];
            for (var i = 0; i < slidesCount; i++) slideIndex[i] = i + 1;
            var objSldRng = pres.Slides.Range(slideIndex);
            var objSst = objSldRng.SlideShowTransition;
            objSst.AdvanceOnTime = MsoTriState.msoTrue;
            objSst.AdvanceTime = 0;
        }

        private static void Log(string s) => _form.AppendLog(DateTime.Now.ToString("hh:mm:ss") + ": " + s);
    }
}
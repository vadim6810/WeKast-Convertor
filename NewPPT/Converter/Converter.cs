using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using mshtml;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using WeCastConvertor.Utils;
using WeCastConvertor.Utils.YouTubeDownloader;
using YoutubeExtractor;
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
            //ParseExternalMedia(pres);
            //SetNullTrnsactDurations(pres);
            pres.CreateVideo(@"d:\video.mp4",true,0,1080,30,100);
            while (pres.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusInProgress ||
                pres.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusDone)
            {
                System.Windows.Forms.Application.DoEvents(); 
                Log(pres.CreateVideoStatus.ToString());
                Thread.Sleep(2000);
            }
            //ExportToMp4(pres);
            _el.DetachEvents();
            //pres.Close();
            //_pw.Quit();
        }

        private static void ParseExternalMedia(Presentation pres)
        {
            
            foreach (Slide slide in pres.Slides)
            {
                Log(string.Format("Parsing slide{0}:", slide.SlideNumber));
                ShowAllHiperlinks(slide);
                ShowAllComments(slide);
                ShowAllNotes(slide);
                ShowAllShapes(slide);
                if (ExistYouTubeVideo(slide))
                {
                    string videoPath = DownloadVideoFromYouTube(slide);
                    CreateNewVideoShape(slide, videoPath);
                }
                var isFileDownloaded = false;
                var tempResFolderPath = Environment.GetEnvironmentVariable("TEMP");
                var slideShapes = slide.Shapes;
                foreach (Shape shape in slideShapes)
                {
                    if (shape.Type == MsoShapeType.msoEmbeddedOLEObject
                        || shape.Type == MsoShapeType.msoOLEControlObject
                        || shape.Type == MsoShapeType.msoLinkedOLEObject)
                    {
                        Log(string.Format("SLide{0} shapeid={1} shapeType={2}", slide.SlideNumber, shape.Id, shape.Type));
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


                        string videoPath = string.Empty;
                        try
                        {
                            videoPath = shape.LinkFormat.SourceFullName; // contains the movie path 
                            Log(videoPath);
                            //var urls = YouTubeDownloader.GetYouTubeVideoUrls(videoPath);
                            //var list = YouTubeDownloader.GetQuality(urls);
                            var urls = YouTubeDownloader.GetYouTubeVideoUrls(videoPath);

                            YouTubeVideoQuality[] arr = urls.ToArray();
                            YouTubeVideoQuality tempItem = arr[0];
                            var temp = tempItem.VideoTitle.Replace(@"\", "").Replace("&#39;", "'").Replace("&quot;", "'").Replace("&lt;", "(").Replace("&gt;", ")").Replace("+", " ").Replace(":", "-");
                            string FileName = temp + "." + tempItem.Extention;

                            //downloadedFilePath = Path.Combine(targetPath, FileName);
                            //initFileDownloader(tempItem.DownloadUrl, targetPath, FileName);
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

        private static void CreateNewVideoShape(Slide slide, string videoPath)
        {
            videoPath = @"c:\Users\Misteric\Documents\WeKast - Plug & Cast Solution Makes Presentations Easier!.mp4";
            //Shape newSape = slide.Shapes.AddMediaObject2(videoPath, MsoTriState.msoTrue, MsoTriState.msoFalse, Left, top, Width, Height);

        }

        private static string DownloadVideoFromYouTube(Slide slide)
        {
            //throw new NotImplementedException();
            return string.Empty;
        }

        private static bool ExistYouTubeVideo(Slide slide)
        {
            return slide.Shapes.Cast<Shape>().Any(shape => shape.Type == MsoShapeType.msoMedia && shape.LinkFormat.SourceFullName != null);
        }

        private static void ShowAllShapes(Slide slide)
        {
            foreach (Shape shape in slide.Shapes)
            {
                if (shape.Type == MsoShapeType.msoMedia &&
                    shape.MediaType == PpMediaType.ppMediaTypeMovie && shape.LinkFormat.SourceFullName != null)// Log($"Shape type {shape.Type}");
                //else
                {
                    var videoPath = shape.LinkFormat.SourceFullName;
                    //Log($"{shape.LinkFormat.SourceFullName}");
                    //    IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(videoPath, false);
                    //    VideoInfo video = videoInfos
                    //.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);
                    //    if (video.RequiresDecryption)
                    //    {
                    //        DownloadUrlResolver.DecryptDownloadUrl(video);
                    //    }
                    //    var videoDownloader = new VideoDownloader(video,
                    //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    //RemoveIllegalPathCharacters(video.Title) + video.VideoExtension));

                    //    // Register the ProgressChanged event and print the current progress
                    //    videoDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);

                    //    /*
                    //     * Execute the video downloader.
                    //     * For GUI applications note, that this method runs synchronously.
                    //     */
                    //    videoDownloader.Execute();
                    //    //var urls = YouTubeDownloader.GetYouTubeVideoUrls(videoPath);

                    //    //YouTubeVideoQuality[] arr = urls.ToArray();
                    //YouTubeVideoQuality tempItem = arr[0];
                    //var temp = tempItem.VideoTitle.Replace(@"\", "").Replace("&#39;", "'").Replace("&quot;", "'").Replace("&lt;", "(").Replace("&gt;", ")").Replace("+", " ").Replace(":", "-");
                    //string FileName = temp + "." + tempItem.Extention;
                    float Left = shape.Left+10;
                    var top = shape.Top+10;
                    float Width = shape.Width-20;
                    float Height = shape.Height-20;

                    //slide.Shapes. shape.r
                    }
            }
        }

        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        private static void ShowAllNotes(Slide slide)
        {
            if (slide.HasNotesPage != MsoTriState.msoTrue) return;
            var notesPages = slide.NotesPage;
            foreach (var shape in from Shape shape in notesPages.Shapes where shape.Type == MsoShapeType.msoPlaceholder where shape.PlaceholderFormat.Type == PpPlaceholderType.ppPlaceholderBody select shape)
            {
                Log($"Slide[{slide.SlideIndex}] Notes: [{shape.TextFrame.TextRange.Text}]");
            }
        }

        private static void ShowAllComments(Slide slide)
        {
            foreach (Comment comment in slide.Comments)
            {
                try
                {
                    Log($"comment: {comment.Text}");
                }
                catch (Exception exception)
                {
                    Log(exception.Message);
                }
            }
        }

        private static void ShowAllHiperlinks(Slide slide)
        {
            foreach (Hyperlink h in slide.Hyperlinks)
            {
                try
                {
                    Log($"hiperlink: {h.Address}     {h.SubAddress}      {h.TextToDisplay}");
                }
                catch (Exception exception)
                {
                    Log(exception.Message);
                }
            }
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
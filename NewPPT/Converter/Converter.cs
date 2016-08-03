using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using WeCastConvertor.Utils;
using WeCastConvertor.Utils.ZipDocument;
using YoutubeExtractor;
using static Microsoft.Office.Core.MsoShapeType;
using Shape = Microsoft.Office.Interop.PowerPoint.Shape;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Xml;


namespace WeCastConvertor.Converter
{
    internal class Converter
    {
        private static ILogger _logger;
        private static EventLogger _el;
        private static Application _pw;
        private LinkedList<int> _durations;
        static string _tempFolderPath = Environment.GetEnvironmentVariable("TEMP");
        static string ezsFolder = _tempFolderPath + @"\EZSTemp";
        static string commentsFolder = ezsFolder + @"\comments";
        static string videosFolder = ezsFolder + @"\videos";
        static string audioFolder = ezsFolder + @"\audio";
        static string animFolder = ezsFolder + @"\animations";
        //static string tempCopy = @"d:\Users\Misteric\AppData\Local\Temp\EZSTemp\test2.rar";
        static string tempCopy = string.Empty;
        //static LinkedList<string> tempPictures = new LinkedList<string>();

        public static void Convert(string fileName, ILogger iLogger)
        {
            _logger = iLogger;
            _pw = new Application();
            _el = new EventLogger(iLogger, _pw);
            _el.AttachEvents();
            //const string presName = @"d:\flip_split.pptx";
            var pres = _pw.Presentations.Open(fileName);
            ParseSlides(pres);
            CreateVideo(pres);
            _el.DetachEvents();
            CleanTempFiles();
            //pres.Close();
            //_pw.Quit();
        }

        private static void CleanTempFiles()
        {
            File.Delete(tempCopy);
        }

        private static void CreateVideo(Presentation pres)
        {
            //The name of the video file to create.
            var fileName = $"{ezsFolder}\\tempVideo.mp4";
            //Indicates whether to use timings and narrations.
            var useTimingsAndNarrations = true;
            //The duration, in seconds, to view the slide.
            var defaultSlideDuration = 0;
            //The resolution of the slide.
            var vertResolution = 1080;
            //The number of frames per second.
            var framesPerSecond = 30;
            //The level of quality of the slide.
            var quality = 100;
            pres.CreateVideo(fileName, useTimingsAndNarrations, defaultSlideDuration, vertResolution, framesPerSecond, quality);
            while (pres.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusInProgress ||
                pres.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusDone)
            {
                System.Windows.Forms.Application.DoEvents();
                Log(pres.CreateVideoStatus.ToString());
                Thread.Sleep(2000);
            }
        }

        private static void ParseSlides(Presentation pres)
        {
            CreateDirrectories();
            foreach (Slide slide in pres.Slides)
            {
                Log($"Parsing slide{slide.SlideNumber}:");
                string outputFile = ezsFolder + "\\" + slide.SlideNumber + ".jpg";
                //slide.Export(outputFile, "jpg", 1440, 1080);
                ExtractAndSaveComments(slide);
                ParseShapes(slide);
                if (ExistEmbeddedMedia(slide))
                {
                    Log($"slide{slide.SlideNumber} contains embedded media");
                    ExtractEmbeddedVideo(slide);
                }
                ChangeMediaShapes(slide);
            }
            //var filePath = MediaDownloaderClass.downloadMediaFile(address, tempPath, targetYoutubeVideo);
        }

        private static void ChangeMediaShapes(Slide slide)
        {
            foreach (Shape shape in slide.Shapes)
            {
                if (shape.Type == msoMedia)
                {
                    //shape.Delete();
                    ChangeShape(shape);
                }
            }
        }

        private static void ChangeShape(Shape shape)
        {
            //tempPictures.AddLast($"{ezsFolder}\\tempPicture{tempPictures.Count}.jpeg"); 
            string tempPicture = $"{ezsFolder}\\tempPicture.jpeg";
            shape.Export(tempPicture, PpShapeFormat.ppShapeFormatJPG);
            Slide slide = shape.Parent;
            Shape newPicture = slide.Shapes.AddPicture(tempPicture, 
                MsoTriState.msoTriStateMixed, 
                MsoTriState.msoFalse, 
                shape.Left, 
                shape.Top, 
                shape.Width, 
                shape.Height);
            shape.Delete();
            File.Delete(tempPicture);
        }

        private static void CreateDirrectories()
        {
            if (!Directory.Exists(ezsFolder))
                Directory.CreateDirectory(ezsFolder);
            else
            {
                Directory.Delete(ezsFolder, true);
                Directory.CreateDirectory(ezsFolder);
            }
            if (!Directory.Exists(animFolder))
                Directory.CreateDirectory(animFolder);
            if (!Directory.Exists(commentsFolder))
                Directory.CreateDirectory(commentsFolder);
            if (!Directory.Exists(videosFolder))
                Directory.CreateDirectory(videosFolder);
            if (!Directory.Exists(audioFolder))
                Directory.CreateDirectory(audioFolder);

        }

        private static void ExtractAndSaveComments(Slide slide)
        {
            if (!Directory.Exists(commentsFolder))
                Directory.CreateDirectory(commentsFolder);
            var path = commentsFolder + $"\\s{slide.SlideNumber}.txt";
            if (File.Exists(path)) return;
            // Create a file to write to. 
            using (var sw = File.CreateText(path))
            {
                foreach (string comment in GetAllCommentsAndNodes(slide))
                {
                    sw.WriteLine(comment);
                }
            }
        }

        private static IEnumerable<VideoInfo> GetAllYoutubeLinks(Slide slide)
        {
            foreach (Shape shape in slide.Shapes)
                try
                {
                    if (shape.Type == msoMedia && shape.LinkFormat.SourceFullName.Contains("youtube.com"))
                        return DownloadUrlResolver.GetDownloadUrls(shape.LinkFormat.SourceFullName, false);
                }
                catch (Exception)
                {
                    //return null;
                }
            return null;
        }

        private static void CreateNewVideoShape(Slide slide, string videoPath)
        {
            videoPath = @"c:\Users\Misteric\Documents\WeKast - Plug & Cast Solution Makes Presentations Easier!.mp4";
            //Shape newSape = slide.Shapes.AddMediaObject2(v0.ideoPath, MsoTriState.msoTrue, MsoTriState.msoFalse, Left, top, Width, Height);

        }

        private static string DownloadVideoFromYouTube(IEnumerable<VideoInfo> videoInfos, int slideNumber)
        {
            if (videoInfos == null)
                return null;
            /*
             * Select the first .mp4 video with 360p resolution
             */
            var video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 720);

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            /*
             * Create the video downloader.
             * The first argument is the video to download.
             * The second argument is the path to save the video file.
             */
            string savePath = Path.Combine(videosFolder,
                //RemoveIllegalPathCharacters(video.Title)
                $"v{slideNumber}" + video.VideoExtension);
            var videoDownloader = new VideoDownloader(video, savePath);

            // Register the ProgressChanged event and print the current progress
            videoDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);

            /*
             * Execute the video downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            //videoDownloader.Execute();
            return savePath;
        }

        private static bool ExistLinkedMedia(Shape shape)
        {
            try
            {
                return shape.MediaFormat.IsLinked;
                //return slide.Shapes.Cast<Shape>().Any(shape => shape.Type == msoMedia && shape.LinkFormat.SourceFullName != null);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool ExistEmbeddedMedia(Slide slide)
        {

            foreach (Shape shape in slide.Shapes)
            {
                try
                {
                    if (shape.Type == msoMedia && shape.MediaFormat.IsEmbedded)
                        return true;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return false;
        }


        // Returns true if slide consist youtube videos
        private static bool ExistYouTubeVideo(Shape shape)
        {
            try
            {
                return shape.LinkFormat.SourceFullName != null && shape.LinkFormat.SourceFullName.Contains("youtube.com");
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void Temp()
        {
            //foreach (Shape shape in slideShapes)
            //{
            //    if (shape.Type == MsoShapeType.msoEmbeddedOLEObject
            //        || shape.Type == MsoShapeType.msoOLEControlObject
            //        || shape.Type == MsoShapeType.msoLinkedOLEObject)
            //    {
            //        Log(string.Format("SLide{0} shapeid={1} shapeType={2}", slide.SlideNumber, shape.Id, shape.Type));
            //        try
            //        {
            //            //ShockwaveFlashObjects.ShockwaveFlash objFlash = (ShockwaveFlashObjects.ShockwaveFlash)shape.OLEFormat.Object;
            //            //downloadAndSaveMedia(objFlash.Movie, _tempFolderPath, destDirectory, slide);
            //            isFileDownloaded = true;
            //        }
            //        catch (Exception exception)
            //        {
            //            Log(exception.Message);
            //        }
            //        //string moviePath = objFlash.Movie;
            //        //System.Windows.Forms.MessageBox.Show(" Slide  : " + moviePath);
            //    }
            //    //if (!isFileDownloaded)
            //    //{
            //    //    extractDataFromURLByHyperlink(destDirectory, slide);
            //    //}

            //    //if (shape.Type == MsoShapeType.msoMedia &&
            //    //    shape.MediaType == PpMediaType.ppMediaTypeMovie)
            //    //{
            //    //    //var isLocalFile = true;


            //    //    //var videoPath = string.Empty;
            //    //    //try
            //    //    //{
            //    //    //    videoPath = shape.LinkFormat.SourceFullName; // contains the movie path 
            //    //    //    Log(videoPath);
            //    //    //    //var urls = YouTubeDownloader.GetYouTubeVideoUrls(videoPath);
            //    //    //    //var list = YouTubeDownloader.GetQuality(urls);
            //    //    //    //var urls = YouTubeDownloader.GetYouTubeVideoUrls(videoPath);

            //    //    //    //YouTubeVideoQuality[] arr = urls.ToArray();
            //    //    //    //YouTubeVideoQuality tempItem = arr[0];
            //    //    //    //var temp = tempItem.VideoTitle.Replace(@"\", "").Replace("&#39;", "'").Replace("&quot;", "'").Replace("&lt;", "(").Replace("&gt;", ")").Replace("+", " ").Replace(":", "-");
            //    //    //    //string FileName = temp + "." + tempItem.Extention;

            //    //    //    //downloadedFilePath = Path.Combine(targetPath, FileName);
            //    //    //    //initFileDownloader(tempItem.DownloadUrl, targetPath, FileName);
            //    //    //    //get the path like this
            //    //    //    //////////getTargetVideoFile(videoPath, destDirectory, slide.SlideIndex);
            //    //    //    //   System.Windows.Forms.MessageBox.Show(" After downloading : " + videoPath);
            //    //    //}
            //    //    //catch (Exception e)
            //    //    //{
            //    //    //    isLocalFile = false;
            //    //    //    //System.Windows.Forms.MessageBox.Show(e.Message);
            //    //    //}
            //    //    ////or use 
            //    //    //System.IO.File.Copy(shape.LinkFormat.SourceFullName, destDirectory); 
            //    //    //to export them
            //    //    // shape.OLEFormat.Object.

            //    //    //if (!isLocalFile)
            //    //    //{
            //    //    //    var destination = _tempFolderPath + "\\v" + slide.SlideIndex + "\\";
            //    //    //    if (!Directory.Exists(destination))
            //    //    //        Directory.CreateDirectory(destination);
            //    //    //    slide.PublishSlides(destination, true, true);
            //    //    //    var filePaths = Directory.GetFiles(destination);
            //    //    //    if (filePaths.Length != 0)
            //    //    //    {
            //    //    //        //for (var i = 0; i < filePaths.Length; i++)
            //    //    //        //    extractMediaFromSlide(filePaths[i], destDirectory, slide.SlideIndex);
            //    //    //    }
            //    //    //}
            //    //    // SaveSlideAsWmvFile(destination, objPres, destDirectory);
            //    //    //objPres.ExportAsFixedFormat("c:\\temp\\slide\\example.pdf", PpFixedFormatType.ppFixedFormatTypePDF,
            //    //    //PpFixedFormatIntent.ppFixedFormatIntentScreen);
            //    //    //pptPresentation.SaveAs("c:\\temp\\slide\\example.html", PpSaveAsFileType.ppSaveAsHTMLDual);
            //    //    //pptPresentation.SaveAs("c:\\temp\\slide\\exampledual.html", PpSaveAsFileType.ppSaveAsHTMLDual);
            //    //}
            //}
            //}
        }

        private static void ParseShapes(Slide slide)
        {
            foreach (Shape shape in slide.Shapes)
            {
                if (shape.Type != msoMedia) continue;
                try
                {
                    Log($"{shape.Type} - {shape.MediaType} - {shape.MediaFormat.IsEmbedded} - {shape.MediaFormat.IsLinked}");
                }
                catch
                {
                }
                if (ExistYouTubeVideo(shape))
                {
                    Log($"slide{slide.SlideNumber} contains youtube videos");
                    IEnumerable<VideoInfo> youtubeLinks = GetAllYoutubeLinks(slide);
                    var videoPath = DownloadVideoFromYouTube(youtubeLinks, slide.SlideNumber);
                    Log($"YouTube video path: {videoPath}");
                }
                if (ExistLinkedMedia(shape))
                {
                    Log($"slide{slide.SlideNumber} contains linked media");
                    CopyLinkedVideo(shape, slide.SlideNumber);
                }
            }
        }

        private static void ExtractEmbeddedVideo(Slide slide)
        {
            if (tempCopy == string.Empty)
                DoPresantationCopy(slide.Parent);
            using (ZipArchive archive = ZipFile.OpenRead(tempCopy))
            {
                //string xmlPath = "ppt/slides/_rels/slide2.xml.rels";
                string xmlPath = $"ppt/slides/_rels/slide{slide.SlideNumber}.xml.rels";
                ZipArchiveEntry zipSlideXml = archive.GetEntry(xmlPath);
                var slideXml = new XmlDocument();
                slideXml.Load(zipSlideXml.Open());
                foreach (XmlNode node in slideXml.DocumentElement)
                {
                    string type = node.Attributes["Type"].Value;
                    if (type.Contains("media"))
                    {
                        string target = node.Attributes["Target"].Value;
                        Log(target);
                        string nameMedia = Path.GetFileName(target);
                        string pathMedia = $"ppt/media/{nameMedia}";
                        Log(pathMedia);
                        ZipArchiveEntry zipMedia = archive.GetEntry(pathMedia);
                        try
                        {
                            string pathToUnzip = Path.Combine(videosFolder,
                                $"v{slide.SlideNumber}{Path.GetExtension(pathMedia)}");
                            zipMedia.ExtractToFile(pathToUnzip);
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                    }

                }
                //using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                //{
                //    writer.WriteLine("Information about this package.");
                //    writer.WriteLine("========================");
                //}
            }

        }

        private static void DoPresantationCopy(Presentation pres)
        {
            string sourcePath = pres.FullName;//Path + pres.Name;
            Log(sourcePath);
            tempCopy = $"{ezsFolder}\\{pres.Name}";
            bool needToCopy = true;
            int fileCount = 0;
            while (needToCopy)
                try
                {
                    File.Copy(sourcePath, tempCopy);
                    needToCopy = false;
                }
                catch (Exception)
                {
                    needToCopy = true;
                    fileCount++;
                    tempCopy =
                        $"{ezsFolder}\\{Path.GetFileNameWithoutExtension(pres.Name)}({fileCount}).{Path.GetExtension(pres.Name)}";
                    Log($"New destinition: {tempCopy}");
                }

        }

        private static void CopyLinkedVideo(Shape shape, int slideNumber)
        {
            var sourcePath = shape.LinkFormat.SourceFullName;
            var extension = Path.GetExtension(sourcePath);
            var newName = $"v{slideNumber}{extension}";
            var destPath = videosFolder + "\\" + newName;
            //File.Copy(sourcePath, destPath);
        }

        protected static bool HasLinkedVideo(Shape shape)
        {
            try
            {
                if (shape != null && shape.Type == msoMedia && shape.LinkFormat?.SourceFullName != null)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string RemoveIllegalPathCharacters(string path)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        // Shows all notes on logger
        // Not used
        private static void ShowAllNotes(Slide slide)
        {
            if (slide.HasNotesPage != MsoTriState.msoTrue) return;
            var notesPages = slide.NotesPage;
            foreach (var shape in from Shape shape in notesPages.Shapes where shape.Type == msoPlaceholder where shape.PlaceholderFormat.Type == PpPlaceholderType.ppPlaceholderBody select shape)
            {
                Log($"Slide[{slide.SlideIndex}] Notes: [{shape.TextFrame.TextRange.Text}]");
            }
        }

        // Returns list of all notes and comments on slide
        private static List<string> GetAllCommentsAndNodes(_Slide slide)
        {
            var result = new List<string>(); try
            {
                result.AddRange(from Comment comment in slide.Comments select comment.Text);
                if (slide.HasNotesPage == MsoTriState.msoTrue)
                {
                    result.AddRange((from Shape shape in slide.NotesPage.Shapes where shape.Type == msoPlaceholder where shape.PlaceholderFormat.Type == PpPlaceholderType.ppPlaceholderBody select shape).Select(shape => shape.TextFrame.TextRange.Text));
                }
            }
            catch (Exception exception)
            {
                Log(exception.Message);
                return null;
            }
            return result;
        }

        // Shows all hyperlinks on slide
        // Not used
        private static void ShowAllHyperlinks(_Slide slide)
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

        // Create mp3 video using metod SaveAs. 
        // Return path to video file
        private static string SaveAsMp4(_Presentation pres)
        {
            try
            {
                SetNullTransactDurations(pres);
                var strPath = @"D:\"; //Path.GetTempPath();
                Log("Temp path is " + strPath);
                //Environment.GetFolderPath("Temp");
                var nFile = Path.GetFileNameWithoutExtension(pres.Name) + ".mp4";
                Log("Presentation name is " + nFile);
                string strVideoFile = Path.Combine(strPath, nFile);
                pres.SaveAs(strVideoFile, PpSaveAsFileType.ppSaveAsMP4, MsoTriState.msoTrue);
                while (_pw.ActivePresentation.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusInProgress ||
                       _pw.ActivePresentation.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusQueued)
                {
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(500);
                }
                return strVideoFile;
            }
            catch (Exception exception)
            {
                Log(exception.Message);
                return null;
            }

        }

        // Set TansactDuration = 0 for all slides in presentation
        private static void SetNullTransactDurations(_Presentation pres)
        {
            var slidesCount = pres.Slides.Count;
            var slideIndex = new int[slidesCount];
            for (var i = 0; i < slidesCount; i++) slideIndex[i] = i + 1;
            var objSldRng = pres.Slides.Range(slideIndex);
            var objSst = objSldRng.SlideShowTransition;
            objSst.AdvanceOnTime = MsoTriState.msoTrue;
            objSst.AdvanceTime = 0;
        }

        private static void Log(string s) => _logger.AppendLog(DateTime.Now.ToString("hh:mm:ss") + ": " + s);
    }
}
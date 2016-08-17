using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using WeCastConvertor.Utils;
using YoutubeExtractor;
using static Microsoft.Office.Core.MsoShapeType;
using Shape = Microsoft.Office.Interop.PowerPoint.Shape;

namespace WeCastConvertor.Converter
{
    internal class Converter
    {
        private static readonly ILogger Logger = new DebugLogger();
        private static readonly Application Pw = new Application();
        private static readonly EventLogger El = new EventLogger(Logger, Pw);

        //Path to windows TEMP dirrectory
        private static readonly string TempFolderPath = Environment.GetEnvironmentVariable("TEMP");
        //private readonly LinkedList<int> Durations = new LinkedList<int>();
        private string _animFolder;
        private string _audioFolder;
        private string _slideFolder;
        private VideoCutter _cutter;
        private string _ezsContent;

        //Temp EZS dirrectories with content
        private string _ezsTemp;

        private MsoTriState _showPp = MsoTriState.msoTrue;
        private string _tempVideo;
        private string _videosFolder;
        private InfoWriter _writer;

        public Converter(string pathToPresentation)
        {
            PathToPresentation = pathToPresentation;
            var presName = Path.GetFileName(pathToPresentation);
            CreateDirrectories(presName);
        }

        public string PathToPresentation { get; }
        private string TempCopy { get; set; }

        public string Convert()
        {
            El.AttachEvents();
            var pres = Pw.Presentations.Open(PathToPresentation, MsoTriState.msoFalse, MsoTriState.msoFalse, _showPp);
            ParseSlides(pres);
            CreateVideo(pres);
            GetDurations(pres);
            El.DetachEvents();
            _writer.Save();
            CleanTempFiles();
            var result = CreateEzs(pres);
            pres.Close();
            //Pw.Quit();
            return result;
        }

        private string CreateEzs(Presentation pres)
        {
            var startPath = _ezsContent;
            var path = _ezsTemp; //Path.GetFullPath(pres.Path);
            var name = Path.GetFileNameWithoutExtension(pres.Name);
            var zipPath = path + $"\\{name}.ezs";
            var tryCount = 0;
            var needToSave = true;
            while (needToSave && tryCount < 10)
                try
                {
                    ZipFile.CreateFromDirectory(startPath, zipPath);
                    needToSave = false;
                }
                catch (IOException)
                {
                    tryCount++;
                    zipPath = path + $"{name} ({tryCount}).ezs";
                }
            return zipPath;
        }

        private void GetDurations(_Presentation pres)
        {
            foreach (Slide slide in pres.Slides)
            {
                var animId = 0;
                if (slide != null && slide.SlideShowTransition.EntryEffect != PpEntryEffect.ppEffectNone)
                {
                    animId++;
                    var numberOfFrames = (int) (30*slide.SlideShowTransition.Duration);
                    SaveAnimation(slide.SlideNumber, animId, numberOfFrames, true, true);
                }
                var isFirstAnimation = true;
                //Slide before animation
                //Durations.AddLast(1);
                foreach (Effect eff in slide.TimeLine.MainSequence)
                {
                    animId++;
                    //Slide animation duration
                    var numberOfFrames = (int)(30 * eff.Timing.Duration);
                    if (isFirstAnimation)
                    {
                        SaveAnimation(slide.SlideNumber, animId, numberOfFrames, true, true);
                        isFirstAnimation = false;
                    }
                    else
                    {
                        SaveAnimation(slide.SlideNumber, animId, numberOfFrames, false, false);
                    }
                }
                //If no animations on slide
                if (isFirstAnimation)
                {
                    SavePicture(slide);
                    //Durations.AddLast(1);
                }
            }
            //End of last slide black window
            Log(_cutter.CheckSum().ToString());
            _cutter.CloseVideo();
            //Log($"Total : {Durations.Sum()}");
        }

        private void SavePicture(_Slide slide)
        {
            _cutter.SkipFrames(2);
            //string pathToPicture = $"slides/slide{slide.SlideNumber}.jpg";
            //_writer.AddSlidePicture(slide.SlideNumber, pathToPicture);
        }

        private void SaveAnimation(int slideNumber, int animId, int count, bool hasFirstFrame, bool hasLastFrame)
        {
            string pathToVideo = $"animations/slide{slideNumber}_animation{animId}.mp4";
            string pathToPicture = $"animations/slide{slideNumber}_animation{animId}.jpg";
            _writer.AddAnimation(slideNumber, animId, pathToVideo, pathToPicture);
            string videoName = $"{_animFolder}\\slide{slideNumber}_animation{animId}.mp4";
            string pictureName = $"{_animFolder}\\slide{slideNumber}_animation{animId}.jpg";
            _cutter.SaveAnimation(count, slideNumber, animId, videoName, pictureName, hasFirstFrame, hasLastFrame);
        }

        private void CleanTempFiles()
        {
            try
            {
                if (File.Exists(TempCopy))
                    File.Delete(TempCopy);
                //if (File.Exists(_tempVideo))
                //    File.Delete(_tempVideo);
                _writer.Save();
            }
            catch (Exception)
            {
            }
        }

        private string CreateVideo(Presentation pres)
        {
            //The name of the video file to create.
            var fileName = _tempVideo; //$"{_ezsContent}\\_tempVideo.mp4";
            //Indicates whether to use timings and narrations.
            const bool useTimingsAndNarrations = true;
            //The duration, in seconds, to view the slide.
            const int defaultSlideDuration = 0;
            //The resolution of the slide.
            const int vertResolution = 1080;
            //The number of frames per second.
            const int framesPerSecond = 30;
            //The level of quality of the slide.
            const int quality = 100;
            pres.CreateVideo(fileName, useTimingsAndNarrations, defaultSlideDuration, vertResolution, framesPerSecond,
                quality);
            while (pres.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusInProgress)
                //|| pres.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusDone)
            {
                System.Windows.Forms.Application.DoEvents();
                Log(pres.CreateVideoStatus.ToString());
                Thread.Sleep(2000);
            }
            _cutter.OpenVideo();
            return fileName;
        }

        private void ParseSlides(Presentation pres)
        {
            foreach (Slide slide in pres.Slides)
            {
                var outputFile = _slideFolder + "\\" + slide.SlideNumber + ".jpg";
                _writer.AddSlide(slide.SlideNumber);
                _writer.AddAttribute(slide.SlideNumber, "picture", new StringBuilder($"slides/{slide.SlideNumber}.jpg"));
                slide.Export(outputFile, "jpg", 1440, 1080);
                ExtractAndSaveComments(slide);
                ParseShapes(slide);
                if (ExistEmbeddedMedia(slide))
                {
                    Log($"slide{slide.SlideNumber} contains embedded media");
                    ExtractEmbeddedVideo(slide);
                }
                ChangeMediaShapes(slide);
            }
        }

        private void ChangeMediaShapes(Slide slide)
        {
            foreach (Shape shape in slide.Shapes)
            {
                if (shape.Type != msoMedia) continue;
                if (shape.MediaType == PpMediaType.ppMediaTypeMovie)
                    ChangeShape(shape);
            }
            while (HasMedia(slide))
            {
                foreach (var shape in slide.Shapes.Cast<Shape>().Where(shape => shape.Type == msoMedia))
                {
                    shape.Delete();
                }
            }
        }

        private static bool HasMedia(Slide slide)
        {
            return slide.Shapes.Cast<Shape>().Any(shape => shape.Type == msoMedia);
        }

        private void ChangeShape(Shape shape)
        {
            //tempPictures.AddLast($"{_ezsContent}\\tempPicture{tempPictures.Count}.jpeg"); 
            string tempPicture = $"{_ezsContent}\\tempPicture.jpeg";
            shape.Export(tempPicture, PpShapeFormat.ppShapeFormatJPG);
            Slide slide = shape.Parent;
            var newPicture = slide.Shapes.AddPicture(tempPicture,
                MsoTriState.msoTriStateMixed,
                MsoTriState.msoFalse,
                shape.Left,
                shape.Top,
                shape.Width,
                shape.Height);
            File.Delete(tempPicture);
        }

        private void CreateDirrectories(string presName)
        {
            _ezsContent = GetRandomEzsFolder();
            //_commentsFolder = _ezsContent + @"\comments";
            _videosFolder = _ezsContent + @"\video";
            _audioFolder = _ezsContent + @"\audio";
            _animFolder = _ezsContent + @"\animations";
            _slideFolder = _ezsContent + @"\slides";
            _tempVideo = _ezsTemp + @"\tempVideo.mp4";
            TempCopy = _ezsTemp + @"\" + presName;
            ;
            if (!Directory.Exists(_ezsContent))
                Directory.CreateDirectory(_ezsContent);
            else
            {
                Directory.Delete(_ezsContent, true);
                Directory.CreateDirectory(_ezsContent);
            }
            if (!Directory.Exists(_animFolder))
                Directory.CreateDirectory(_animFolder);
            //if (!Directory.Exists(_commentsFolder))
            //    Directory.CreateDirectory(_commentsFolder);
            if (!Directory.Exists(_videosFolder))
                Directory.CreateDirectory(_videosFolder);
            if (!Directory.Exists(_audioFolder))
                Directory.CreateDirectory(_audioFolder);
            if (!Directory.Exists(_slideFolder))
                Directory.CreateDirectory(_slideFolder);
            _writer = new InfoWriter(Path.Combine(_ezsContent, "info.xml"));
            _cutter = new VideoCutter(_tempVideo);
        }

        private string GetRandomEzsFolder()
        {
            const string rc = "qwertyuiopasdfghjklzxcvbnm0123456789";
            var letters = rc.ToCharArray();
            var s = new StringBuilder();
            var rnd = new Random();
            for (var i = 0; i < 8; i++)
            {
                s.Append(letters[rnd.Next(letters.Length)].ToString());
            }
            _ezsTemp = TempFolderPath + @"\EZS_" + s;
            return TempFolderPath + @"\EZS_" + s + @"\content";
        }

        private void ExtractAndSaveComments(Slide slide)
        {
            var text = new StringBuilder();
            foreach (var comment in GetAllCommentsAndNodes(slide))
            {
                text.Append(comment);
            }
            _writer.AddAttribute(slide.SlideNumber, "comment", text);
        }

        private static IEnumerable<VideoInfo> GetAllYoutubeLinks(_Slide slide)
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

        private string DownloadVideoFromYouTube(IEnumerable<VideoInfo> videoInfos, int slideNumber)
        {
            if (videoInfos == null)
                return null;
            /*
             * Select the first .mp4 video with 720p resolution
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
            var savePath = Path.Combine(_videosFolder,
                //RemoveIllegalPathCharacters(video.Title)
                $"v{slideNumber}" + video.VideoExtension);
            var videoDownloader = new VideoDownloader(video, savePath);

            // Register the ProgressChanged event and print the current progress
            videoDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);

            /*
             * Execute the video downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            videoDownloader.Execute();
            _writer.AddSlideMedia(slideNumber, $"video/v{slideNumber}{video.VideoExtension}", "video");
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
                }
            }
            return false;
        }


        // Returns true if slide consist youtube videos
        private static bool ExistYouTubeVideo(Shape shape)
        {
            try
            {
                return shape.LinkFormat.SourceFullName != null &&
                       shape.LinkFormat.SourceFullName.Contains("youtube.com");
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ParseShapes(Slide slide)
        {
            foreach (Shape shape in slide.Shapes)
            {
                if (shape.Type != msoMedia) continue;
                try
                {
                    Log(
                        $"{shape.Type} - {shape.MediaType} - {shape.MediaFormat.IsEmbedded} - {shape.MediaFormat.IsLinked}");
                }
                catch(Exception e)
                {
                    Log(e.Message);
                }
                if (ExistYouTubeVideo(shape))
                {
                    Log($"slide{slide.SlideNumber} contains youtube videos");
                    var youtubeLinks = GetAllYoutubeLinks(slide);
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

        private void ExtractEmbeddedVideo(Slide slide)
        {
            if (!File.Exists(TempCopy))
                DoPresantationCopy(slide.Parent);
            using (var archive = ZipFile.OpenRead(TempCopy))
            {
                string xmlPath = $"ppt/slides/_rels/slide{slide.SlideNumber}.xml.rels";
                var zipSlideXml = archive.GetEntry(xmlPath);
                var slideXml = new XmlDocument();
                slideXml.Load(zipSlideXml.Open());
                foreach (XmlNode node in slideXml.DocumentElement)
                {
                    if (node.Attributes == null) continue;
                    var type = node.Attributes["Type"].Value;
                    if (!(type.Contains("audio") || type.Contains("video"))) continue;
                    var target = node.Attributes["Target"].Value;
                    Log(target);
                    var nameMedia = Path.GetFileName(target);
                    string pathMedia = $"ppt/media/{nameMedia}";
                    Log(pathMedia);
                    var zipMedia = archive.GetEntry(pathMedia);
                    var pathToUnzip = type.Contains("audio")
                        ? $"{_ezsContent}\\audio\\a{slide.SlideNumber}{Path.GetExtension(pathMedia)}"
                        : $"{_ezsContent}\\video\\v{slide.SlideNumber}{Path.GetExtension(pathMedia)}";
                    var needToExtract = true;
                    var tryCount = 0;
                    while (needToExtract)
                        try
                        {
                            tryCount++;
                            zipMedia.ExtractToFile(pathToUnzip);
                            needToExtract = false;
                        }
                        catch (Exception)
                        {
                            pathToUnzip = type.Contains("audio")
                                ? $"{_ezsContent}\\audio\\a{slide.SlideNumber}_{tryCount}{Path.GetExtension(pathMedia)}"
                                : $"{_ezsContent}\\video\\v{slide.SlideNumber}_{tryCount}{Path.GetExtension(pathMedia)}";
                        }
                    var typeForInfoWriter = type.Contains("audio") ? "audio" : "video";
                    var pathForInfoWriter = $"{typeForInfoWriter}/{Path.GetFileName(pathToUnzip)}";
                    _writer.AddSlideMedia(slide.SlideNumber, pathForInfoWriter, typeForInfoWriter);
                }
                //slideXml.
                //archive.
            }
        }

        private void DoPresantationCopy(Presentation pres)
        {
            var sourcePath = pres.FullName; //Path + pres.Name;
            Log(sourcePath);
            TempCopy = $"{_ezsContent}\\{pres.Name}";
            var needToCopy = true;
            var fileCount = 0;
            while (needToCopy)
                try
                {
                    File.Copy(sourcePath, TempCopy);
                    needToCopy = false;
                }
                catch (Exception)
                {
                    needToCopy = true;
                    fileCount++;
                    TempCopy =
                        $"{_ezsContent}\\{Path.GetFileNameWithoutExtension(pres.Name)}({fileCount}).{Path.GetExtension(pres.Name)}";
                    Log($"New destinition: {TempCopy}");
                }
        }

        private void CopyLinkedVideo(Shape shape, int slideNumber)
        {
            var sourcePath = shape.LinkFormat.SourceFullName;
            var extension = Path.GetExtension(sourcePath);
            string newName;
            string type = null;
            string internalPath = null;
            var destPath = string.Empty;
            if (shape.MediaType == PpMediaType.ppMediaTypeMovie)
            {
                newName = $"v{slideNumber}{extension}";
                destPath = _videosFolder + "\\" + newName;
                type = "video";
                internalPath = $"video/{newName}";
            }
            if (shape.MediaType == PpMediaType.ppMediaTypeSound)
            {
                newName = $"audio{slideNumber}{extension}";
                destPath = _audioFolder + "\\" + newName;
                type = "audio";
                internalPath = $"audio/{newName}";
            }
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException($"File {sourcePath} not found");
            SaveFile(sourcePath, destPath);
            _writer.AddSlideMedia(slideNumber, internalPath, type);
        }

        private static string SaveFile(string sourcePath, string destPath)
        {
            var tryCount = 0;
            var needToSave = true;

            while (needToSave && tryCount < 10)
                try
                {
                    tryCount++;
                    File.Copy(sourcePath, destPath);
                    needToSave = false;
                }
                catch (IOException)
                {
                    destPath = $"{Path.GetFileNameWithoutExtension(destPath)}_{tryCount}{Path.GetExtension(destPath)}";
                }
            return destPath;
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
        private void ShowAllNotes(Slide slide)
        {
            if (slide.HasNotesPage != MsoTriState.msoTrue) return;
            var notesPages = slide.NotesPage;
            foreach (var shape in from Shape shape in notesPages.Shapes
                where shape.Type == msoPlaceholder
                where shape.PlaceholderFormat.Type == PpPlaceholderType.ppPlaceholderBody
                select shape)
            {
                Log($"Slide[{slide.SlideIndex}] Notes: [{shape.TextFrame.TextRange.Text}]");
            }
        }

        // Returns list of all notes and comments on slide
        private List<string> GetAllCommentsAndNodes(_Slide slide)
        {
            var result = new List<string>();
            try
            {
                result.AddRange(from Comment comment in slide.Comments select comment.Text);
                if (slide.HasNotesPage == MsoTriState.msoTrue)
                {
                    result.AddRange((from Shape shape in slide.NotesPage.Shapes
                        where shape.Type == msoPlaceholder
                        where shape.PlaceholderFormat.Type == PpPlaceholderType.ppPlaceholderBody
                        select shape).Select(shape => shape.TextFrame.TextRange.Text));
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
        private void ShowAllHyperlinks(_Slide slide)
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
        private string SaveAsMp4(_Presentation pres)
        {
            try
            {
                SetNullTransactDurations(pres);
                var strPath = @"D:\"; //Path.GetTempPath();
                Log("Temp path is " + strPath);
                //Environment.GetFolderPath("Temp");
                var nFile = Path.GetFileNameWithoutExtension(pres.Name) + ".mp4";
                Log("Presentation name is " + nFile);
                var strVideoFile = Path.Combine(strPath, nFile);
                pres.SaveAs(strVideoFile, PpSaveAsFileType.ppSaveAsMP4, MsoTriState.msoTrue);
                while (Pw.ActivePresentation.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusInProgress ||
                       Pw.ActivePresentation.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusQueued)
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

        private static void Log(string s) => Logger.AppendLog(DateTime.Now.ToString("hh:mm:ss") + ": " + s);

        public void SetShow(bool show)
        {
            _showPp = show ? MsoTriState.msoTrue : MsoTriState.msoFalse;
        }

        public void Clear()
        {
            if (!Directory.Exists(_ezsContent)) return;
            try
            {
                Directory.Delete(_ezsTemp, true);
            }
            catch (IOException e)
            {
                Log(e.Message);
            }
        }
    }
}
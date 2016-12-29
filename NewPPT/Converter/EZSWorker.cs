using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms.VisualStyles;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Converter
{
    internal class EzsWorker
    {
        #region Pathes

        //Temp directory
        private static readonly string TempFolderPath = Environment.GetEnvironmentVariable("TEMP");

        //Main directory of ezs in temp folder
        //Name of directory generates in function GetRandomEzsFolder
        private string _ezsTemp;

        //Content folder in directory {_ezsTemp}
        private string _ezsContent;


        //Content folders
        private string _animFolder;
        private string _audioFolder;
        private string _slideFolder;
        private string _videosFolder;
        #endregion

        public string PathToPresentation { get; }
        private string TempCopy { get; set; }

        public string EzsTemp
        {
            get { return _ezsTemp; }
        }

        private string _tempVideo;
        private InfoWriter _writer;

        LinkedList<Image> slides = new LinkedList<Image>();

        private readonly ILogger Logger = new DebugLogger();

        public EzsWorker(string pathToPresentation)
        {
            PathToPresentation = pathToPresentation;
            var presName = Path.GetFileName(pathToPresentation);
            CreateDirrectories(); //presName);
        }

        public void AddSlide(Bitmap image)
        {
            slides.AddLast(image);
            image.Save(_slideFolder + "\\" + slides.Count + ".jpg", ImageFormat.Jpeg);
            _writer.AddSlide(slides.Count);
            _writer.AddAttribute(slides.Count, "picture", new StringBuilder($"slides/{slides.Count}.jpg"));
        }

        public void AddPrewew(Bitmap image)
        {
            var preview = new Bitmap(image, new Size(256,192));
            string pathToPreview = $"{_ezsContent}\\preview.jpeg";
            preview.Save(pathToPreview, ImageFormat.Jpeg);
            _writer.AddPresanpationAtribute("preview", new StringBuilder("preview.jpeg"));
        }

        //Function converts content folder to {name}.ezs in exsTemp folder
        public string Save()
        {
            SaveOrder(slides.Count);
            _writer.Save();
            string name = System.IO.Path.GetFileNameWithoutExtension(PathToPresentation);
            var startPath = _ezsContent;
            var path = _ezsTemp;
            //var name = Path.GetFileNameWithoutExtension(pres.Name);
            var zipPath = path + $"\\{name}.ezs";
            var tryCount = 0;
            var needToSave = true;
            while (needToSave && tryCount < 10)
                try
                {
                    ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Fastest, false, new Encoder());
                    needToSave = false;
                }
                catch (IOException)
                {
                    tryCount++;
                    zipPath = path + $"{name} ({tryCount}).ezs";
                }

            return zipPath;
        }



        private void SaveAnimation(int slideNumber, int animId, int fromFrame, int frameCount)
        {
            string pathToVideo = $"animations/slide{slideNumber}_animation{animId}.mp4";
            string pathToPicture = $"animations/slide{slideNumber}_animation{animId}.jpg";
            _writer.AddAnimation(slideNumber, animId, pathToVideo, pathToPicture);
            string videoName = $"{_animFolder}\\slide{slideNumber}_animation{animId}.mp4";
            string pictureName = $"{_animFolder}\\slide{slideNumber}_animation{animId}.jpg";
            //_cutter.SaveAnimation(fromFrame, frameCount, slideNumber, animId, videoName);
        }


        private void SaveOrder(int slidesCount)
        {
            if (slidesCount < 1)
                throw new Exception("Order exception: wrong slides count");
            var builder = new StringBuilder("1");
            for (var i = 2; i <= slidesCount; i++)
                builder.Append(";" + i);
            _writer.SaveOrder(builder.ToString());
        }

        private void SavePreview()
        {
            string pathToPreview = $"{_ezsContent}\\preview.jpeg";
            _writer.AddPresanpationAtribute("preview", new StringBuilder("preview.jpeg"));
        }

        private void CreateDirrectories() //string presName)
        {
            _ezsContent = GetRandomEzsFolder();

            _videosFolder = _ezsContent + @"\video";
            _audioFolder = _ezsContent + @"\audio";
            _animFolder = _ezsContent + @"\animations";
            _slideFolder = _ezsContent + @"\slides";
            _tempVideo = _ezsTemp + @"\tempVideo.mp4";
            if (!Directory.Exists(_ezsContent))
                Directory.CreateDirectory(_ezsContent);
            else
            {
                Directory.Delete(_ezsContent, true);
                Directory.CreateDirectory(_ezsContent);
            }
            CreateDirectory(_animFolder);
            CreateDirectory(_videosFolder);
            CreateDirectory(_audioFolder);
            CreateDirectory(_slideFolder);
            _writer = new InfoWriter(Path.Combine(_ezsContent, "info.xml"));
            var extension = Path.GetExtension(PathToPresentation);
            if (extension == null) return;
            var type = extension.Remove(0, 1);
            _writer.AddPresanpationAtribute("type", new StringBuilder(type));
            //_cutter = new VideoCutter(_tempVideo);
        }

        private void CreateDirectory(string directoryName)
        {
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
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


        //private static string SaveFile(string sourcePath, string destPath)
        //{
        //    var tryCount = 0;
        //    var needToSave = true;

        //    while (needToSave && tryCount < 10)
        //        try
        //        {
        //            tryCount++;
        //            File.Copy(sourcePath, destPath);
        //            needToSave = false;
        //        }
        //        catch (IOException)
        //        {
        //            destPath = $"{Path.GetFileNameWithoutExtension(destPath)}_{tryCount}{Path.GetExtension(destPath)}";
        //        }
        //    return destPath;
        //}


        private void Log(string s) => Logger.AppendLog(DateTime.Now.ToString("hh:mm:ss") + ": " + s);

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
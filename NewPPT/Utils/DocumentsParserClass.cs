using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Application = Microsoft.Office.Interop.PowerPoint.Application;
//using D = DocumentFormat.OpenXml.Drawing;
using Encoder = System.Drawing.Imaging.Encoder;
using Hyperlink = Microsoft.Office.Interop.PowerPoint.Hyperlink;
using Path = System.IO.Path;
using Presentation = Microsoft.Office.Interop.PowerPoint.Presentation;
//using Shape = DocumentFormat.OpenXml.Presentation.Shape;
using Slide = Microsoft.Office.Interop.PowerPoint.Slide;


namespace WeCastConvertor.Utils
{
    internal class DocumentsParserClass
    {
        //private static void downloadAndSaveMedia(string address, string tempPath, string outputPath, Slide slide)
        //{
        //    try
        //    {
        //        string filePath = MediaDownloaderClass.downloadMediaFile(address, tempPath);
        //        if (File.Exists(filePath))
        //        {
        //            getTargetVideoFile(filePath, outputPath, slide.SlideIndex);
        //            File.Delete(filePath);

        //        }
        //        //   System.Windows.Forms.MessageBox.Show(filePath);
        //    }
        //    catch (Exception e)
        //    {
        //        System.Windows.Forms.MessageBox.Show(e.Message);
        //    }

        //}
        public delegate void MediaHandler(string mediaPath);

        private static string videosDir = string.Empty;

        private static readonly string[] mediaExtensions =
        {
            ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", ".asf",
            ".AVI", ".MP4", ".DIVX", ".WMV", ".m4v", ".mov", ".mpg", ".mpeg",
            ".swf", ".aiff", ".au", ".mid", ".midi", ".m4a", ".wma" // audio
        };

        private static readonly string embeddingPartString = "/ppt/media/";

        private static bool IsMediaFile(string extension)
        {
            return -1 != Array.IndexOf(mediaExtensions, extension.ToUpperInvariant());
        }

        public static void openPPTXDocument(string fullPath, string outputPath)
        {
            //outputPath = @"C:\Users\hp\Desktop\OutputFiles";
            var ppApp = new Application();
            // ppApp.Visible = MsoTriState.msoTrue;
            var ppPresens = ppApp.Presentations;
            var objPres = ppPresens.Open(fullPath, MsoTriState.msoFalse, MsoTriState.msoTrue);

            var objSlides = objPres.Slides;
            // int count = 0;
            var commentDir = outputPath + "\\comments";
            var outputFile = "";
            if (!Directory.Exists(commentDir))
                Directory.CreateDirectory(commentDir);
            foreach (Slide slide in objSlides)
            {
                CreatePptNotesFile(commentDir, slide, slide.SlideNumber);
                //   count++;
                outputFile = outputPath + "\\" + slide.SlideNumber + ".jpg";
                slide.Export(outputFile, "jpg", 1920, 1080);
                if (slide.SlideNumber == 1)
                {
                    var bmp = CreateThumbnail(outputFile, 256, 192);
                    CreateJpegFromBitmap(bmp, outputPath);
                }
            }
            objPres.Close();
            ppApp.Quit();
        }

        public static void parsePPTXDocument(Presentation objPres, string fullPath, string outputPath)
        {
            var objSlides = objPres.Slides;
            // int count = 0;
            var commentDir = outputPath + "\\comments";
            var outputFile = "";
            if (!Directory.Exists(commentDir))
                Directory.CreateDirectory(commentDir);

            //begin the code repare bug in Android client
            if (videosDir.Equals(string.Empty))
                videosDir = outputPath + "\\videos";
            if (!Directory.Exists(videosDir))
                Directory.CreateDirectory(videosDir);
            //end the code repare bug in Android client

            foreach (Slide slide in objSlides)
            {
                CreatePptNotesFile(commentDir, slide, slide.SlideNumber);
                //   count++;
                outputFile = outputPath + "\\" + slide.SlideNumber + ".jpg";
                slide.Export(outputFile, "jpg", 1920, 1080);
                if (slide.SlideNumber == 1)
                {
                    var bmp = CreateThumbnail(outputFile, 256, 192);
                    CreateJpegFromBitmap(bmp, outputPath);
                }
                //downloadFromUrl("");
                writeSlideTitleToFilePPT(outputPath, slide);
                extractVideoFile(outputPath, slide, objPres);
            }
            prepareAnimationSlides(outputPath, objPres, fullPath);
            //writeSlideTitleToFile(outputPath,fullPath);
        }

        private static void prepareAnimationSlides(string outputPath, Presentation pptPresentation, string presFileName)
        {
            var dir = SplitAnimation.splitAnimation(presFileName, pptPresentation);
            var folderName = outputPath + "\\" + Path.GetFileName(dir);

            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            var subdirectoryEntries = Directory.GetDirectories(dir);
            foreach (var subdirectory in subdirectoryEntries)
            {
                if (Path.GetFileName(subdirectory).Equals("videos"))
                {
                    var fileEntries = Directory.GetFiles(subdirectory);
                    foreach (var fileName in fileEntries)
                    {
                        if (File.Exists(fileName))
                        {
                            if (videosDir.Equals(string.Empty))
                                videosDir = outputPath + "\\videos";
                            if (!Directory.Exists(videosDir))
                                Directory.CreateDirectory(videosDir);
                            var destFilePath = videosDir + "\\" + Path.GetFileName(fileName);
                            File.Move(fileName, destFilePath);
                        }
                    }
                }
                else
                {
                    //string[] slidesSubdirectories = Directory.GetDirectories(subdirectory);
                    // foreach (string slideSubdirectory in slidesSubdirectories)
                    // {
                    var destFolder = folderName + "\\" + Path.GetFileName(subdirectory);
                    Directory.CreateDirectory(destFolder);
                    var fileEntries = Directory.GetFiles(subdirectory);
                    foreach (var fileName in fileEntries)
                    {
                        if (Path.GetExtension(fileName).Equals(".jpg"))
                        {
                            var destFilePath = destFolder + "\\" + Path.GetFileName(fileName);
                            File.Move(fileName, destFilePath);
                        }
                        else
                        {
                            File.Delete(fileName);
                        }
                    }
                    Directory.Delete(subdirectory);

                    // }
                }
            }


            //// Thread.Sleep(5000);
            //EmptyFolder(new DirectoryInfo(dir));
        }

        private static void writeSlideTitleToFilePPT(string destDirectory, Slide slide)
        {
            var path = destDirectory + "\\titles.txt";
            StreamWriter sw = null;
            try
            {
                if (!File.Exists(path))
                {
                    var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    sw = new StreamWriter(fs);
                }
                else
                {
                    sw = File.AppendText(path);
                }

                var s = GetSlideTitlePpt(slide);
                sw.WriteLine(s);
                sw.Close();
            }
            catch (Exception)
            {
                sw.Close();
            }
        }

        private static void writeSlideTitleToFile(string destDirectory, string fullPath)
        {
            var path = destDirectory + "\\titles.txt";
            StreamWriter sw = null;
            try
            {
                if (!File.Exists(path))
                {
                    var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    sw = new StreamWriter(fs);
                }
                else
                {
                    sw = File.AppendText(path);
                }

                foreach (var s in GetSlideTitles(fullPath))
                    sw.WriteLine(s);
                sw.Close();
            }
            catch (Exception)
            {
                sw.Close();
            }
        }

        private static void extractVideoFile(string destDirectory, Slide slide, Presentation objPres)
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
                        getTargetVideoFile(videoPath, destDirectory, slide.SlideIndex);
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
                            for (var i = 0; i < filePaths.Length; i++)
                                extractMediaFromSlide(filePaths[i], destDirectory, slide.SlideIndex);
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

        private static string getTargetVideoFile(string videoPath, string destDirectory, int slideIndex)
        {
            var filename = Path.GetFileName(videoPath);
            var filePath = string.Empty;
            //System.Windows.Forms.MessageBox.Show(" Before downloading : " + videoPath);
            // MediaDownloaderClass.downloadMediaFile(videoPath, destination);
            if (videosDir.Equals(string.Empty))
                videosDir = destDirectory + "\\videos";
            var outputFile = string.Empty;
            if (!Directory.Exists(videosDir))
                Directory.CreateDirectory(videosDir);
            var extension = Path.GetExtension(filename);
            if (IsMediaFile(extension))
            {
                var newName = "v" + slideIndex + extension;
                filePath = videosDir + "\\" + newName;

                File.Copy(videoPath, filePath);
            }
            return filePath;
        }

        private static void extractDataFromURLByHyperlink(string outputPath, Slide slide)
        {
            var hyperLinks = slide.Hyperlinks;
            var filePath = string.Empty;
            var tempPath = Environment.GetEnvironmentVariable("TEMP");

            foreach (Hyperlink hl in hyperLinks)
            {
                downloadAndSaveMedia(hl.Address, tempPath, outputPath, slide);
                // System.Windows.Forms.MessageBox.Show("Slide " + slide.SlideIndex + " hyperlink : " + hl.Address);
                //try
                //{
                //    filePath = MediaDownloaderClass.downloadMediaFile(hl.Address, tempPath);
                //   if (File.Exists(filePath))
                //   {
                //       getTargetVideoFile(filePath, outputPath, slide.SlideIndex);
                //       File.Delete(filePath);

                //   }
                ////   System.Windows.Forms.MessageBox.Show(filePath);
                //}
                //catch (Exception e)
                //{
                //   // System.Windows.Forms.MessageBox.Show(e.Message);
                //}
            }
        }

        private static void downloadAndSaveMedia(string address, string tempPath, string outputPath, Slide slide)
        {
            try
            {
                // address = "http://www.youtube.com/watch?v=UZyTCQyzPm0";
                var filePath = MediaDownloaderClass.downloadMediaFile(address, tempPath, targetYoutubeVideo);
                //if (File.Exists(filePath))
                //{
                //    getTargetVideoFile(filePath, outputPath, slide.SlideIndex);
                //    File.Delete(filePath);

                //}
                //   System.Windows.Forms.MessageBox.Show(filePath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void targetYoutubeVideo(string filePath)
        {
            if (File.Exists(filePath))
            {
                //getTargetVideoFile(filePath, outputPath, slide.SlideIndex);
                File.Delete(filePath);
            }
        }

        private static string downloadFromUrl(string url)
        {
            url = "http://www.today.com/id/25802159/&GT1=43001#.U3_URvmSy8Q";
            var fileName = @"C:\AddInnToPPT\YoutubeVideo";
            var aClient = new WebClient();
            var collection = new WebHeaderCollection();
            collection[HttpRequestHeader.Range] = "bytes=0-0";
            aClient.Headers = collection;
            aClient.DownloadStringCompleted += DownloadStringCallback2;
            aClient.DownloadStringAsync(new Uri(url));


            //Create a stream for the file
            // Stream stream = null;

            ////This controls how many bytes to read at a time and send to the client
            //int bytesToRead = 10000;

            //// Buffer to read bytes in chunk size specified above
            //byte[] buffer = new Byte[bytesToRead];

            //// The number of bytes read
            //try
            //{
            //    //Create a WebRequest to get the file
            //   HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

            //    //Create a response for this request
            //    HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

            //    if (fileReq.ContentLength > 0)
            //        fileResp.ContentLength = fileReq.ContentLength;

            //    //Get the Stream returned from the response
            //    stream = fileResp.GetResponseStream();

            //    // prepare the response to the client. resp is the client Response
            //    var resp = HttpContext.Current.Response;

            //    //Indicate the type of data being sent
            //    resp.ContentType = "video/x-flv";//"application/octet-stream";

            //    //Name the file
            //    resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            //    resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());

            //    int length;
            //    do
            //    {
            //        // Verify that the client is connected.
            //        if (resp.IsClientConnected)
            //        {
            //            // Read data into the buffer.
            //            length = stream.Read(buffer, 0, bytesToRead);

            //            // and write it out to the response's output stream
            //            resp.OutputStream.Write(buffer, 0, length);

            //            // Flush the data
            //            resp.Flush();

            //            //Clear the buffer
            //            buffer = new Byte[bytesToRead];
            //        }
            //        else
            //        {
            //            // cancel the download if client has disconnected
            //            length = -1;
            //        }
            //    } while (length > 0); //Repeat until no data is read
            //}
            //finally
            //{
            //    if (stream != null)
            //    {
            //        //Close the input stream
            //        stream.Close();
            //    }
            //}
            return fileName;
        }

        private static void DownloadStringCallback2(object sender, DownloadStringCompletedEventArgs e)
        {
            var aClient = (WebClient) sender;
            var responseHeaders = aClient.ResponseHeaders;
            var TypeOfContent = responseHeaders["Content-Type"];
            MessageBox.Show(TypeOfContent);
            //throw new NotImplementedException();
        }

        private static string extractMediaFromSlide(string presName, string destDirectory, int slideNumber)
        {
            // Open the package and loop through parts 
            // Check if the part uri to find if it contains the selected items in checked list box
            var filePath = string.Empty;
            //var pkg = Package.Open(presName);
            var videosDir = destDirectory + "\\videos";
            if (!Directory.Exists(videosDir))
                Directory.CreateDirectory(videosDir);


            //foreach (var pkgPart in pkg.GetParts())
            //{
            //    if (pkgPart.Uri.ToString().Contains(embeddingPartString))
            //    {
            //        // Get the file name
            //        var fileName1 = pkgPart.Uri.ToString().Remove(0, embeddingPartString.Length);

            //        // Get the stream from the part
            //        var partStream = pkgPart.GetStream();
            //        var extension = Path.GetExtension(fileName1);
            //        if (!IsMediaFile(extension)) continue;
            //        var newName = "v" + slideNumber + extension;
            //        filePath = videosDir + "\\" + newName;

            //        // Write the steam to the file.
            //        var writeStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            //        ReadWriteStream(partStream, writeStream);

            //        // If the file is a structured storage file stored as a oleObjectXX.bin file
            //        // Use Ole10Native class to extract the contents inside it.
            //        if (fileName1.Contains("oleObject"))
            //        {
            //            // The Ole10Native class is defined in Ole10Native.cs file
            //            Ole10Native.ExtractFile(filePath, videosDir);
            //        }
            //        //    }
            //    }
            //}
            //pkg.Close();
            RemoveTempFilesAndFolders(presName);
            return filePath;
        }

        private static void RemoveTempFilesAndFolders(string filePath)
        {
            if (File.Exists(filePath))
            {
                var folderPath = Path.GetDirectoryName(filePath);
                File.Delete(filePath);
                if (folderPath != null) Directory.Delete(folderPath);
            }
        }

        private static void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            var Length = 256;
            var buffer = new byte[Length];
            var bytesRead = readStream.Read(buffer, 0, Length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }

        private static void SaveSlideAsWmvFile(string fileLocationDir, Presentation objPres, string destDirectory)
        {
            var dir = destDirectory + "\\videos";
            string outputFile;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var filePaths = Directory.GetFiles(fileLocationDir);
            if (filePaths.Length == 0)
                return;
            outputFile = dir + "\\" + new DirectoryInfo(fileLocationDir).Name + ".wmv";

            try
            {
                _Presentation objPresNew;
                var objApp = objPres.Application;
                objApp.Visible = MsoTriState.msoFalse;

                objPresNew = objApp.Presentations.Open(filePaths[0], MsoTriState.msoTrue, MsoTriState.msoTrue);
                objPresNew.SaveAs(outputFile, PpSaveAsFileType.ppSaveAsWMV);
                long len = 0;
                do
                {
                    Thread.Sleep(500);
                    try
                    {
                        var f = new FileInfo(outputFile);
                        len = f.Length;
                    }
                    catch
                    {
                    }
                } while (len == 0);
            }
            catch (Exception)
            {
                //System.Console.WriteLine("Error: " + e.Message);
            }
        }

        // Get a list of the titles of all the slides in the presentation.
        public static string GetSlideTitlePpt(Slide slide)
        {
            var title = string.Empty;
            if (slide.Shapes.HasTitle == MsoTriState.msoTrue)
                title = slide.Shapes.Title.TextFrame.TextRange.Text;

            return title;
        }

        // Get a list of the titles of all the slides in the presentation.
        public static IList<string> GetSlideTitles(string presentationFile)
        {
            //// Open the presentation as read-only.
            //using (var presentationDocument =
            //    PresentationDocument.Open(presentationFile, false))
            //{
            //    return GetSlideTitles(presentationDocument);
            //}
            return null;
        }

        //// Get a list of the titles of all the slides in the presentation.
        //public static IList<string> GetSlideTitles(PresentationDocument presentationDocument)
        //{
        //    if (presentationDocument == null)
        //    {
        //        throw new ArgumentNullException("presentationDocument");
        //    }

        //    // Get a PresentationPart object from the PresentationDocument object.
        //    var presentationPart = presentationDocument.PresentationPart;

        //    if (presentationPart != null &&
        //        presentationPart.Presentation != null)
        //    {
        //        // Get a Presentation object from the PresentationPart object.
        //        var presentation = presentationPart.Presentation;

        //        if (presentation.SlideIdList != null)
        //        {
        //            var titlesList = new List<string>();

        //            // Get the title of each slide in the slide order.
        //            foreach (var slideId in presentation.SlideIdList.Elements<SlideId>())
        //            {
        //                var slidePart = presentationPart.GetPartById(slideId.RelationshipId) as SlidePart;

        //                // Get the slide title.
        //                var title = GetSlideTitle(slidePart);

        //                // An empty title can also be added.
        //                titlesList.Add(title);
        //            }

        //            return titlesList;
        //        }
        //    }

        //    return null;
        //}

        //// Get the title string of the slide.
        //public static string GetSlideTitle(SlidePart slidePart)
        //{
        //    if (slidePart == null)
        //    {
        //        throw new ArgumentNullException("slidePart");
        //    }

        //    // Declare a paragraph separator.
        //    string paragraphSeparator = null;

        //    if (slidePart.Slide != null)
        //    {
        //        // Find all the title shapes.
        //        var shapes = from shape in slidePart.Slide.Descendants<Shape>()
        //            where IsTitleShape(shape)
        //            select shape;

        //        var paragraphText = new StringBuilder();

        //        foreach (var shape in shapes)
        //        {
        //            // Get the text in each paragraph in this shape.
        //            foreach (var paragraph in shape.TextBody.Descendants<D.Paragraph>())
        //            {
        //                // Add a line break.
        //                paragraphText.Append(paragraphSeparator);

        //                foreach (var text in paragraph.Descendants<D.Text>())
        //                {
        //                    paragraphText.Append(text.Text);
        //                }

        //                paragraphSeparator = "\n";
        //            }
        //        }

        //        return paragraphText.ToString();
        //    }

        //    return string.Empty;
        //}

        //// Determines whether the shape is a title shape.
        //private static bool IsTitleShape(Shape shape)
        //{
        //    var placeholderShape =
        //        shape.NonVisualShapeProperties.ApplicationNonVisualDrawingProperties.GetFirstChild<PlaceholderShape>();
        //    if (placeholderShape != null && placeholderShape.Type != null && placeholderShape.Type.HasValue)
        //    {
        //        switch ((PlaceholderValues) placeholderShape.Type)
        //        {
        //            // Any title shape.
        //            case PlaceholderValues.Title:

        //            // A centered title.
        //            case PlaceholderValues.CenteredTitle:
        //                return true;

        //            default:
        //                return false;
        //        }
        //    }
        //    return false;
        //}

        private static void CreatePptNotesFile(string destDirectory, Slide slide, int slideNum)
        {
            var path = destDirectory + "\\s" + slideNum + ".txt";
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (var sw = File.CreateText(path))
                {
                    sw.WriteLine(GetNotes(slide));
                }
            }
        }

        private static string GetNotes(Slide slide)
        {
            if (slide.HasNotesPage == MsoTriState.msoFalse)
                return string.Empty;

            var slideNodes = string.Empty;
            var notesPage = slide.NotesPage;
            var length = 0;
            foreach (Microsoft.Office.Interop.PowerPoint.Shape shape in notesPage.Shapes)
            {
                if (shape.Type == MsoShapeType.msoPlaceholder)
                {
                    var tf = shape.TextFrame;
                    try
                    {
                        //Some TextFrames do not have a range
                        var range = tf.TextRange;
                        if (range.Length > length)
                        {
                            //Some have a digit in the text, 
                            //so find the longest text item and return that
                            slideNodes = range.Text;
                            length = range.Length;
                        }
                        Marshal.ReleaseComObject(range);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                    finally
                    {
                        //Ensure clear up
                        Marshal.ReleaseComObject(tf);
                    }
                }
                Marshal.ReleaseComObject(shape);
            }
            return slideNodes;
        }

        public static Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
        {
            Bitmap bmpOut;
            try
            {
                var loBmp = new Bitmap(lcFilename);

                decimal lnRatio;
                int lnNewWidth;
                int lnNewHeight;

                //*** If the image is smaller than a thumbnail just return it
                if (loBmp.Width < lnWidth && loBmp.Height < lnHeight)
                    return loBmp;

                if (loBmp.Width > loBmp.Height)
                {
                    lnRatio = (decimal) lnWidth/loBmp.Width;
                    lnNewWidth = lnWidth;
                    var lnTemp = loBmp.Height*lnRatio;
                    lnNewHeight = (int) lnTemp;
                }
                else
                {
                    lnRatio = (decimal) lnHeight/loBmp.Height;
                    lnNewHeight = lnHeight;
                    var lnTemp = loBmp.Width*lnRatio;
                    lnNewWidth = (int) lnTemp;
                }
                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                var g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                g.DrawImage(loBmp, 0, 0, lnNewWidth, lnNewHeight);

                loBmp.Dispose();
            }
            catch
            {
                return null;
            }

            return bmpOut;
        }

        public static void CreateJpegFromBitmap(Bitmap myBitmap, string outputPath)
        {
            //Bitmap myBitmap;
            ImageCodecInfo myImageCodecInfo;
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Create a Bitmap object based on a BMP file.
            // myBitmap = new Bitmap("Shapes.bmp");

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            // Create an Encoder object based on the GUID 

            // for the Quality parameter category.
            myEncoder = Encoder.Quality;

            // Create an EncoderParameters object. 

            // An EncoderParameters object has an array of EncoderParameter 

            // objects. In this case, there is only one 

            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);


            // Save the bitmap as a JPEG file with quality level 75.
            myEncoderParameter = new EncoderParameter(myEncoder, 75L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            myBitmap.Save(outputPath + "\\preview.jpeg", myImageCodecInfo, myEncoderParameters);
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
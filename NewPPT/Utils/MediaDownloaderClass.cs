using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
//using WeCastConvertor.Utils.YouTubeDownloader;

namespace WeCastConvertor.Utils
{
    class MediaDownloaderClass
    {
        //private List<YouTubeVideoQuality> urls;
        private FileDownloader downloader;
        private string downloadedFilePath = String.Empty;
        private static DocumentsParserClass.MediaHandler handler;

        public void initFileDownloader(string Url,string folder ,string file)
        {
            downloader = new FileDownloader(Url, folder, file);
            //downloader.ProgressChanged += downloader_ProgressChanged;
            downloader.RunWorkerCompleted += downloader_RunWorkerCompleted;
            downloader.RunWorkerAsync();
        }
        void downloader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            handler(downloadedFilePath);
            //MessageBox.Show("Download completed !");
        }
        private void checkLinkAndDownload(string link, string targetPath)
        {
            try
            {
                //if (!Helper.isValidUrl(link) || !link.ToLower().Contains("www.youtube.com/watch?"))
                //{
                //    Console.WriteLine("You enter invalid YouTube URL, Please correct it.\r\n\nNote: URL should start with:\r\nhttp://www.youtube.com/watch?",
                //        "Invalid URL");
                //    return;
                //}
                //urls = YouTubeDownloader.YouTubeDownloader.GetYouTubeVideoUrls(link);

                //YouTubeVideoQuality[] arr = urls.ToArray();
                //YouTubeVideoQuality tempItem = arr[0];
                //string FileName = FormatTitle(tempItem.VideoTitle) +"." + tempItem.Extention;

                //downloadedFilePath = Path.Combine(targetPath, FileName);
                //initFileDownloader(tempItem.DownloadUrl, targetPath, FileName);
            }
            catch (Exception ex) { Console.WriteLine (ex.Message); }
        }

        public static string FormatTitle(string title)
        {
            return title.Replace(@"\", "").Replace("&#39;", "'").Replace("&quot;", "'").Replace("&lt;", "(").Replace("&gt;", ")").Replace("+", " ").Replace(":", "-");
        }
        public static string downloadMediaFile(string link,string targetPath,DocumentsParserClass.MediaHandler hndl)
        {
            // Our test youtube link
            //const string link = "http://www.youtube.com/watch?v=O3UBOOZw-FE";

            /*
             * Get the available video formats.
             * We'll work with them in the video and audio download examples.
             */
            handler = hndl;
            new MediaDownloaderClass().checkLinkAndDownload(link,targetPath);

            //DownloadAudio(videoInfos);
            // IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
            //return DownloadVideo(videoInfos, targetPath);
            return null;
        }
        //private static string DownloadAudio(IEnumerable<VideoInfo> videoInfos,string targetPath)
        //{
        //    /*
        //     * We want the first extractable video with the highest audio quality.
        //     */
        //    VideoInfo video = videoInfos
        //        .Where(info => info.CanExtractAudio)
        //        .OrderByDescending(info => info.AudioBitrate)
        //        .First();

        //    /*
        //     * Create the audio downloader.
        //     * The first argument is the video where the audio should be extracted from.
        //     * The second argument is the path to save the audio file.
        //     */
        //    string filePath = Path.Combine(targetPath, video.Title + video.AudioExtension);
        //    var audioDownloader = new AudioDownloader(video,filePath);
        //        //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), video.Title + video.AudioExtension));

        //    // Register the progress events. We treat the download progress as 85% of the progress
        //    // and the extraction progress only as 15% of the progress, because the download will
        //    // take much longer than the audio extraction.
        //    audioDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage * 0.85);
        //    audioDownloader.AudioExtractionProgressChanged += (sender, args) => Console.WriteLine(85 + args.ProgressPercentage * 0.15);

        //    /*
        //     * Execute the audio downloader.
        //     * For GUI applications note, that this method runs synchronously.
        //     */
        //    audioDownloader.Execute();
        //    return filePath;
        //}

        //private static string DownloadVideo(IEnumerable<VideoInfo> videoInfos , string targetPath)
        //{
        //    /*
        //     * Select the first .mp4 video with 360p resolution
        //     */
        //    VideoInfo video = videoInfos
        //        .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 240);

        //    /*
        //     * Create the video downloader.
        //     * The first argument is the video to download.
        //     * The second argument is the path to save the video file.
        //     */
        //    /*Environment.GetFolderPath(Environment.SpecialFolder.Desktop)*/
        //    string filePath = Path.Combine(targetPath, "temp" + video.VideoExtension);
        //    var videoDownloader = new VideoDownloader(video, filePath);

        //    // Register the ProgressChanged event and print the current progress
        //    videoDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);

        //    /*
        //     * Execute the video downloader.
        //     * For GUI applications note, that this method runs synchronously.
        //     */
        //    videoDownloader.Execute();
        //    return filePath;
        //}
    }
}

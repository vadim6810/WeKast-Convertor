using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;

namespace NewPPT.Utils.YouTubeDownloader
{
    /// <summary>
    ///     Contains information about the video url extension and dimension
    /// </summary>
    public class YouTubeVideoQuality
    {
        /// <summary>
        ///     Gets or Sets the file name
        /// </summary>
        public string VideoTitle { get; set; }

        /// <summary>
        ///     Gets or Sets the file extention
        /// </summary>
        public string Extention { get; set; }

        /// <summary>
        ///     Gets or Sets the file url
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        ///     Gets or Sets the youtube video url
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        ///     Gets or Sets the youtube video size
        /// </summary>
        public long VideoSize { get; set; }

        /// <summary>
        ///     Gets or Sets the youtube video dimension
        /// </summary>
        public Size Dimension { get; set; }

        /// <summary>
        ///     Gets the youtube video length
        /// </summary>
        public long Length { get; set; }

        public override string ToString()
        {
            var videoExtention = Extention;
            var videoDimension = FormatSize(Dimension);
            var videoSize = string.Format(new FileSizeFormatProvider(), "{0:fs}", VideoSize);

            return $"{videoExtention.ToUpper()} ({videoDimension}) - {videoSize}";
        }

        private string FormatSize(Size value)
        {
            var s = value.Height >= 720 ? " HD" : "";
            return value.Width + " x " + value.Height + s;
        }

        public void SetQuality(string extention, Size dimension)
        {
            Extention = extention;
            Dimension = dimension;
        }

        public void SetSize(long size)
        {
            VideoSize = size;
        }
    }

    /// <summary>
    ///     Use this class to get youtube video urls
    /// </summary>
    public class YouTubeDownloader
    {
        public static List<YouTubeVideoQuality> GetYouTubeVideoUrls(params string[] videoUrls)
        {
            var urls = new List<YouTubeVideoQuality>();
            foreach (var videoUrl in videoUrls)
            {
                var html = Helper.DownloadWebPage(videoUrl);
                var title = GetTitle(html);
                //  foreach (var videoLink in ExtractUrls(html))
                // {
                var list = ExtractUrls(html);
                var arr = list.ToArray();
                var videoLink = arr[0];

                var q = new YouTubeVideoQuality
                {
                    VideoUrl = videoUrl,
                    VideoTitle = title,
                    DownloadUrl = videoLink + "&title=" + title
                };
                if (!GetSize(q)) continue;
                q.Length =
                    long.Parse(
                        Regex.Match(html, "\"length_seconds\":(.+?),", RegexOptions.Singleline).Groups[1].ToString());
                IsWideScreen(html);
                //if (GetQuality(q, IsWide))
                q.SetQuality("mp4", new Size(640, 360));
                urls.Add(q);
                //}
            }
            return urls;
        }

        private static string GetTitle(string rssDoc)
        {
            var str14 = Helper.GetTxtBtwn(rssDoc, "'VIDEO_TITLE': '", "'", 0);
            if (str14 == "") str14 = Helper.GetTxtBtwn(rssDoc, "\"title\" content=\"", "\"", 0);
            if (str14 == "") str14 = Helper.GetTxtBtwn(rssDoc, "&title=", "&", 0);
            str14 =
                str14.Replace(@"\", "")
                    .Replace("'", "&#39;")
                    .Replace("\"", "&quot;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("+", " ");
            return str14;
        }

        private static List<string> ExtractUrls(string html)
        {
            var urls = new List<string>();
            var dataBlockStart = "\"url_encoded_fmt_stream_map\":\\s+\"(.+?)&"; // Marks start of Javascript Data Block

            html =
                Uri.UnescapeDataString(Regex.Match(html, dataBlockStart, RegexOptions.Singleline).Groups[1].ToString());

            var firstPatren = html.Substring(0, html.IndexOf('=') + 1);
            var matchs = Regex.Split(html, firstPatren);
            for (var i = 0; i < matchs.Length; i++)
                matchs[i] = firstPatren + matchs[i];
            foreach (var match in matchs)
            {
                // var match = matchs[0];
                if (!match.Contains("url=")) continue;

                var url = Helper.GetTxtBtwn(match, "url=", "\\u0026", 0);
                if (url == "") url = Helper.GetTxtBtwn(match, "url=", ",url", 0);
                if (url == "") url = Helper.GetTxtBtwn(match, "url=", "\",", 0);

                var sig = Helper.GetTxtBtwn(match, "sig=", "\\u0026", 0);
                if (sig == "") sig = Helper.GetTxtBtwn(match, "sig=", ",sig", 0);
                if (sig == "") sig = Helper.GetTxtBtwn(match, "sig=", "\",", 0);

                while (url.EndsWith(",") || url.EndsWith(".") || url.EndsWith("\""))
                    url = url.Remove(url.Length - 1, 1);

                while (sig.EndsWith(",") || sig.EndsWith(".") || sig.EndsWith("\""))
                    sig = sig.Remove(sig.Length - 1, 1);

                if (string.IsNullOrEmpty(url)) continue;
                if (!string.IsNullOrEmpty(sig))
                    url += "&signature=" + sig;
                urls.Add(url);
            }
            return urls;
        }

        public static bool GetQuality(YouTubeVideoQuality q, bool wide)
        {
            var itag =
                Regex.Match(q.DownloadUrl, @"itag=([1-9]?[0-9]?[0-9])", RegexOptions.Singleline).Groups[1].ToString();
            if (itag != "")
            {
                int iTagValue;
                if (int.TryParse(itag, out iTagValue) == false)
                    iTagValue = 0;

                switch (iTagValue)
                {
                    case 5:
                        q.SetQuality("flv", new Size(320, wide ? 180 : 240));
                        break;
                    case 6:
                        q.SetQuality("flv", new Size(480, wide ? 270 : 360));
                        break;
                    case 17:
                        q.SetQuality("3gp", new Size(176, wide ? 99 : 144));
                        break;
                    case 18:
                        q.SetQuality("mp4", new Size(640, wide ? 360 : 480));
                        break;
                    case 22:
                        q.SetQuality("mp4", new Size(1280, wide ? 720 : 960));
                        break;
                    case 34:
                        q.SetQuality("flv", new Size(640, wide ? 360 : 480));
                        break;
                    case 35:
                        q.SetQuality("flv", new Size(854, wide ? 480 : 640));
                        break;
                    case 36:
                        q.SetQuality("3gp", new Size(320, wide ? 180 : 240));
                        break;
                    case 37:
                        q.SetQuality("mp4", new Size(1920, wide ? 1080 : 1440));
                        break;
                    case 38:
                        q.SetQuality("mp4", new Size(2048, wide ? 1152 : 1536));
                        break;
                    case 43:
                        q.SetQuality("webm", new Size(640, wide ? 360 : 480));
                        break;
                    case 44:
                        q.SetQuality("webm", new Size(854, wide ? 480 : 640));
                        break;
                    case 45:
                        q.SetQuality("webm", new Size(1280, wide ? 720 : 960));
                        break;
                    case 46:
                        q.SetQuality("webm", new Size(1920, wide ? 1080 : 1440));
                        break;
                    case 82:
                        q.SetQuality("3D.mp4", new Size(480, wide ? 270 : 360));
                        break; // 3D
                    case 83:
                        q.SetQuality("3D.mp4", new Size(640, wide ? 360 : 480));
                        break; // 3D
                    case 84:
                        q.SetQuality("3D.mp4", new Size(1280, wide ? 720 : 960));
                        break; // 3D
                    case 85:
                        q.SetQuality("3D.mp4", new Size(1920, wide ? 1080 : 1440));
                        break; // 3D
                    case 100:
                        q.SetQuality("3D.webm", new Size(640, wide ? 360 : 480));
                        break; // 3D
                    case 101:
                        q.SetQuality("3D.webm", new Size(640, wide ? 360 : 480));
                        break; // 3D
                    case 102:
                        q.SetQuality("3D.webm", new Size(1280, wide ? 720 : 960));
                        break; // 3D
                    case 120:
                        q.SetQuality("live.flv", new Size(1280, wide ? 720 : 960));
                        break; // Live-streaming - should be ignored?
                    default:
                        q.SetQuality("itag-" + itag, new Size(0, 0));
                        break; // unknown or parse error
                }
                return true;
            }
            return false;
        }

        /// <summary>
        ///     check whether the video is in widescreen format
        /// </summary>
        public static bool IsWideScreen(string html)
        {
            var match =
                Regex.Match(html, @"'IS_WIDESCREEN':\s+(.+?)\s+", RegexOptions.Singleline).Groups[1].ToString()
                    .ToLower()
                    .Trim();
            var res = (match == "true") || (match == "true,");
            return res;
        }

        private static bool GetSize(YouTubeVideoQuality q)
        {
            try
            {
                var fileInfoRequest = (HttpWebRequest) WebRequest.Create(q.DownloadUrl);
                fileInfoRequest.Proxy = Helper.InitialProxy();
                var fileInfoResponse = (HttpWebResponse) fileInfoRequest.GetResponse();
                var bytesLength = fileInfoResponse.ContentLength;
                fileInfoRequest.Abort();
                if (bytesLength != -1)
                {
                    q.SetSize(bytesLength);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
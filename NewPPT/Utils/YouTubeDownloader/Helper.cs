using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace WeCastConvertor.Utils.YouTubeDownloader
{
    public static class FormatLeftTime
    {
        private static readonly string[] TimeUnitsNames =
        {
            "Milli", "Sec", "Min", "Hour", "Day", "Month", "Year",
            "Decade", "Century"
        };

        private static readonly int[] TimeUnitsValue = {1000, 60, 60, 24, 30, 12, 10, 10}; //refrernce unit is milli

        public static string Format(long millis)
        {
            var format = "";
            for (var i = 0; i < TimeUnitsValue.Length; i++)
            {
                var y = millis%TimeUnitsValue[i];
                millis = millis/TimeUnitsValue[i];
                if (y == 0) continue;
                format = y + " " + TimeUnitsNames[i] + " , " + format;
            }

            format = format.Trim(',', ' ');
            if (format == "") return "0 Sec";
            return format;
        }
    }

    public static class Helper
    {
        /// <summary>
        ///     Decode a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        public static bool IsValidUrl(string url)
        {
            const string pattern =
                @"^(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?$";
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex.IsMatch(url);
        }

        /// <summary>
        ///     Gets the txt that lies between these two strings
        /// </summary>
        public static string GetTxtBtwn(string input, string start, string end, int startIndex)
        {
            return GetTxtBtwn(input, start, end, startIndex, false);
        }

        /// <summary>
        ///     Gets the txt that lies between these two strings
        /// </summary>
        public static string GetLastTxtBtwn(string input, string start, string end, int startIndex)
        {
            return GetTxtBtwn(input, start, end, startIndex, true);
        }

        /// <summary>
        ///     Gets the txt that lies between these two strings
        /// </summary>
        private static string GetTxtBtwn(string input, string start, string end, int startIndex, bool useLastIndexOf)
        {
            var index1 = useLastIndexOf
                ? input.LastIndexOf(start, startIndex, StringComparison.Ordinal)
                : input.IndexOf(start, startIndex, StringComparison.Ordinal);
            if (index1 == -1) return "";
            index1 += start.Length;
            var index2 = input.IndexOf(end, index1, StringComparison.Ordinal);
            return index2 == -1 ? input.Substring(index1) : input.Substring(index1, index2 - index1);
        }

        /// <summary>
        ///     Split the input text for this pattren
        /// </summary>
        public static string[] Split(string input, string pattren)
        {
            return Regex.Split(input, pattren);
        }


        /// <summary>
        ///     Returns the content of a given web adress as string.
        /// </summary>
        /// <param name="url">URL of the webpage</param>
        /// <returns>Website content</returns>
        public static string DownloadWebPage(string url)
        {
            return DownloadWebPage(url, null);
        }

        private static string DownloadWebPage(string url, string stopLine)
        {
            // Open a connection
            var webRequestObject = (HttpWebRequest) WebRequest.Create(url);
            webRequestObject.Proxy = InitialProxy();
            // You can also specify additional header values like 
            // the user agent or the referer:
            webRequestObject.UserAgent = ".NET Framework/2.0";

            // Request response:
            var response = webRequestObject.GetResponse();

            // Open data stream:
            var webStream = response.GetResponseStream();

            // Create reader object:
            var reader = new StreamReader(webStream);
            var pageContent = "";
            if (stopLine == null)
                pageContent = reader.ReadToEnd();
            else
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    pageContent += line + Environment.NewLine;
                    if (line != null && line.Contains(stopLine)) break;
                }
            // Cleanup
            reader.Close();
            webStream?.Close();
            response.Close();

            return pageContent;
        }

        /// <summary>
        ///     Get the ID of a youtube video from its URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetVideoIdFromUrl(string url)
        {
            url = url.Substring(url.IndexOf("?", StringComparison.Ordinal) + 1);
            var props = url.Split('&');

            var videoid = "";
            foreach (var prop in props.Where(prop => prop.StartsWith("v=")))
            {
                videoid = prop.Substring(prop.IndexOf("v=", StringComparison.Ordinal) + 2);
            }

            return videoid;
        }


        public static IWebProxy InitialProxy()
        {
            var address = GetIeProxy();
            if (!string.IsNullOrEmpty(address))
            {
                var proxy = new WebProxy(address) {Credentials = CredentialCache.DefaultNetworkCredentials};
                return proxy;
            }
            return null;
        }

        private static string GetIeProxy()
        {
            var p = WebRequest.DefaultWebProxy;
            if (p == null) return null;
            WebProxy webProxy;
            var proxy = p as WebProxy;
            if (proxy != null) webProxy = proxy;
            else
            {
                var t = p.GetType();
                var s = t.GetProperty("WebProxy", (BindingFlags) 0xfff).GetValue(p, null);
                webProxy = s as WebProxy;
            }
            return string.IsNullOrEmpty(webProxy?.Address?.AbsolutePath) ? null : webProxy.Address.Host;
        }
    }
}
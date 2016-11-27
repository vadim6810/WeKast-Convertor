using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using NReco.VideoConverter;
using NReco.VideoInfo;

namespace WeCastConvertor.Converter
{
    internal class VideoCutter
    {
        public VideoCutter(string inputPathToVideo)
        {
            if (inputPathToVideo != null)
                InputPath = Path.Combine(Path.GetDirectoryName(inputPathToVideo),Path.GetFileNameWithoutExtension(inputPathToVideo)+"_1"+Path.GetExtension(inputPathToVideo));
            var settings = new ConvertSettings();
            //settings.VideoFrameRate = (int?)FrameRate;
            //settings.VideoCodec = Format.h264;
            //settings.Seek = fromFrame / FrameRate;
            //Debug.WriteLine($"Seek {slideNumber} - {settings.Seek} - {frameCount} frames");
            //settings.VideoFrameCount = frameCount;
            settings.CustomOutputArgs = " -c:v libx264 -crf 0 ";
            FfMpeg.ConvertMedia(inputPathToVideo, null, InputPath, null, settings);
        }

        private FFMpegConverter FfMpeg { get; } = new FFMpegConverter();
        private MediaInfo VideoInfo { get; set; }
        private float FrameRate { get; set; }

        private string InputPath { get; }

        public bool CheckSum(int sum)
        {
            var videoInfo = GetVideoInfio();
            Debug.WriteLine("VideoDuration: {0}", videoInfo.Duration.Duration());
            var streams = videoInfo.Streams;
            foreach (var stream in streams)
            {
                Debug.WriteLine(stream + " frame rate: " + stream.FrameRate);
            }
            var frameRate = streams[0].FrameRate;
            //MediaInfo.StreamInfo = 
            var frameCount = (int) (videoInfo.Duration.Duration().TotalMilliseconds*frameRate/1000);
            Debug.WriteLine("Frame count: {0}", frameCount);
            Debug.WriteLine("Check sum: {0}", sum);
            //Debug.WriteLine("current frame: "+currentFrame+"   FrameCount: "+_reader.FrameCount);
            //return currentFrame == _reader.FrameCount;
            return frameCount==sum;
        }

        internal void SaveAnimation(int fromFrame, int frameCount, int slideNumber, int animId, string pathToVideo)
        {
            if (VideoInfo == null)
            {
                VideoInfo = GetVideoInfio();
                Debug.WriteLine("VideoDuration: {0}", VideoInfo.Duration.Duration());
                var streams = VideoInfo.Streams;
                foreach (var stream in streams)
                {
                    Debug.WriteLine(stream + " frame rate: " + stream.FrameRate);
                }
                FrameRate = streams[0].FrameRate;
            }
            //Debug.WriteLine($"Cut from frame {fromFrame} - {frameCount} frames");
            var settings = new ConvertSettings();
            settings.VideoFrameRate = (int?) FrameRate;
            //settings.VideoCodec = Format.h264;
            settings.Seek = fromFrame/FrameRate;
            Debug.WriteLine($"Seek {slideNumber} - {settings.Seek} - {frameCount} frames");
            settings.VideoFrameCount = frameCount;
            settings.CustomOutputArgs = " -c:v libx264 -crf 18 ";
            FfMpeg.ConvertMedia(InputPath, null, pathToVideo, null, settings);
            //if (hasFirstFrame)
            //    GetBitmap();
            //VideoFileWriter writer = new VideoFileWriter();
            //writer.Open(inputPathToVideo, _reader.Width, _reader.Height, _reader.FrameRate, VideoCodec.MPEG4,100);
            //for (int i = 0; i < count; i++)
            //{
            //    Bitmap bmp = GetBitmap();
            //    writer.WriteVideoFrame(bmp);
            //    if (!hasLastFrame && (i == count - 1))
            //    {
            //        bmp.Save(pathToPicture);
            //    }
            //}
            //try
            //{
            //    writer.Close();
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //if (hasLastFrame)
            //{
            //    var bmp = GetBitmap();
            //    bmp.Save(pathToPicture);

            //}
        }

        internal void SaveAnimation(int fromFrame, int animId, string pictureName)
        {
            float? position = fromFrame/FrameRate;
            FfMpeg.GetVideoThumbnail(InputPath, pictureName, position);
        }

        private Bitmap GetBitmap()
        {
            //currentFrame++;
            //return _reader.ReadVideoFrame();
            return null;
        }

        private MediaInfo GetVideoInfio()
        {
            var ffProbe = new FFProbe();
            while (IsFileLocked(InputPath))
                Thread.Sleep(100);
            return ffProbe.GetMediaInfo(InputPath);
        }

        private bool IsFileLocked(string filename)
        {
            var result = false;
            var fileinfo = new FileInfo(filename);
            try
            {
                Stream stream = fileinfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                stream.Dispose();
            }
            catch (IOException)
            {
                result = true;
            }
            return result;
        }
    }
}
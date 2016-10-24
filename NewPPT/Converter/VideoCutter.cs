using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NReco.VideoInfo;
using NReco.VideoConverter;

namespace WeCastConvertor.Converter
{
    class VideoCutter
    {
        //private readonly VideoFileReader _reader = new VideoFileReader();
        private FFMpegConverter FfMpeg { get; } = new FFMpegConverter();
        private MediaInfo VideoInfo { get; set; }
        private float FrameRate { get; set; }

        private string InputPath { get; }
        //private int currentFrame { get; set; }

        public VideoCutter(string inputPathToVideo)
        {
            InputPath = inputPathToVideo;

        }


        //private void OpenVideo()
        //{
        //    try
        //    {
        //        //_reader.Open(_path);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public void CloseVideo()
        {
            try
            {
                //_reader.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SkipFrames(int count)
        {
            //currentFrame += count;
        }

        public bool CheckSum(int sum)
        {
            var videoInfo = GetVideoInfio();
            Debug.WriteLine("VideoDuration: {0}", videoInfo.Duration.Duration());
            MediaInfo.StreamInfo[] streams = videoInfo.Streams;
            foreach (var stream in streams)
            {
                Debug.WriteLine(stream + " frame rate: " + stream.FrameRate);
            }
            var frameRate = streams[0].FrameRate;
            //MediaInfo.StreamInfo = 
            int frameCount = (int)(videoInfo.Duration.Duration().TotalMilliseconds * frameRate / 1000);
            Debug.WriteLine("Frame count: {0}", frameCount);
            Debug.WriteLine("Check sum: {0}", sum);
            //Debug.WriteLine("current frame: "+currentFrame+"   FrameCount: "+_reader.FrameCount);
            //return currentFrame == _reader.FrameCount;
            return false;
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
            Debug.WriteLine($"Cut from frame {fromFrame} - {frameCount} frames");
            var settings = new ConvertSettings();
            settings.VideoFrameRate = (int?)FrameRate;
            settings.VideoCodec = Format.h264;
            settings.Seek = fromFrame / (int)FrameRate;
            settings.VideoFrameCount = frameCount;
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
            float? position = fromFrame / FrameRate;
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

        private bool IsFileLocked(String filename)
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

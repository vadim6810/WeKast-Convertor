using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeCastConvertor.Converter
{
    class VideoCutter
    {
        private readonly VideoFileReader _reader = new VideoFileReader();
        string _path;
        private int currentFrame { get; set; }

        public VideoCutter(string pathToVideo)
        {
            _path = pathToVideo;
        }

        public void OpenVideo()
        {
            try
            {
                _reader.Open(_path);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void CloseVideo()
        {
            try
            {
                _reader.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SkipFrames(int count)
        {
            currentFrame += count;
        }

        public bool CheckSum()
        {
            Debug.WriteLine("current frame: "+currentFrame+"   FrameCount: "+_reader.FrameCount);
            return currentFrame == _reader.FrameCount;
        }

        public void SaveAnimation(int count, int slideNumber, int animId, string pathToVideo, string pathToPicture)
        {
            //throw new NotImplementedException();
            VideoFileWriter writer = new VideoFileWriter();
            for (int i = 0; i < count; i++)
            {
                Bitmap bmp = _reader.ReadVideoFrame();
            }
        }

        internal void SaveAnimation(int count, int slideNumber, int animId, string pathToVideo, string pathToPicture, bool hasFirstFrame, bool hasLastFrame)
        {
            if (hasFirstFrame)
                GetBitmap();
            VideoFileWriter writer = new VideoFileWriter();
            writer.Open(pathToVideo, _reader.Width, _reader.Height, _reader.FrameRate, VideoCodec.MPEG4,100);
            for (int i = 0; i < count; i++)
            {
                Bitmap bmp = GetBitmap();
                writer.WriteVideoFrame(bmp);
                if (!hasLastFrame && (i == count - 1))
                {
                    bmp.Save(pathToPicture);
                }
            }
            try
            {
                writer.Close();
            }
            catch (Exception)
            {
                
                throw;
            }
            if (hasLastFrame)
            {
                var bmp = GetBitmap();
                bmp.Save(pathToPicture);

            }


        }

        private Bitmap GetBitmap()
        {
            currentFrame++;
            return _reader.ReadVideoFrame();
        }
    }
}

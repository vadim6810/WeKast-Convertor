using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
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
            _reader.Open(_path);
        }

        public void CloseVideo()
        {
            _reader.Close();
        }

        public void SkipFrames(int i)
        {
            currentFrame += 2;
        }

        public bool CheckSum()
        {
            return currentFrame == _reader.FrameCount;
        }
    }
}

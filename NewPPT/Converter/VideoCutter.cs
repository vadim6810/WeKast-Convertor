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
            throw new NotImplementedException();
        }

        public void CheckSum()
        {
            throw new NotImplementedException();
        }
    }
}

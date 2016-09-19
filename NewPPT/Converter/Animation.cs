namespace WeCastConvertor.Converter
{
    internal class Animation
    {
        public Animation(int id, int fromSlide, int numberOfFrames, string pathToVideo, string pathToPicture)
        {
            AnimId = id;
            FromSlide = fromSlide;
            NumberOfFrames = numberOfFrames;
            PathToVideo = pathToVideo;
            PathToPicture = pathToPicture;
        }

        public int AnimId { get; }
        public int FromSlide { get; }
        public int NumberOfFrames { get; }
        public string PathToVideo { get; }
        public string PathToPicture { get; }
    }
}
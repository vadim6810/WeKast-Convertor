using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WeCastConvertor.Converter
{
    internal class InfoWriter
    {
        private readonly XDocument _doc;
        private readonly string _fileName;
        private readonly XElement _root;

        public InfoWriter(string pathToXml)
        {
            _fileName = pathToXml;
            _doc = new XDocument();
            _root = new XElement("_root");
            _doc.Add(_root);
        }

        public void AddSlide(int slideId)
        {
            var slide = new XElement("slide",
                new XAttribute("id", slideId));
            _root.Add(slide);
        }

        public void AddAnimation(int slideId, int animId, string pathToVideo, string pathToEndState)
        {
            var slide =
                _doc.Root?.Elements()
                    .FirstOrDefault(node => node.Name == "slide" && node.Attribute("id").Value == slideId.ToString());
            var animation = new XElement("animation",
                new XAttribute("id", animId),
                new XAttribute("video", pathToVideo),
                new XAttribute("picture", pathToEndState));
            slide?.Add(animation);
        }

        public void AddAttribute(int slideNumber, string attrName, StringBuilder value)
        {
            var slide =
                _doc.Root?.Elements()
                    .FirstOrDefault(node => node.Name == "slide" && node.Attribute("id").Value == slideNumber.ToString());
            slide?.Add(new XAttribute(attrName,value));
        }

        public void Save()
        {
            _doc.Save(_fileName);
        }

        public void CheckSum()
        {
            //throw new System.NotImplementedException();
        }

        public void AddSlidePicture(int slideNumber, string pathToPicture)
        {
            //throw new System.NotImplementedException();
        }
    }
}
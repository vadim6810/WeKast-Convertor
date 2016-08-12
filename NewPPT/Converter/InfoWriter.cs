using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Schema;

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

        public XElement AddSlide(int slideId)
        {
            var slide = GetSlideNodeById(slideId);
            if (slide != null) return slide;
            slide = new XElement("slide",
                new XAttribute("id", slideId));
            _root.Add(slide);
            return slide;
        }

        private XElement GetSlideNodeById(int slideId)
        {
            try
            {
                return _doc.Root?.Elements()
                            .FirstOrDefault(node => node.Name == "slide" && node.Attribute("id").Value == slideId.ToString());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public XElement AddAnimation(int slideId, int animId, string pathToVideo, string pathToEndState)
        {
            var slide = AddSlide(slideId);
            //GetSlideNodeById(slideId);
            //_doc.Root?.Elements()
            //    .FirstOrDefault(node => node.Name == "slide" && node.Attribute("id").Value == slideId.ToString());
            var animation = new XElement("animation",
                new XAttribute("id", animId),
                new XAttribute("video", pathToVideo),
                new XAttribute("picture", pathToEndState));
            slide?.Add(animation);
            return animation;
        }

        public void AddAttribute(int slideNumber, string attrName, StringBuilder value)
        {
            var slide =
                _doc.Root?.Elements()
                    .FirstOrDefault(node => node.Name == "slide" && node.Attribute("id").Value == slideNumber.ToString());
            slide?.Add(new XAttribute(attrName, value));
        }

        public void Save()
        {
            try
            {
                _doc.Save(_fileName);
            }
            catch (IOException)
            {
                Thread.Sleep(1000);
            }
        }

        public XElement AddSlideMedia(int slideNumber, string pathToMedia, string type)
        {
            var slide = AddSlide(slideNumber);
            var media = new XElement("media",
                //new XAttribute("id", animId),
                new XAttribute("type", type),
                new XAttribute("path", pathToMedia));
            slide?.Add(media);
            return media;
        }
    }
}
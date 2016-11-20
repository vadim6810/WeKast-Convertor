using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeCastConvertor.Converter;

namespace WeCastTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPdfConvert()
        {
            Converter converter = new PdfConverter();
            converter.Convert(@"d:/access_1.pdf");
        }
    }

}
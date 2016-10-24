using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeCastConvertor.Utils;

namespace WeCastTests.Utils
{
    [TestClass]
    public class ServerApiTests
    {
        private static readonly WeKastServerApi Api = WeKastServerApi.Instance;
        private const string DefaultLogin = "972543928489";
        private const string DefaultPassword = "0iFU54C0";

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            Trace.WriteLine("Assembly Init");
        }

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            Trace.WriteLine("ClassInit");
        }

        [TestInitialize()]
        public void Initialize()
        {
            Trace.WriteLine("TestMethodInit");
        }

        [TestCleanup()]
        public void Cleanup()
        {
            Trace.WriteLine("TestMethodCleanup");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            Trace.WriteLine("ClassCleanup");
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            Trace.WriteLine("AssemblyCleanup");
        }


        [TestMethod]
        public void UploadTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestAuth()
        {
            // arrange
            Api.Login = DefaultLogin;
            Api.Password = DefaultPassword;
            // act
            var authResult = WeKastServerApi.Instance.Auth();
            // assert
            Assert.IsTrue(authResult.Result);
        }

        [TestMethod]
        public void TestList()
        {
            Debug.WriteLine("==========================================================================================");
            // arrange
            Api.Login = DefaultLogin;
            Api.Password = DefaultPassword;
            // act
            var response = WeKastServerApi.Instance.ListAsync(0);
            response.Wait();
            //Trace.WriteLine("werfgsewrgztf");
            // assert
            Assert.IsTrue(response.Result.Answer.Length > 0);
        }

        [TestMethod]
        public void TestPreview()
        {
            // arrange
            Api.Login = DefaultLogin;
            Api.Password = DefaultPassword;
            // act
            //var preview = WeKastServerApi.Instance.GetPreview(3);
            ////assert
            //Assert.IsTrue(preview!=null);
            // act
            var preview = WeKastServerApi.Instance.Preview(3);
            preview.Wait();
            Bitmap bmp = preview.Result;
            bmp.Save("d://preview.jpg", ImageFormat.Jpeg);
            //assert
            //Assert.IsTrue(preview != null);
        }

        [TestMethod]
        public void TestMyDownload()
        {
            // arrange
            Api.Login = DefaultLogin;
            Api.Password = DefaultPassword;
            // act
            WeKastServerApi.Instance.MyUpload();
        }
    }
}
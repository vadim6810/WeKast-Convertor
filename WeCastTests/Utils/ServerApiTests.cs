using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
        private const string DefaultPassword = "KWEYhcdP";
        private const string Url = @"http://78.153.150.254/";
        private const string Upload = "upload";

        //[AssemblyInitialize()]
        //public static void AssemblyInit(TestContext context)
        //{
        //    Trace.WriteLine("Assembly Init");
        //}

        //[ClassInitialize()]
        //public static void ClassInit(TestContext context)
        //{
        //    Trace.WriteLine("ClassInit");
        //}

        //[TestInitialize()]
        //public void Initialize()
        //{
        //    Trace.WriteLine("TestMethodInit");
        //}

        //[TestCleanup()]
        //public void Cleanup()
        //{
        //    Trace.WriteLine("TestMethodCleanup");
        //}

        //[ClassCleanup()]
        //public static void ClassCleanup()
        //{
        //    Trace.WriteLine("ClassCleanup");
        //}

        //[AssemblyCleanup()]
        //public static void AssemblyCleanup()
        //{
        //    Trace.WriteLine("AssemblyCleanup");
        //}


        [TestMethod]
        public void UploadTest()
        {
            var result = MyPostTest();
            result.Wait();
            Trace.WriteLine(result.Result.Content.ReadAsStringAsync().Result);
            Assert.IsTrue(true);
        }

        private async Task<HttpResponseMessage> MyPostTest()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "WeKast Tests/1.0");
                using (var content = new MultipartFormDataContent("A67R7E769FF862SF2M32WLE3345RWD"))
                {
                    //content.Headers.TryAddWithoutValidation("User-Agent", "WeKast Tests/1.0");
                    content.Add(new StringContent(DefaultLogin), "login");
                    content.Add(new StringContent(DefaultPassword), "password");
                    //var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(@"d:/Matem_4_Bogdanovych-M.V.-Lyshenko-G.P. (3).ezs"));
                    //fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    //{
                    //    FileName = "Matem_4_Bogdanovych-M.V.-Lyshenko-G.P. (3).ezs"
                    //};
                    var fileStream = new FileStream(@"d:\Matem.ezs", FileMode.Open,
                        FileAccess.Read);
                    var fileContent = new StreamContent(fileStream);
                    content.Add(fileContent, "file");//, "Matem_4_Bogdanovych-M.V.-Lyshenko-G.P. (3).ezs");
                    Trace.WriteLine(content.ToString());
                    var response = await httpClient.PostAsync(Url + Upload, content);
                    //response.r
                    return response;
                }
            }
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
            Assert.IsTrue(authResult.Result.Status == 0);
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
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

        //[AssemblyInitialize()]
        //public static void AssemblyInit(TestContext context)
        //{
        //    MessageBox.Show("Assembly Init");
        //}

        //[ClassInitialize()]
        //public static void ClassInit(TestContext context)
        //{
        //    MessageBox.Show("ClassInit");
        //}

        //[TestInitialize()]
        //public void Initialize()
        //{
        //    MessageBox.Show("TestMethodInit");
        //}

        //[TestCleanup()]
        //public void Cleanup()
        //{
        //    MessageBox.Show("TestMethodCleanup");
        //}

        //[ClassCleanup()]
        //public static void ClassCleanup()
        //{
        //    MessageBox.Show("ClassCleanup");
        //}

        //[AssemblyCleanup()]
        //public static void AssemblyCleanup()
        //{
        //    MessageBox.Show("AssemblyCleanup");
        //}


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
            // arrange
            Api.Login = DefaultLogin;
            Api.Password = DefaultPassword;
            // act
            var authResult = WeKastServerApi.Instance.Auth();
            // assert
            Assert.IsTrue(authResult.Result);
        }
    }
}
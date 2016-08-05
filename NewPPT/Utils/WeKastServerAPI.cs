using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using P = WeCastConvertor.Forms;

namespace WeCastConvertor.Utils
{
    class WeKastServerAPI
    {

        [DataContract]
        private class ListResponse
        {
            [DataMember(Name = "status")]
            public int Status { get; set; }

            [DataMember(Name = "answer")]
            public Presentation[] Answer { get; set; }
        }

        [DataContract]
        private class Presentation
        {
            [DataMember(Name = "id")]
            public int Id { get; set; }

            [DataMember(Name = "name")]
            public string Name { get; set; }
        }

        [DataContract]
        private class UploadResponse
        {
            [DataMember(Name = "status")]
            public int Status { get; set; }

            [DataMember(Name = "answer")]
            public FileAnswer Answer { get; set; }
        }

        [DataContract]
        private class FileAnswer
        {
            [DataMember(Name = "id")]
            public int Id { get; set; }

            [DataMember(Name = "file")]
            public string File { get; set; }
        }


        private static readonly WeKastServerAPI Inst = new WeKastServerAPI();

        public static WeKastServerAPI Instance
        {
            get
            {
                if (Inst != null)
                {
                    return Inst;
                } 
                throw new Exception("Server API not inited.");
            }
        }

        public string ServerUrl = @"http://78.153.150.254";
        public string Login;
        public string Password;

        protected WeKastServerAPI()
        {
            Login = SharedPreferences.Login;
            Password = SharedPreferences.Password;
        }
            

        private async Task<string> PostRequest(string url, Dictionary<string, string> values)
        {
            var content = new FormUrlEncodedContent(values);
            return await PostRequest(url, content);
        }

        private async Task<string> PostRequest(string url, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                var requestUri = ServerUrl + url;
                var response = await client.PostAsync(requestUri, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<bool> Auth()
        {
            var data = new Dictionary <string, string>
            {
                { "login", Login },
                { "password", Password }
            };

            var response = await PostRequest("/list", data);
            var json = new DataContractJsonSerializer(typeof(ListResponse));
            var listResponse = (ListResponse) json.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(response)));
            return listResponse.Status == 0;
        }

        public async Task<bool> Upload(P.Presentation presentation)
        {
            var file = new FileStream(presentation.SourcePath, FileMode.Open, FileAccess.Read);
            using (var content = new MultipartFormDataContent("A67R7E769FF862SF2M32WLE3345RWD"))
            {
                content.Add(new StringContent(Login), "login");
                content.Add(new StringContent(Password), "password");
                content.Add(new StreamContent(file), "file", "file.ezs");

                var response = await PostRequest("/upload", content);
                var json = new DataContractJsonSerializer(typeof(UploadResponse));
                var uploadResponse = (UploadResponse)json.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(response)));
                return uploadResponse.Status == 0;
            }      
        }
    }
}

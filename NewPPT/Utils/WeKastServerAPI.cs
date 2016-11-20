using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeCastConvertor.Converter;
using P = WeCastConvertor.Forms;

namespace WeCastConvertor.Utils
{
    public class WeKastServerApi
    {
        private static readonly WeKastServerApi Inst = new WeKastServerApi();
        public string Login;
        public string Password;

        public string ServerUrl = @"http://78.153.150.254";

        protected WeKastServerApi()
        {
            Login = SharedPreferences.Login;
            Password = SharedPreferences.Password;
        }

        public static WeKastServerApi Instance
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

        private async Task<string> PostRequest(string url, Dictionary<string, string> values)
        {
            var content = new FormUrlEncodedContent(values);
            return await PostRequest(url, content);
        }

        private async Task<string> PostRequest(string url, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "WeKast PptConverter/1.0");
                var requestUri = ServerUrl + url;
                var response = await client.PostAsync(requestUri, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        private async Task<Stream> PostRequestStream(string url, Dictionary<string, string> values)
        {
            var content = new FormUrlEncodedContent(values);
            return await PostRequestStream(url, content);
        }

        private async Task<Stream> PostRequestStream(string url, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "WeKast PptConverter/1.0");
                var requestUri = ServerUrl + url;
                var response = await client.PostAsync(requestUri, content);
                //Debug.WriteLine("Responce status: {0}",response.StatusCode);
                return await response.Content.ReadAsStreamAsync();
            }
        }



        public async Task<AuthResult> Auth()
        {
            var data = new Dictionary<string, string>
            {
                {"login", Login},
                {"password", Password}
            };
            var response = await PostRequest("/list", data);
            Debug.WriteLine($"response: {response}");
            var json = new DataContractJsonSerializer(typeof(ListResponse));
            var listResponse = (ListResponse)json.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(response)));
            AuthResult result = null;
            if (listResponse.Status == 0)
                result = new AuthResult(0, null);
            else
                result = GetError(response);
            return result;
        }

        private static string GetErrorMessage(string response)
        {
            var json = new DataContractJsonSerializer(typeof(ErrorAnswer));
            var errorAnswer =
                (ErrorAnswer)json.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(response)));
            return errorAnswer.Error;
        }

        private static AuthResult GetError(string response)
        {
            var json = new DataContractJsonSerializer(typeof(ErrorAnswer));
            var errorAnswer =
                (ErrorAnswer)json.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(response)));
            var result = new AuthResult(errorAnswer.Status, errorAnswer.Error);
            return result;
        }

        public async Task<bool> Upload(P.Presentation presentation)
        {
            ProcessHandler.OnStatusChanged("Updloading");
            var path = presentation.EzsPath;
            var name = Path.GetFileName(path);
            Console.WriteLine(@"Uploading " + path);
            if (!File.Exists(path))
            {
                Console.WriteLine($"Coudn't upload {path}: File not found");
                return false;
            }
            var file = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var content = new MultipartFormDataContent("A67R7E769FF862SF2M32WLE3345RWD"))
            {
                content.Add(new StringContent(Login), "login");
                content.Add(new StringContent(Password), "password");
                var stream = new StreamContent(file);
                content.Add(stream, "file", name);
                var response = await PostRequest("/upload", content);
                Debug.WriteLine($"response: {response}");
                try
                {
                    var json = new DataContractJsonSerializer(typeof(UploadResponse));
                    var uploadResponse =
                        (UploadResponse)json.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(response)));
                    Console.WriteLine(@"Presentation uploaded " + path);
                    if (uploadResponse.Status != 0) return false;
                    presentation.Upload = 100;
                    return true;
                }
                catch (SerializationException)
                {
                    Console.WriteLine(@"Cound't parse answer");
                    Console.WriteLine(response);
                    return false;
                }
            }
        }

        public async Task<ListResponse> ListAsync(int page = -1)
        {
            var data = new Dictionary<string, string>
            {
                {"login", Login},
                {"password", Password}
            };
            var response = await PostRequest($"/list/{page}", data);
            //Debug.WriteLine(response);
            var json = new DataContractJsonSerializer(typeof(ListResponse));
            var listResponse =
                (ListResponse)json.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(response)));
            //Debug.WriteLine("Responce status: "+listResponse.Status);
            //Debug.WriteLine(listResponse.Answer);
            //foreach (var presantation in listResponse.Answer)
            //{
            //    Debug.WriteLine("presantation.Name={0} presantation.Id={1}", presantation.Name, presantation.Id);
            //}
            return listResponse;
        }

        public async Task<Bitmap> Preview(int id)
        {
            var data = new Dictionary<string, string>
            {
                {"login", Login},
                {"password", Password}
            };
            var content = new FormUrlEncodedContent(data);
            var stream = await PostRequestStream($"/preview/{id}", content);
            //= await PostRequestStream(, data);
            try
            {
                var res = new Bitmap(stream);
                return res;
            }
            catch
            {
                Debug.WriteLine("Error downloading preview id={0}", id);
                Debug.WriteLine(StreamToString(stream));
                //GetErrorMessage(stream.)
                return null;
            }
        }
        private string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public async void MyUpload()
        {
            var data = new Dictionary<string, string>
            {
                {"login", Login},
                {"password", Password}
            };
            var content = new FormUrlEncodedContent(data);
            var client = new HttpClient();
            Debug.WriteLine("==============================");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "WeKast PptConverter/1.0");
            Debug.WriteLine("==============================");
            var requestUri = ServerUrl + "/preview/{3}";
            Debug.WriteLine("==============================");
            try
            {
                var resp = client.PostAsync(requestUri, content);
                Debug.WriteLine("==============================");
                //resp.GetAwaiter().GetResult().
                //Stream stream2 = await client.GetStreamAsync(requestUri.ToString, content);
                //Debug.WriteLine("==============================");// + stream2.CanRead);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<bool> Delete(int presId)
        {
            var data = new Dictionary<string, string>
            {
                {"login", Login},
                {"password", Password}
            };
            var response = await PostRequest($"/delete/{presId}", data);
            Debug.WriteLine($"response: {response}");
            var json = new DataContractJsonSerializer(typeof(DeleteResponse));
            var deleteResponse = (DeleteResponse)json.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(response)));

            if (deleteResponse.Status != 0)
            {
                GetError(response);
                return false;
            }
            return true;
        }

        [DataContract]
        public class ResponseAnswer
        {
            [DataMember(Name = "status")]
            public int Status { get; set; }

            [DataMember(Name = "answer")]
            public object Answer { get; set; }
        }

        [DataContract]
        public class DeleteResponse
        {
            [DataMember(Name = "status")]
            public int Status { get; set; }

            [DataMember(Name = "answer")]
            public int Answer { get; set; }
        }

        [DataContract]
        public class ListResponse
        {
            [DataMember(Name = "status")]
            public int Status { get; set; }

            [DataMember(Name = "answer")]
            public Presentation[] Answer { get; set; }
        }

        [DataContract]
        public class Presentation
        {
            [DataMember(Name = "id")]
            public int Id { get; set; }

            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "hash")]
            public string Hash { get; set; }

            [DataMember(Name = "size")]
            public int Size { get; set; }

            [DataMember(Name = "type")]
            public string Type { get; set; }

            [DataMember(Name = "updated_at")]
            public string Date { get; set; }
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


        [DataContract]
        private class ErrorAnswer
        {
            [DataMember(Name = "status")]
            public int Status { get; set; }

            [DataMember(Name = "error")]
            public string Error { get; set; }
        }


    }

    public class AuthResult
    {
        public int Status { get; }
        public string Message { get; }

        public AuthResult(int status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
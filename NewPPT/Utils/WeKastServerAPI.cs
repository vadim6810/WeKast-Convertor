using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

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



        private static readonly WeKastServerAPI instance = new WeKastServerAPI();

        public static WeKastServerAPI Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                } else
                {
                    throw new Exception("Server API not inited.");
                }
            }
        }

        public string serverUrl;
        public string login;
        public string password;

        protected WeKastServerAPI()
        {
        }
            

        private async Task<string> postRequest(string url, Dictionary<string, string> values)
        {
            using (HttpClient client = new HttpClient())
            {
                FormUrlEncodedContent content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(serverUrl + url, content);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<bool> auth()
        {
            var data = new Dictionary <string, string>
            {
                { "login", login },
                { "password", password }
            };

            var response = await postRequest("/list", data);

            Console.WriteLine(response);

            var json = new DataContractJsonSerializer(typeof(ListResponse));
            
            var listResponse = (ListResponse) json.ReadObject(new System.IO.MemoryStream(Encoding.Unicode.GetBytes(response)));

            return listResponse.Status == 0;
        }
    }
}

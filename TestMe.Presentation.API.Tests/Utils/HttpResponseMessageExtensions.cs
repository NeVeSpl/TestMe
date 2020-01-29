using System.Net.Http;
using Newtonsoft.Json;

namespace TestMe.Presentation.API.Tests.Utils
{
    internal static class HttpResponseMessageExtensions
    {
        public static Content<T> GetContent<T>(this HttpResponseMessage response)
        {
            var content = new Content<T>
            {
                Text = response.Content.ReadAsStringAsync().Result
            };
            if (response.IsSuccessStatusCode)
            {
                content.Value = JsonConvert.DeserializeObject<T>(content.Text);
            }
            response.EnsureSuccessStatusCode();
            return content;
        }

        public static string GetContent(this HttpResponseMessage response)
        {
            var content  = response.Content.ReadAsStringAsync().Result;            
            return content;
        }
    }

    public struct Content<T>
    {
        public string Text;
        public T Value;
    }
}

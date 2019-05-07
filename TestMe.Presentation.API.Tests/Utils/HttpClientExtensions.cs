using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace System.Net.Http
{
    internal static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> GetAsync(this HttpClient client, string requestUri, object content)
        {
            return client.GetAsync(requestUri, ConvertToByteArrayContent(content));
        }
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string requestUri, object content)
        {
            return client.PostAsync(requestUri, ConvertToByteArrayContent(content));
        }
        public static Task<HttpResponseMessage> PutAsync(this HttpClient client, string requestUri, object content)
        {
            return client.PutAsync(requestUri, ConvertToByteArrayContent(content));
        }
        public static Task<HttpResponseMessage> DeleteAsync(this HttpClient client, string requestUri, object content)
        {
            return client.DeleteAsync(requestUri, ConvertToByteArrayContent(content));
        }


        private static ByteArrayContent ConvertToByteArrayContent(object content)
        {
            var jsonContent = JsonConvert.SerializeObject(content);
            var buffer = Encoding.UTF8.GetBytes(jsonContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
    }
}

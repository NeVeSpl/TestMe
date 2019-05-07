using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.Presentation.API.Services;
using TestMe.UserManagement.Domain;

namespace TestMe.Presentation.API.Tests.Utils
{
    public abstract class BaseFixture
    {
        private protected static readonly string Token;
        private protected ApiFactory factory;
        


        static BaseFixture()
        {
            Token = AuthenticationService.BuildToken(new User() { UserId = 1 }, "https://localhost:44357", "196A813D-9E9B-48BD-85C2-E90DE807BBDD");
        }



        protected async Task CheckAuthorization(string httpMethod, string requestUri)
        {
            var client = factory.CreateClient();

            HttpResponseMessage response = null;

            switch (httpMethod)
            {
                case "Get":
                    response = await client.GetAsync(requestUri);
                    break;
                case "Post":
                    response = await client.PostAsync(requestUri, new { });
                    break;
                case "Put":
                    response = await client.PutAsync(requestUri, new { });
                    break;
                case "Delete":
                    response = await client.DeleteAsync(requestUri);
                    break;
            }

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}

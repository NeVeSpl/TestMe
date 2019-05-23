using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestMe.Presentation.API.Tests.Utils;
using TestMe.UserManagement.App.Users.DTO;

namespace TestMe.Presentation.API.Tests
{
    [TestClass]
    public class Tokens_HappyPath : BaseFixture
    {
        private const string EndpointName = "Tokens";
        private HttpClient client;


        [TestInitialize]
        public void TestInitialize()
        {
            factory = new ApiFactory(ApiFactory.DatabaseType.SQLiteInMemory);
            client = factory.CreateClient(ValidToken);
            SeedDatabase(factory);
        }       


        [DataTestMethod]
        [DataRow("User A", "12345678", "1")]
        [DataRow("User B", "abcdefghj", "2")]
        public async Task CreateToken(string userName, string password, string userId)
        {          
            var command = new LoginCredentials() {  Login = userName, Password = password };

            var response = await client.PostAsync(EndpointName, command);
            
            var content = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            var anonymousTypeObject = new { Token = "" };
            var responseObject =  JsonConvert.DeserializeAnonymousType(content, anonymousTypeObject);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(responseObject.Token) as JwtSecurityToken;
            var userIdFromToken = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            Assert.AreEqual(userId, userIdFromToken);
        }
    }
}

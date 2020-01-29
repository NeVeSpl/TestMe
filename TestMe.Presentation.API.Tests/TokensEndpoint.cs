using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.Tests;
using TestMe.Presentation.API.Controllers.Tokens.Input;
using TestMe.Presentation.API.Controllers.Tokens.Output;
using TestMe.Presentation.API.Tests.Utils;
using TestMe.UserManagement.Domain;
using static TestMe.UserManagement.TestUtils;

namespace TestMe.Presentation.API.Tests
{
    [TestClass]
    public class TokensEndpoint : BaseFixture
    {
        private const string EndpointName = "Tokens";
        private HttpClient client;


        [TestInitialize]
        public void TestInitialize()
        {
            factory = new ApiFactory(FakeDatabaseType.SQLiteInMemory, MockServices, SeedDatabase);
            client = factory.CreateClient(ValidOwnerToken);            
        }       


        [DataTestMethod]
        [DataRow(ValidUser1Mail, ValidUser1Password, ValidUser1Id, ValidUser1Role)]
        [DataRow(ValidUser2Mail, ValidUser2Password, ValidUser2Id, ValidUser2Role)]
        public async Task CreateToken_HappyPathIsSuccessful(string userName, string password, long userId, UserRole userRole)
        {          
            var payload = new LoginCredentialsDTO()
            { 
                Email = userName, 
                Password = password 
            };

            var response = await client.PostAsync(EndpointName, payload);
            AssertExt.EnsureSuccessStatusCode(response);

            var responseObject = response.GetContent<TokenDTO>().Value;

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(responseObject.Token) as JwtSecurityToken;
            var userIdFromToken = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var userRoleFromToken = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;

            Assert.AreEqual(userId.ToString(), userIdFromToken);
            Assert.AreEqual(userRole.ToString(), userRoleFromToken);
        }
    }
}

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.App;
using TestMe.BuildingBlocks.Tests;
using TestMe.Presentation.API.Controllers.Users.Input;
using TestMe.Presentation.API.Tests.Utils;
using TestMe.TestCreation.Persistence;
using TestMe.UserManagement.App.Users.Output;
using TestMe.UserManagement.Persistence;

namespace TestMe.Presentation.API.Tests
{
    [TestClass]
    public class UsersEndpoint : BaseFixture
    {
        private const string EndpointName = "Users";
        private HttpClient client;


        [TestInitialize]
        public void TestInitialize()
        {
            factory = new ApiFactory(FakeDatabaseType.SQLiteInMemory, MockServices, SeedDatabase);
            client = factory.CreateClient(ValidOwnerToken);
        }


        [TestMethod]
        public async Task CreateUser_HappyPathIsSuccessful()
        {
            var payload = new CreateUserDTO()
            {
               Name = "Mr robot",
               EmailAddress = "eliot@ecorp.us",
               Password = "n00b&r00t",
            };

            var response = await client.PostAsync(EndpointName, payload);
            AssertExt.EnsureSuccessStatusCode(response);

            var createdUserId = response.GetContent<long>().Value;
            var userManagementContext = factory.GetService<UserManagementDbContext>();
            var createdUser = userManagementContext.Users.FirstOrDefault(x => x.UserId == createdUserId);

            Assert.AreEqual(payload.Name, createdUser.Name);
            Assert.AreEqual(payload.EmailAddress, createdUser.EmailAddress.Value);
            
            await factory.DispatchEvents();

            var testCreationDbContext = factory.GetService<TestCreationDbContext>();            
            var createdOwner = testCreationDbContext.Owners.FirstOrDefault(x => x.OwnerId == createdUserId);
            Assert.IsNotNull(createdOwner);
        }

        [TestMethod]
        public async Task ReadUsers_HappyPathIsSuccessful()
        {
            client = factory.CreateClient(ValidAdminToken);
            var response = await client.GetAsync(EndpointName);
            AssertExt.EnsureSuccessStatusCode(response);

            var userManagementContext = factory.GetService<UserManagementDbContext>();
            var expectedUsers = userManagementContext.Users.ToList();

            var users = response.GetContent<CursorPagedResults<UserDTO>>().Value;
            AssertExt.AreEquivalent(expectedUsers, users.Result);
        }
    }
}

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.EventBus;
using TestMe.Infrastructure.EventBus.InMemory;
using TestMe.Presentation.API.Services;
using TestMe.TestCreation;
using TestMe.TestCreation.Persistence;
using TestMe.UserManagement.App.Users.Output;
using TestMe.UserManagement.Domain;
using TestMe.UserManagement.Persistence;

namespace TestMe.Presentation.API.Tests.Utils
{
    public abstract class BaseFixture
    {
        private const string JWTIssuer = "https://localhost:44357";
        private const string JWTKey = "196A813D-9E9B-48BD-85C2-E90DE807BBDD";
        private protected static readonly string InvalidToken;
        private protected static readonly string ValidOwnerToken;
        private protected static readonly string ValidAdminToken;
        private protected ApiFactory factory;


        static BaseFixture()
        {
            ValidOwnerToken = AuthenticationService.BuildToken(userCredentials: new UserCredentialsDTO(TestUtils.OwnerId) { UserRole = UserRole.Regular }, JWTIssuer, JWTKey);
            ValidAdminToken = AuthenticationService.BuildToken(userCredentials: new UserCredentialsDTO(TestUtils.OwnerId) { UserRole = UserRole.Admin }, JWTIssuer, JWTKey);
        }


        private protected void MockServices(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, InMemoryEventBus>();
        }

        private protected void SeedDatabase(IServiceScope serviceScope)
        {            
            using (var context = serviceScope.ServiceProvider.GetRequiredService<TestCreationDbContext>())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                TestCreation.TestUtils.Seed(context);
            }
            using (var context = serviceScope.ServiceProvider.GetRequiredService<UserManagementDbContext>())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                UserManagement.TestUtils.Seed(context);
            }            
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

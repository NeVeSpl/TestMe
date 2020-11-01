using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestMe.BuildingBlocks.Tests;
using TestMe.Presentation.API.BackgroundServices;
using TestMe.TestCreation.Persistence;
using TestMe.UserManagement.Persistence;

namespace TestMe.Presentation.API.Tests.Utils
{
    internal sealed class ApiFactory : WebApplicationFactory<Startup>
    {
        private readonly FakeContextDefinition<TestCreationDbContext> testCreationFakeContextDefinition;
        private readonly FakeContextDefinition<UserManagementDbContext> userManagementFakeContextDefinition;
        private readonly Action<IServiceCollection> configureServices;
        private readonly Action<IServiceScope> onWebHostReady;
        private IServiceScope serviceScope;
       

        public ApiFactory(FakeDatabaseType databaseType, Action<IServiceCollection> configureServices = null, Action<IServiceScope> onWebHostReady = null)
        { 
            this.configureServices = configureServices;
            this.onWebHostReady = onWebHostReady;
            testCreationFakeContextDefinition = new FakeContextDefinition<TestCreationDbContext>(databaseType);
            userManagementFakeContextDefinition = new FakeContextDefinition<UserManagementDbContext>(databaseType);
        }

        public HttpClient CreateClient(string token)
        {
            var httpClient = base.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return httpClient;
        }
        public TService GetService<TService>()
        {
            serviceScope ??= Server.Services.CreateScope();
            TService service = serviceScope.ServiceProvider.GetRequiredService<TService>();            
            return service;
        }
        /// <summary>
        /// Dispatches events that are waiting in outbox
        /// </summary>    
        public Task DispatchEvents()
        {
            serviceScope ??= Server.Services.CreateScope();
            PostManService postman = serviceScope.ServiceProvider.GetServices<IHostedService>().OfType<PostManService>().First();
            return postman.DispatchMessages(default);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        { 
            builder.ConfigureTestServices(services =>
            {
                // It appears that in .net core 3.0 DbContextOptions provided for DbContext are registered in DI container as singletons behind our backs,
                // in order to use our fake DbContext we first need to remove these singletons 
                RemoveService(services, "DbContextOptions`1"); 

                services.AddDbContextPool<TestCreationDbContext>(options => testCreationFakeContextDefinition.SetupBuilder(options));
                services.AddDbContextPool<ReadOnlyTestCreationDbContext>(options => testCreationFakeContextDefinition.SetupBuilder(options));
                services.AddDbContextPool<UserManagementDbContext>(options => userManagementFakeContextDefinition.SetupBuilder(options));                

                configureServices?.Invoke(services);

                var serviceProvider = services.BuildServiceProvider();
               
                using (var scope = serviceProvider.CreateScope())
                {
                    onWebHostReady?.Invoke(scope);
                }
            });
        }

        private void RemoveService(IServiceCollection services, string name)
        {          
            ServiceDescriptor founded = null;
            do
            {
                founded = services.FirstOrDefault(x => x.ServiceType.Name == name);
                if (founded != null)
                {
                    services.Remove(founded);
                }
            } while (founded != null);
        }

        protected override void Dispose(bool disposing)
        {
            serviceScope?.Dispose();
            testCreationFakeContextDefinition.Cleanup();
            userManagementFakeContextDefinition.Cleanup();
            base.Dispose(disposing);
        }
    }
}
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.Presentation.API.Tests.Utils;
using TestMe.TestCreation.App.Catalogs.Input;
using TestMe.TestCreation.App.Catalogs.Output;
using TestMe.TestCreation.Domain;
using TestMe.TestCreation.Persistence;

namespace TestMe.Presentation.API.Tests
{
    [TestClass]
    public class QuestionsCatalogs_HappyPath : BaseFixture
    {
        private const string EndpointName = "QuestionsCatalogs";
        private HttpClient client;


        [TestInitialize]
        public void TestInitialize()
        {
            factory = new ApiFactory(ApiFactory.DatabaseType.SQLiteInMemory);
            client = factory.CreateClient(Token);
        }


        [TestMethod]
        public async Task ReadCatalogHeaders()
        {
            var response = await client.GetAsync($"{EndpointName}/headers");
            
            var catalogs = response.GetContent<CatalogHeaderDTO[]>().Value;
            var context = factory.GetContext<TestCreationDbContext>();
            var expectedCatalogs = context.QuestionsCatalogs.Where(x => x.OwnerId == 1 && x.IsDeleted == false).ToList();

            AssertExt.AreEquivalent(expectedCatalogs, catalogs);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public async Task ReadCatalog(long catalogId)
        {
            var response = await client.GetAsync($"{EndpointName}/{catalogId}");

            var catalog = response.GetContent<CatalogDTO>().Value;
            var context = factory.GetContext<TestCreationDbContext>();
            var expectedCatalog = context.QuestionsCatalogs.FirstOrDefault(x => x.CatalogId == catalogId);

            AssertExt.AreEquivalent(expectedCatalog, catalog);
        }

        [TestMethod]
        [DataRow("New Catalog 1")]
        [DataRow("New Catalog 2")]
        public async Task CreateCatalog(string catalogName)
        {           
            var command = new CreateCatalog() { Name = catalogName };          

            var response = await client.PostAsync(EndpointName, command);
           
            var createdCatalogId = response.GetContent<long>().Value;
            var context = factory.GetContext<TestCreationDbContext>();            
            var catalog = context.QuestionsCatalogs.Find(createdCatalogId);

            Assert.AreEqual(catalogName, catalog.Name);           
        }

        [TestMethod]
        [DataRow(1, "New Catalog name 1")]
        [DataRow(2, "New Catalog name 2")]
        public async Task UpdateCatalog(long catalogId, string catalogName)
        {           
            var command = new UpdateCatalog() { Name = catalogName };          

            var response = await client.PutAsync($"{EndpointName}/{catalogId}/", command);

            response.EnsureSuccessStatusCode();
            var context = factory.GetContext<TestCreationDbContext>();
            var actualCatalog = context.QuestionsCatalogs.Find(catalogId);

            Assert.AreEqual(catalogName, actualCatalog.Name);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task DeleteCatalog(long catalogId)
        { 
            var response = await client.DeleteAsync($"{EndpointName}/{catalogId}/");

            response.EnsureSuccessStatusCode();
            var context = factory.GetContext<TestCreationDbContext>();
            var catalog = context.QuestionsCatalogs.IgnoreQueryFilters().Include(x => x.Questions).Where(x => x.CatalogId == catalogId).FirstOrDefault();

            Assert.AreEqual(true, catalog.IsDeleted);
            foreach (Question question in catalog.Questions)
            {
                Assert.AreEqual(true, question.IsDeleted);
            }            
        }

        [TestMethod]
        [DataRow("Get", EndpointName+ "/headers")]
        [DataRow("Get", EndpointName + "/1")]
        [DataRow("Post", EndpointName)]
        [DataRow("Put", EndpointName + "/1")]
        [DataRow("Delete", EndpointName + "/1")]
        public new async Task CheckAuthorization(string httpMethod, string requestUri)
        {
            await base.CheckAuthorization(httpMethod, requestUri);
        }
    }
}

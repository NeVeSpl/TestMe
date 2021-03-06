﻿using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.App;
using TestMe.BuildingBlocks.Tests;
using TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input;
using TestMe.Presentation.API.Tests.Utils;
using TestMe.TestCreation;
using TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalog;
using TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalogs;
using TestMe.TestCreation.Domain;
using TestMe.TestCreation.Persistence;

namespace TestMe.Presentation.API.Tests
{
    [TestClass]
    public class QuestionsCatalogsEndpoint : BaseFixture
    {
        private const string EndpointName = "QuestionsCatalogs";
        private HttpClient client;


        [TestInitialize]
        public void TestInitialize()
        {
            factory = new ApiFactory(FakeDatabaseType.SQLiteInMemory, MockServices, SeedDatabase);
            client = factory.CreateClient(ValidOwnerToken);          
        }


        [TestMethod]     
        public async Task CatalogsCanBeReadForGivenOwner()
        {
            var response = await client.GetAsync($"{EndpointName}/?ownerId={TestUtils.OwnerId}");
            AssertExt.EnsureSuccessStatusCode(response);
            
            var catalogs = response.GetContent<OffsetPagedResults<CatalogOnListDTO>>().Value;
            var context = factory.GetService<TestCreationDbContext>();
            var expectedCatalogs = context.QuestionsCatalogs.Where(x => x.OwnerId == TestUtils.OwnerId).ToList();

            AssertExt.AreEquivalent(expectedCatalogs, catalogs.Result);
        }

        [TestMethod]
        [DataRow(2, 0)]
        [DataRow(3, 0)]
        [DataRow(4, 0)]
        [DataRow(2, 1)]
        [DataRow(3, 1)]       
        public async Task CatalogsCanBeReadWithUsingPagination(int limit, int offset)
        {
            var response = await client.GetAsync($"{EndpointName}/?ownerId={TestUtils.OwnerId}&limit={limit}&offset={offset}");
            AssertExt.EnsureSuccessStatusCode(response);

            var catalogs = response.GetContent<OffsetPagedResults<CatalogOnListDTO>>().Value;
            var context = factory.GetService<TestCreationDbContext>();
            var expectedCatalogs = context.QuestionsCatalogs
                                                            .Where(x => x.OwnerId == TestUtils.OwnerId && x.IsDeleted == false)
                                                            .Skip(offset)
                                                            .Take(limit)
                                                            .ToList();

            AssertExt.AreEquivalent(expectedCatalogs, catalogs.Result);
        }

        [TestMethod]
        [DataRow(TestUtils.ValidQuestionsCatalog1Id)]
        [DataRow(TestUtils.ValidQuestionsCatalog2Id)]
        [DataRow(TestUtils.ValidQuestionsCatalog3Id)]
        public async Task CatalogCanBeRead(long catalogId)
        {
            var response = await client.GetAsync($"{EndpointName}/{catalogId}");
            AssertExt.EnsureSuccessStatusCode(response);

            var catalog = response.GetContent<CatalogDTO>().Value;
            var context = factory.GetService<TestCreationDbContext>();
            var expectedCatalog = context.QuestionsCatalogs.FirstOrDefault(x => x.CatalogId == catalogId);

            AssertExt.AreEquivalent(expectedCatalog, catalog);
        }

        [TestMethod]
        [DataRow("New Catalog 1")]
        [DataRow("New Catalog 2")]
        public async Task NewCatalogCanBeCreated(string catalogName)
        {           
            var body = new CreateCatalogDTO() { Name = catalogName, OwnerId = TestUtils.OwnerId };          

            var response = await client.PostAsync(EndpointName, body);
            AssertExt.EnsureSuccessStatusCode(response);

            var createdCatalogId = response.GetContent<long>().Value;
            var context = factory.GetService<TestCreationDbContext>();            
            var catalog = context.QuestionsCatalogs.Find(createdCatalogId);

            Assert.AreEqual(catalogName, catalog.Name);           
        }

        [TestMethod]
        [DataRow(TestUtils.ValidQuestionsCatalog1Id, "New Catalog name 1")]
        [DataRow(TestUtils.ValidQuestionsCatalog2Id, "New Catalog name 2")]
        [DataRow(TestUtils.ValidQuestionsCatalog3Id, "New Catalog name 3")]
        public async Task ExistingCatalogCanBeUpdated(long catalogId, string catalogName)
        {           
            var command = new UpdateCatalogDTO() { Name = catalogName };          

            var response = await client.PutAsync($"{EndpointName}/{catalogId}/", command);
            AssertExt.EnsureSuccessStatusCode(response);
           
            var context = factory.GetService<TestCreationDbContext>();
            var actualCatalog = context.QuestionsCatalogs.Find(catalogId);

            Assert.AreEqual(catalogName, actualCatalog.Name);
        }

        [TestMethod]
        [DataRow(TestUtils.ValidQuestionsCatalog1Id)]
        [DataRow(TestUtils.ValidQuestionsCatalog2Id)]
        [DataRow(TestUtils.ValidQuestionsCatalog3Id)]
        public async Task ExistingCatalogCanBeDeleted(long catalogId)
        { 
            var response = await client.DeleteAsync($"{EndpointName}/{catalogId}/");
            AssertExt.EnsureSuccessStatusCode(response);

            var context = factory.GetService<TestCreationDbContext>();
            var catalog = context.QuestionsCatalogs.IgnoreQueryFilters().Include(x => x.Questions).Where(x => x.CatalogId == catalogId).FirstOrDefault();

            Assert.AreEqual(true, catalog.IsDeleted);
            foreach (Question question in catalog.Questions)
            {
                Assert.AreEqual(true, question.IsDeleted);
            }            
        }

        [TestMethod]       
        [DataRow("Get", EndpointName + "/1")]
        [DataRow("Post", EndpointName)]
        [DataRow("Put", EndpointName + "/1")]
        [DataRow("Delete", EndpointName + "/1")]
        public async Task RequestWithInvalidTokenDoesNotHaveAccessToSecuredEndpoints(string httpMethod, string requestUri)
        {
            await base.CheckAuthorization(httpMethod, requestUri);
        }
    }
}
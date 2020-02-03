using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.App;
using TestMe.BuildingBlocks.Tests;
using TestMe.Presentation.API.Controllers.Tests.Input;
using TestMe.Presentation.API.Tests.Utils;
using TestMe.TestCreation;
using TestMe.TestCreation.App.Tests.Output;
using TestMe.TestCreation.Persistence;

namespace TestMe.Presentation.API.Tests
{
    [TestClass]
    public class TestsEndpoint : BaseFixture
    {
        private const string EndpointName = "Tests";
        private HttpClient client;


        [TestInitialize]
        public void TestInitialize()
        {
            factory = new ApiFactory(FakeDatabaseType.SQLiteInMemory, MockServices, SeedDatabase);
            client = factory.CreateClient(ValidOwnerToken);            
        }


        [TestMethod]
        [DataRow(TestUtils.ValidTestsCatalog1Id)]
        [DataRow(TestUtils.ValidTestsCatalog2Id)]      
        public async Task ReadTestHeaders_HappyPathIsSuccessful(long catalogId)
        {
            var response = await client.GetAsync($"{EndpointName}/headers?catalogId={catalogId}");
            AssertExt.EnsureSuccessStatusCode(response);

            var actualTests = response.GetContent<OffsetPagedResults<TestHeaderDTO>>().Value;
            var context = factory.GetService<TestCreationDbContext>();
            var expectedTests = context.Tests.Where(x => x.CatalogId == catalogId && x.IsDeleted == false).ToList();

            AssertExt.AreEquivalent(expectedTests, actualTests.Result);
        }

        [TestMethod]
        [DataRow(TestUtils.ValidTest1Id)]
        [DataRow(TestUtils.ValidTest2Id)]
        public async Task ReadTestWithQuestionItemsAndQuestionHeaders_HappyPathIsSuccessful(long testId)
        {
            var response = await client.GetAsync($"{EndpointName}/{testId}");
            AssertExt.EnsureSuccessStatusCode(response);

            var actualTest = response.GetContent<TestDTO>().Value;
            var context = factory.GetService<TestCreationDbContext>();
            var expectedTest = context.Tests.Include(x => x.Questions).ThenInclude(x => x.Question).FirstOrDefault(x => x.TestId == testId);

            AssertExt.AreEquivalent(expectedTest, actualTest);            
        }

        [TestMethod]
        [DataRow(TestUtils.ValidTestsCatalog1Id, "New Test A")]
        [DataRow(TestUtils.ValidTestsCatalog1Id, "New Test B")]
        [DataRow(TestUtils.ValidTestsCatalog2Id, "New Test C")]
        public async Task CreateTest_HappyPathIsSuccessful(long catalogId, string testTitle)
        {
            var command = new CreateTestDTO() { CatalogId = catalogId, Title = testTitle };

            var response = await client.PostAsync(EndpointName, command);
            AssertExt.EnsureSuccessStatusCode(response);

            var createdTestId = response.GetContent<long>().Value;    
            var context = factory.GetService<TestCreationDbContext>();          
            var actualTest = context.Tests.Find(createdTestId);

            AssertExt.AreEquivalent(command, actualTest);
        }
        
        [TestMethod]
        [DataRow(TestUtils.ValidTest1Id, TestUtils.ValidTestsCatalog1Id, "Updated Test A")]
        [DataRow(TestUtils.ValidTest2Id, TestUtils.ValidTestsCatalog2Id, "Updated Test B")]
        public async Task UpdateTest_HappyPathIsSuccessful(long testId, long catalogId, string title)
        {
            var command = new UpdateTestDTO
            {
                 CatalogId = catalogId,
                 Title = title
            };

            var response = await client.PutAsync($"{EndpointName}/{testId}/", command);
            AssertExt.EnsureSuccessStatusCode(response);
          
            var context = factory.GetService<TestCreationDbContext>();
            var actualTest = context.Tests.Find(testId);

            AssertExt.AreEquivalent(command, actualTest);
        }

        [TestMethod]
        [DataRow(TestUtils.ValidTest1Id)]
        [DataRow(TestUtils.ValidTest2Id)]
        public async Task DeleteTest_HappyPathIsSuccessful(long testId)
        {
            var response = await client.DeleteAsync($"{EndpointName}/{testId}/");
            AssertExt.EnsureSuccessStatusCode(response);

            var context = factory.GetService<TestCreationDbContext>();
            var test = context.Tests.IgnoreQueryFilters().FirstOrDefault(x => x.TestId == testId);
            Assert.AreEqual(true, test.IsDeleted);
        }

        [TestMethod]
        [DataRow(TestUtils.ValidTest1Id, TestUtils.ValidQuestion2Id)]
        [DataRow(TestUtils.ValidTest2Id, TestUtils.ValidQuestion1Id)]
        public async Task CreateQuestionItem_HappyPathIsSuccessful(long testId, long questionId)
        {
            var command = new CreateQuestionItemDTO() {  QuestionId = questionId };

            var response = await client.PostAsync($"{EndpointName}/{testId}/questions/", command);
            AssertExt.EnsureSuccessStatusCode(response);

            var addedItemId = response.GetContent<long>().Value; 
            var context = factory.GetService<TestCreationDbContext>();
            var test = context.Tests.Include(x => x.Questions).ThenInclude(x => x.Question).First(x => x.TestId == testId);
            var questionItem = test.Questions.FirstOrDefault(x => x.QuestionItemId == addedItemId);

            AssertExt.AreEquivalent(command, questionItem);
        }

        [TestMethod]
        [DataRow(TestUtils.ValidTest1Id, 1, TestUtils.ValidQuestion1Id)]
        [DataRow(TestUtils.ValidTest2Id, 5, TestUtils.ValidQuestion1Id)]
        public async Task UpdateQuestionItem_HappyPathIsSuccessful(long testId, long questionItemId, long validQuestionId)
        {
            var command = new UpdateQuestionItemDTO() 
            { 
                QuestionId = validQuestionId
            };

            var response = await client.PutAsync($"{EndpointName}/{testId}/questions/{questionItemId}", command);
            AssertExt.EnsureSuccessStatusCode(response);
           
            var context = factory.GetService<TestCreationDbContext>();
            var test = context.Tests.Include(x => x.Questions).ThenInclude(x => x.Question).First(x => x.TestId == testId);
            var questionItem = test.Questions.FirstOrDefault(x => x.QuestionItemId == questionItemId);

            // There is no much to assert right now
            //AssertExt.AreEquivalent(command, questionItem);
        }

        [TestMethod]
        [DataRow(TestUtils.ValidTest1Id, 1)]
        [DataRow(TestUtils.ValidTest2Id, 5)]
        public async Task DeleteQuestionItem_HappyPathIsSuccessful(long testId, long questionItemId)
        {
            var response = await client.DeleteAsync($"{EndpointName}/{testId}/questions/{questionItemId}"); 
            AssertExt.EnsureSuccessStatusCode(response);

            var context = factory.GetService<TestCreationDbContext>();
            var test = context.Tests.Include(x => x.Questions).ThenInclude(x => x.Question).First(x => x.TestId == testId);
            var questionItem = test.Questions.FirstOrDefault(x => x.QuestionItemId == questionItemId);

            Assert.AreEqual(null, questionItem);
        }

        [TestMethod]
        [DataRow("Get", EndpointName + "/headers?catalogId=5")]
        [DataRow("Get", EndpointName + "/1")]
        [DataRow("Post", EndpointName)]
        [DataRow("Put", EndpointName + "/1")]
        [DataRow("Delete", EndpointName + "/1")]
        [DataRow("Post", EndpointName + "/1/questions/")]
        [DataRow("Put", EndpointName + "/1/questions/1")]
        [DataRow("Delete", EndpointName + "/1/questions/1")]
        public async Task RequestWithInvalidTokenDoesNotHaveAccessToSecuredEndpoints(string httpMethod, string requestUri)
        {
            await base.CheckAuthorization(httpMethod, requestUri);
        }
    }
}

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.Presentation.API.Tests.Utils;
using TestMe.TestCreation.App.Tests.Input;
using TestMe.TestCreation.App.Tests.Output;
using TestMe.TestCreation.Persistence;

namespace TestMe.Presentation.API.Tests
{
    [TestClass]
    public class Tests_HappyPath : BaseFixture
    {
        private const string EndpointName = "Tests";
        private HttpClient client;


        [TestInitialize]
        public void TestInitialize()
        {
            factory = new ApiFactory(ApiFactory.DatabaseType.SQLiteInMemory);           
            client = factory.CreateClient(Token);           
        }


        [TestMethod]
        [DataRow(5)]
        [DataRow(6)]      
        public async Task ReadTestHeaders(long catalogId)
        {
            var response = await client.GetAsync($"{EndpointName}/headers?catalogId={catalogId}");

            var actualTests = response.GetContent<TestHeaderDTO[]>().Value;
            var context = factory.GetContext<TestCreationDbContext>();
            var expectedTests = context.Tests.Where(x => x.CatalogId == catalogId && x.IsDeleted == false).ToList();

            AssertExt.AreEquivalent(expectedTests, actualTests);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public async Task ReadTestWithQuestionItemsAndQuestionHeaders(long testId)
        {
            var response = await client.GetAsync($"{EndpointName}/{testId}");

            var actualTest = response.GetContent<TestDTO>().Value;
            var context = factory.GetContext<TestCreationDbContext>();
            var expectedTest = context.Tests.Include(x => x.Questions).ThenInclude(x => x.Question).FirstOrDefault(x => x.TestId == testId);

            AssertExt.AreEquivalent(expectedTest, actualTest);            
        }

        [TestMethod]
        [DataRow(5, "New Test A")]
        [DataRow(5, "New Test B")]
        [DataRow(6, "New Test C")]
        public async Task CreateTest(long catalogId, string testTitle)
        {
            var command = new CreateTest() { CatalogId = catalogId, Title = testTitle };

            var response = await client.PostAsync(EndpointName, command);

            var createdTestId = response.GetContent<long>().Value;    
            var context = factory.GetContext<TestCreationDbContext>();          
            var actualTest = context.Tests.Find(createdTestId);

            AssertExt.AreEquivalent(command, actualTest);
        }
        
        [TestMethod]
        [DataRow(1, 5, "New Test A")]
        [DataRow(2, 6, "New Test A")]
        public async Task UpdateTest(long testId, long catalogId, string title)
        {
            var command = new UpdateTest
            {
                 CatalogId = catalogId,
                 Title = title
            };

            var response = await client.PutAsync($"{EndpointName}/{testId}/", command);
           
            response.EnsureSuccessStatusCode();
            var context = factory.GetContext<TestCreationDbContext>();
            var actualTest = context.Tests.Find(testId);

            AssertExt.AreEquivalent(command, actualTest);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public async Task DeleteTest(long testId)
        {
            var response = await client.DeleteAsync($"{EndpointName}/{testId}/");
          
            response.EnsureSuccessStatusCode();
            var context = factory.GetContext<TestCreationDbContext>();
            var test = context.Tests.IgnoreQueryFilters().FirstOrDefault(x => x.TestId == testId);
            Assert.AreEqual(true, test.IsDeleted);
        }

        [TestMethod]
        [DataRow(1, 2)]
        [DataRow(2, 1)]
        public async Task CreateQuestionItem(long testId, long questionId)
        {
            var command = new AddQuestionItem() {  QuestionId = questionId };

            var response = await client.PostAsync($"{EndpointName}/{testId}/questions/", command);

            var addedItemId = response.GetContent<long>().Value; 
            var context = factory.GetContext<TestCreationDbContext>();
            var test = context.Tests.Include(x => x.Questions).ThenInclude(x => x.Question).First(x => x.TestId == testId);
            var questionItem = test.Questions.FirstOrDefault(x => x.QuestionItemId == addedItemId);

            AssertExt.AreEquivalent(command, questionItem);
        }

        [TestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 5)]
        public async Task UpdateQuestionItem(long testId, long questionItemId)
        {
            var command = new UpdateQuestionItem() { };

            var response = await client.PutAsync($"{EndpointName}/{testId}/questions/{questionItemId}", command);

            response.EnsureSuccessStatusCode();
            var context = factory.GetContext<TestCreationDbContext>();
            var test = context.Tests.Include(x => x.Questions).ThenInclude(x => x.Question).First(x => x.TestId == testId);
            var questionItem = test.Questions.FirstOrDefault(x => x.QuestionItemId == questionItemId);

            // There is no much to assert right now
            //AssertExt.AreEquivalent(command, questionItem);
        }

        [TestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 5)]
        public async Task DeleteQuestionItem(long testId, long questionItemId)
        {
            var response = await client.DeleteAsync($"{EndpointName}/{testId}/questions/{questionItemId}");
            
            response.EnsureSuccessStatusCode();

            var context = factory.GetContext<TestCreationDbContext>();
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
        public new async Task CheckAuthorization(string httpMethod, string requestUri)
        {
            await base.CheckAuthorization(httpMethod, requestUri);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.Tests;
using TestMe.Presentation.API.Controllers.Questions.Input;
using TestMe.Presentation.API.Tests.Utils;
using TestMe.TestCreation;
using TestMe.TestCreation.App.Questions.Output;
using TestMe.TestCreation.Persistence;

namespace TestMe.Presentation.API.Tests
{
    [TestClass]
    public class QuestionsEndpoint : BaseFixture
    {
        private const string EndpointName = "Questions";
        private HttpClient client;


        [TestInitialize]
        public void TestInitialize()
        {
            factory = new ApiFactory(FakeDatabaseType.SQLiteInMemory, MockServices, SeedDatabase);
            client = factory.CreateClient(ValidOwnerToken);                   
        }


        [TestMethod]
        [DataRow(TestUtils.ValidQuestionsCatalog1Id)]
        [DataRow(TestUtils.ValidQuestionsCatalog2Id)]
        [DataRow(TestUtils.ValidQuestionsCatalog3Id)]
        public async Task ReadQuestionHeaders_HappyPathIsSuccessful(long catalogId)
        { 
            var response = await client.GetAsync($"{EndpointName}/headers?catalogId={catalogId}");
            AssertExt.EnsureSuccessStatusCode(response);

            var actualQuestions = response.GetContent<QuestionHeaderDTO[]>().Value;
            var context = factory.GetService<TestCreationDbContext>();
            var expectedQuestions = context.Questions.Where(x => x.CatalogId == catalogId && x.IsDeleted == false).ToList();

            AssertExt.AreEquivalent(expectedQuestions, actualQuestions);
        }

        [TestMethod]
        [DataRow(TestUtils.ValidQuestion1Id)]
        [DataRow(TestUtils.ValidQuestion2Id)]
        [DataRow(TestUtils.ValidQuestion3Id)]
        public async Task ReadQuestionHeader_HappyPathIsSuccessful(long questionId)
        {
            var response = await client.GetAsync($"{EndpointName}/{questionId}/header");
            AssertExt.EnsureSuccessStatusCode(response);

            var actualQuestion = response.GetContent<QuestionHeaderDTO>().Value;
            var context = factory.GetService<TestCreationDbContext>();
            var expectedQuestion = context.Questions.Where(x => x.QuestionId == questionId).First();

            AssertExt.AreEquivalent(expectedQuestion, actualQuestion);
        }


        [TestMethod]
        [DataRow(TestUtils.ValidQuestion1Id)]
        [DataRow(TestUtils.ValidQuestion2Id)]
        [DataRow(TestUtils.ValidQuestion3Id)]
        public async Task ReadQuestionWithAnswers_HappyPathIsSuccessful(long questionId)
        {
            var response = await client.GetAsync($"{EndpointName}/{questionId}/");
            AssertExt.EnsureSuccessStatusCode(response);

            var actualQuestion = response.GetContent<QuestionDTO>().Value;
            var context = factory.GetService<TestCreationDbContext>();
            var expectedQuestion = context.Questions.Include(x => x.Answers).FirstOrDefault(x => x.QuestionId == questionId);

            AssertExt.AreEquivalent(expectedQuestion, actualQuestion);
        }
       
        [TestMethod]
        public async Task CreateQuestionWithAnswers_HappyPathIsSuccessful()
        {          
            var command = new CreateQuestionDTO()
            {
                CatalogId = TestUtils.ValidQuestionsCatalog1Id,
                Content = "Who is your dady?",
                Answers = new List<CreateAnswerDTO>()
                {
                    new CreateAnswerDTO() { Content = "Adam", IsCorrect = true },
                    new CreateAnswerDTO() { Content = "Peter", IsCorrect = false}
                }
            };

            var response = await client.PostAsync(EndpointName, command);
            AssertExt.EnsureSuccessStatusCode(response);

            var createdId = response.GetContent<long>().Value;
            var context = factory.GetService<TestCreationDbContext>();            
            var actualQuestion = context.Questions.Include(x => x.Answers).FirstOrDefault(x => x.QuestionId == createdId);

            AssertExt.AreEquivalent(command, actualQuestion);
        }

       
        [TestMethod]
        [DataRow(TestUtils.ValidQuestion1Id, 1)]
        [DataRow(TestUtils.ValidQuestion2Id, 2)]
        [DataRow(TestUtils.ValidQuestion3Id, 4)]
        public async Task UpdateQuestionWithAnswers_HappyPathIsSuccessful(long questionId, long answerId)
        {
            var command = new UpdateQuestionDTO
            {
                Content = "Who is your momy?",
                Answers = new List<UpdateAnswerDTO>
                {
                    new UpdateAnswerDTO { AnswerId = answerId, Content = "Alicia", IsCorrect = false }
                },
                CatalogId = TestUtils.ValidQuestionsCatalog2Id,
                ConcurrencyToken = 0
            };

            var response = await client.PutAsync($"{EndpointName}/{questionId}/", command);
            AssertExt.EnsureSuccessStatusCode(response);

            var context = factory.GetService<TestCreationDbContext>();
            var actualQuestion = context.Questions.Include(x => x.Answers).FirstOrDefault(x => x.QuestionId == questionId);

            AssertExt.AreEquivalent(command, actualQuestion);
        }

        [TestMethod]
        [DataRow(TestUtils.ValidQuestion1Id)]
        [DataRow(TestUtils.ValidQuestion2Id)]
        [DataRow(TestUtils.ValidQuestion3Id)]
        public async Task DeleteQuestionWithAnswers_HappyPathIsSuccessful(long id)
        {
            var response = await client.DeleteAsync($"{EndpointName}/{id}/");
            AssertExt.EnsureSuccessStatusCode(response);

            var context = factory.GetService<TestCreationDbContext>();
            var question = context.Questions.IgnoreQueryFilters().Where(x => x.QuestionId == id).FirstOrDefault();

            Assert.AreEqual(true, question.IsDeleted);            
        }

        [TestMethod]
        [DataRow("Get", EndpointName + "/headers?catalogId=1")]
        [DataRow("Get", EndpointName + "/1/header")]
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

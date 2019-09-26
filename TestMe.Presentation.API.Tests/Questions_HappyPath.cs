using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.Presentation.API.Tests.Utils;
using TestMe.TestCreation.App.Questions.Input;
using TestMe.TestCreation.App.Questions.Output;
using TestMe.TestCreation.Persistence;

namespace TestMe.Presentation.API.Tests
{
    [TestClass]
    public class Questions_HappyPath : BaseFixture
    {
        private const string EndpointName = "Questions";
        private HttpClient client;


        [TestInitialize]
        public void TestInitialize()
        {
            factory = new ApiFactory(ApiFactory.DatabaseType.SQLiteInMemory, SeedDatabase);
            client = factory.CreateClient(ValidToken);                   
        }


        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task ReadQuestionHeaders(long catalogId)
        { 
            var response = await client.GetAsync($"{EndpointName}/headers?catalogId={catalogId}");
           
            var actualQuestions = response.GetContent<QuestionHeaderDTO[]>().Value;
            var context = factory.GetContext<TestCreationDbContext>();
            var expectedQuestions = context.Questions.Where(x => x.CatalogId == catalogId && x.IsDeleted == false).ToList();

            AssertExt.AreEquivalent(expectedQuestions, actualQuestions);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task ReadQuestionHeader(long questionId)
        {
            var response = await client.GetAsync($"{EndpointName}/{questionId}/header");

            var actualQuestion = response.GetContent<QuestionHeaderDTO>().Value;
            var context = factory.GetContext<TestCreationDbContext>();
            var expectedQuestion = context.Questions.Where(x => x.QuestionId == questionId).First();

            AssertExt.AreEquivalent(expectedQuestion, actualQuestion);
        }


        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task ReadQuestionWithAnswers(long questionId)
        {
            var response = await client.GetAsync($"{EndpointName}/{questionId}/");
          
            var actualQuestion = response.GetContent<QuestionDTO>().Value;
            var context = factory.GetContext<TestCreationDbContext>();
            var expectedQuestion = context.Questions.Include(x => x.Answers).FirstOrDefault(x => x.QuestionId == questionId);

            AssertExt.AreEquivalent(expectedQuestion, actualQuestion);
        }
       
        [TestMethod]
        public async Task CreateQuestionWithAnswers()
        {          
            var command = new CreateQuestion()
            {
                CatalogId = 1,
                Content = "Who is your dady?",
                Answers = new List<CreateAnswer>()
                {
                    new CreateAnswer() { Content = "Adam", IsCorrect = true },
                    new CreateAnswer() { Content = "Peter", IsCorrect = false}
                }
            };

            var response = await client.PostAsync(EndpointName, command);          
          
            var createdId = response.GetContent<long>().Value;
            var context = factory.GetContext<TestCreationDbContext>();            
            var actualQuestion = context.Questions.Include(x => x.Answers).FirstOrDefault(x => x.QuestionId == createdId);

            AssertExt.AreEquivalent(command, actualQuestion);
        }

        /*
         * This test stopped working after migration to ef core 3.0 due to problem with tracking change of principal entity id 
         */
        [TestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 2)]
        [DataRow(3, 4)]
        public async Task UpdateQuestionWithAnswers(long questionId, long answerId)
        {
            var command = new UpdateQuestion
            {
                Content = "Who is your momy?",
                Answers = new List<UpdateAnswer>
                {
                    new UpdateAnswer { AnswerId = answerId, Content = "Alicia", IsCorrect = false }
                },
                CatalogId = 2,
                ConcurrencyToken = 0
            };

            var response = await client.PutAsync($"{EndpointName}/{questionId}/", command);

            response.EnsureSuccessStatusCode();
            var context = factory.GetContext<TestCreationDbContext>();
            var actualQuestion = context.Questions.Include(x => x.Answers).FirstOrDefault(x => x.QuestionId == questionId);

            AssertExt.AreEquivalent(command, actualQuestion);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public async Task DeleteQuestionWithAnswers(long id)
        {
            var response = await client.DeleteAsync($"{EndpointName}/{id}/");

            response.EnsureSuccessStatusCode();
            var context = factory.GetContext<TestCreationDbContext>();
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
        public new async Task CheckAuthorization(string httpMethod, string requestUri)
        {
            await base.CheckAuthorization(httpMethod, requestUri);
        }
    }
}

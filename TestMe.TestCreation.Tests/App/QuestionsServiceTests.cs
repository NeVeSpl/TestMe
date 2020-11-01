using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.App;
using TestMe.BuildingBlocks.Tests;
using TestMe.TestCreation.App.RequestHandlers.Questions.CreateQuestion;
using TestMe.TestCreation.App.RequestHandlers.Questions.DeleteQuestion;
using TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestion;
using TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestions;
using TestMe.TestCreation.App.RequestHandlers.Questions.UpdateQuestion;
using TestMe.TestCreation.Persistence;
using static TestMe.TestCreation.TestUtils;

namespace TestMe.TestCreation.Tests.App
{
    [TestClass]
    public class QuestionsServiceTests : BaseFixture
    {
        private TestCreationDbContext testCreationDbContext;
        private ReadOnlyTestCreationDbContext readOnlyTestCreationDbContext;      
        private TestCreation.Domain.ITestCreationUoW uow;

        private protected override FakeDatabaseType GetDatabaseType()
        {
            return FakeDatabaseType.SQLiteInMemory;
        }

        [TestInitialize]
        public void TestInitialize()
        {           
            testCreationDbContext = CreateTestCreationDbContext();
            readOnlyTestCreationDbContext = CreateReadOnlyTestCreationDbContext();
            uow = TestUtils.CreateTestCreationUoW(testCreationDbContext);
           
        }   
        [TestCleanup]
        public void TestCleanup()
        {
            testCreationDbContext.Dispose();
        }


        [TestMethod]
        [DataRow(ValidQuestionsCatalog2Id, ResultStatus.Ok)]
        [DataRow(DeletedQuestionsCatalogId, ResultStatus.NotFound)]
        [DataRow(ValidTestsCatalog1Id, ResultStatus.NotFound)]
        [DataRow(NotExisitngQuestionsCatalogId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerQuestionsCatalogId, ResultStatus.Unauthorized)]
        public async Task ReadQuestionHeaders(long catalogId, ResultStatus expectedResult)
        {
            var handler = new ReadQuestionsHandler(readOnlyTestCreationDbContext);
            var query = new ReadQuestionsQuery(catalogId, new OffsetPagination())
            {
                UserId = OwnerId,
            };
            var result = await handler.Handle(query, default);
            Assert.AreEqual(expectedResult, result.Status);
        }       

        [TestMethod]
        [DataRow(ValidQuestion1Id, ResultStatus.Ok)]       
        [DataRow(NotExisitngQuestionId, ResultStatus.NotFound)]
        [DataRow(DeletedQuestionId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerQuestionId, ResultStatus.Unauthorized)]
        public async Task ReadQuestionWithAnswers(long questionId, ResultStatus expectedResult)
        {
            var handler = new ReadQuestionWithAnswersHandler(readOnlyTestCreationDbContext);
            var query = new ReadQuestionWithAnswersQuery(questionId)
            {               
                UserId = OwnerId,                
            };
            var result = await handler.Handle(query, default);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]      
        [DataRow(ValidQuestionsCatalog2Id, ResultStatus.Ok)]
        [DataRow(DeletedQuestionsCatalogId, ResultStatus.Error)]
        [DataRow(ValidTestsCatalog1Id, ResultStatus.Error)]
        [DataRow(NotExisitngQuestionsCatalogId, ResultStatus.Error)]
        [DataRow(OtherOwnerQuestionsCatalogId, ResultStatus.Unauthorized)]
        public async Task CreateQuestionWithAnswers(long catalogId, ResultStatus expectedResult)
        {
            var handler = new CreateQuestionWithAnswersHandler(uow);
            var command = new CreateQuestionWithAnswersCommand()
            {
                UserId = OwnerId,
                Content = "Dani Carvajal",
                CatalogId = catalogId,
            };
            var result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }
        
        [TestMethod]
        [DataRow(ValidQuestion1Id, ResultStatus.Ok)]
        [DataRow(DeletedQuestionId, ResultStatus.NotFound)]
        [DataRow(NotExisitngQuestionId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerQuestionId,   ResultStatus.Unauthorized)]     
        public async Task UpdateQuestionWithAnswers(long questionId, ResultStatus expectedResult)
        {
            var handler = new UpdateQuestionWithAnswersHandler(uow);
            var command = new UpdateQuestionWithAnswersCommand()
            {
                UserId = OwnerId,
                QuestionId = questionId,
                Content = "Dani Carvajal",
                CatalogId = ValidQuestionsCatalog2Id,
                ConcurrencyToken = 0,
            };
            Result result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]      
        public async Task UpdateQuestionWithAnswers_GivenWrongConcurrencyToken_ShouldReturnResultConflict()
        {
            var handler = new UpdateQuestionWithAnswersHandler(uow);
            var command = new UpdateQuestionWithAnswersCommand()
            {
                UserId = OwnerId,
                QuestionId = ValidQuestion1Id,
                Content = "Dani Carvajal",
                CatalogId = ValidQuestionsCatalog2Id,
                ConcurrencyToken = 666,
            };
            Result result = await handler.Handle(command, default);
            Assert.AreEqual(ResultStatus.Conflict, result.Status);
        }

        [TestMethod]
        [DataRow(ValidQuestion1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngQuestionId, ResultStatus.NotFound)]
        [DataRow(DeletedQuestionId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerQuestionId, ResultStatus.Unauthorized)]
        public async Task DeleteQuestionWithAnswers(long questionId, ResultStatus expectedResult)
        {
            var handler = new DeleteQuestionWithAnswersHandler(uow);
            var command = new DeleteQuestionWithAnswersCommand()
            {
                UserId = OwnerId,
                QuestionId = questionId,
            };
            Result result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }
    }
}
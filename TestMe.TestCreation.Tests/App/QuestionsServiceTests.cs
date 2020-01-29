using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.App;
using TestMe.BuildingBlocks.Tests;
using TestMe.TestCreation.App.Questions;
using TestMe.TestCreation.App.Questions.Input;
using TestMe.TestCreation.Persistence;
using static TestMe.TestCreation.TestUtils;

namespace TestMe.TestCreation.Tests.App
{
    [TestClass]
    public class QuestionsServiceTests : BaseFixture
    {
        private TestCreationDbContext testCreationDbContext;
        private QuestionsService serviceUnderTest;

        private protected override FakeDatabaseType GetDatabaseType()
        {
            return FakeDatabaseType.SQLiteInMemory;
        }

        [TestInitialize]
        public void TestInitialize()
        {           
            testCreationDbContext = CreateTestCreationDbContext();
            var uow = TestUtils.CreateTestCreationUoW(testCreationDbContext);
            serviceUnderTest = new QuestionsService(new QuestionReader(CreateReadOnlyTestCreationDbContext()), uow);
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
        public void ReadQuestionHeaders(long catalogId, ResultStatus expectedResult)
        {
            Result result = serviceUnderTest.ReadQuestionHeaders(OwnerId, catalogId);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidQuestion1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngQuestionId, ResultStatus.NotFound)]
        [DataRow(DeletedQuestionId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerQuestionId, ResultStatus.Unauthorized)]
        public void ReadQuestionHeader(long questionId, ResultStatus expectedResult)
        {
            Result result = serviceUnderTest.ReadQuestionHeader(OwnerId, questionId);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidQuestion1Id, ResultStatus.Ok)]       
        [DataRow(NotExisitngQuestionId, ResultStatus.NotFound)]
        [DataRow(DeletedQuestionId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerQuestionId, ResultStatus.Unauthorized)]
        public void ReadQuestionWithAnswers(long questionId, ResultStatus expectedResult)
        {
            Result result = serviceUnderTest.ReadQuestionWithAnswers(OwnerId, questionId);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]      
        [DataRow(ValidQuestionsCatalog2Id, ResultStatus.Ok)]
        [DataRow(DeletedQuestionsCatalogId, ResultStatus.Error)]
        [DataRow(ValidTestsCatalog1Id, ResultStatus.Error)]
        [DataRow(NotExisitngQuestionsCatalogId, ResultStatus.Error)]
        [DataRow(OtherOwnerQuestionsCatalogId, ResultStatus.Unauthorized)]
        public void CreateQuestionWithAnswers(long catalogId, ResultStatus expectedResult)
        {
            var command = new CreateQuestion()
            {
                UserId = OwnerId,
                Content = "Dani Carvajal",
                CatalogId = catalogId,
            };
            Result result = serviceUnderTest.CreateQuestionWithAnswers(command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        
        [TestMethod]
        [DataRow(ValidQuestion1Id, ResultStatus.Ok)]
        [DataRow(DeletedQuestionId, ResultStatus.NotFound)]
        [DataRow(NotExisitngQuestionId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerQuestionId,   ResultStatus.Unauthorized)]     
        public void UpdateQuestionWithAnswers(long questionId, ResultStatus expectedResult)
        {
            var command = new UpdateQuestion()
            {
                UserId = OwnerId,
                QuestionId = questionId,
                Content = "Dani Carvajal",
                CatalogId = ValidQuestionsCatalog2Id,
                ConcurrencyToken = 0,
            };
            Result result = serviceUnderTest.UpdateQuestionWithAnswers(command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]      
        public void UpdateQuestionWithAnswers_GivenWrongConcurrencyToken_ShouldReturnResultConflict()
        {
            var command = new UpdateQuestion()
            {
                UserId = OwnerId,
                QuestionId = ValidQuestion1Id,
                Content = "Dani Carvajal",
                CatalogId = ValidQuestionsCatalog2Id,
                ConcurrencyToken = 666,
            };
            Result result = serviceUnderTest.UpdateQuestionWithAnswers(command);
            Assert.AreEqual(ResultStatus.Conflict, result.Status);
        }

        [TestMethod]
        [DataRow(ValidQuestion1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngQuestionId, ResultStatus.NotFound)]
        [DataRow(DeletedQuestionId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerQuestionId, ResultStatus.Unauthorized)]
        public void DeleteQuestionWithAnswers(long questionId, ResultStatus expectedResult)
        {
            var command = new DeleteQuestion()
            {
                UserId = OwnerId,
                QuestionId = questionId,
            };
            Result result = serviceUnderTest.DeleteQuestionWithAnswers(command);
            Assert.AreEqual(expectedResult, result.Status);
        }
    }
}

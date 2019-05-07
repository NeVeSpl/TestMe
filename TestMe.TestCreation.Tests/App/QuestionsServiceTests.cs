using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Questions;
using TestMe.TestCreation.App.Questions.Input;
using TestMe.TestCreation.Persistence;
using static TestMe.TestCreation.TestUtils;

namespace TestMe.TestCreation.Tests.App
{
    [TestClass]
    public class QuestionsServiceTests : BaseFixture
    {
        private TestCreationDbContext TestCreationDbContext;
        private QuestionsService serviceUnderTest;

        private protected override DatabaseType GetDatabaseType()
        {
            return DatabaseType.SQLiteInMemory;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            using (var context = CreateTestCreationDbContext())
            {
                TestUtils.Seed(context);
            }
            TestCreationDbContext = CreateTestCreationDbContext();
            var uow = TestUtils.CreateTestCreationUoW(TestCreationDbContext);
            serviceUnderTest = new QuestionsService(new QuestionReader(TestCreationDbContext), uow);
        }
        [TestCleanup]
        public void TestCleanup()
        {
            TestCreationDbContext.Dispose();
        }


        [TestMethod]
        [DataRow(ValidQuestionsCatalogId, ResultStatus.Ok)]
        [DataRow(DeletedQuestionsCatalogId, ResultStatus.NotFound)]
        [DataRow(ValidTestsCatalogId, ResultStatus.NotFound)]
        [DataRow(NotExisitngQuestionsCatalogId, ResultStatus.NotFound)]
        [DataRow(OtherUserQuestionsCatalogId, ResultStatus.Unauthorized)]
        public void ReadQuestionHeaders(long catalogId, ResultStatus expectedResult)
        {
            Result result = serviceUnderTest.ReadQuestionHeaders(OwnerId, catalogId);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidQuestionId, ResultStatus.Ok)]
        [DataRow(NotExisitngQuestionId, ResultStatus.NotFound)]
        [DataRow(DeletedQuestionId, ResultStatus.NotFound)]
        [DataRow(OtherUserQuestionId, ResultStatus.Unauthorized)]
        public void ReadQuestionHeader(long questionId, ResultStatus expectedResult)
        {
            Result result = serviceUnderTest.ReadQuestionHeader(OwnerId, questionId);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidQuestionId, ResultStatus.Ok)]       
        [DataRow(NotExisitngQuestionId, ResultStatus.NotFound)]
        [DataRow(DeletedQuestionId, ResultStatus.NotFound)]
        [DataRow(OtherUserQuestionId, ResultStatus.Unauthorized)]
        public void ReadQuestionWithAnswers(long questionId, ResultStatus expectedResult)
        {
            Result result = serviceUnderTest.ReadQuestionWithAnswers(OwnerId, questionId);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]      
        [DataRow(ValidQuestionsCatalogId, ResultStatus.Ok)]
        [DataRow(DeletedQuestionsCatalogId, ResultStatus.Error)]
        [DataRow(ValidTestsCatalogId, ResultStatus.Error)]
        [DataRow(NotExisitngQuestionsCatalogId, ResultStatus.Error)]
        [DataRow(OtherUserQuestionsCatalogId, ResultStatus.Unauthorized)]
        public void CreateQuestionWithAnswers(long catalogId, ResultStatus expectedResult)
        {
            var command = new CreateQuestion()
            {
                Content = "Dani Carvajal",
                CatalogId = catalogId,
            };
            Result result = serviceUnderTest.CreateQuestionWithAnswers(OwnerId, command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidQuestionId, ResultStatus.Ok)]
        [DataRow(DeletedQuestionId, ResultStatus.NotFound)]
        [DataRow(NotExisitngQuestionId, ResultStatus.NotFound)]
        [DataRow(OtherUserQuestionId,   ResultStatus.Unauthorized)]     
        public void UpdateQuestionWithAnswers(long questionId, ResultStatus expectedResult)
        {
            var command = new UpdateQuestion()
            {
                Content = "Dani Carvajal",
                CatalogId = ValidQuestionsCatalogId,
                ConcurrencyToken = 0,
            };
            Result result = serviceUnderTest.UpdateQuestionWithAnswers(OwnerId, questionId, command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]      
        public void UpdateQuestionWithAnswers_GivenWrongConcurrencyToken_ShouldReturnResultConflict()
        {
            var command = new UpdateQuestion()
            {
                Content = "Dani Carvajal",
                CatalogId = 1,
                ConcurrencyToken = 666,
            };
            Result result = serviceUnderTest.UpdateQuestionWithAnswers(1, 1, command);
            Assert.AreEqual(ResultStatus.Conflict, result.Status);
        }

        [TestMethod]
        [DataRow(ValidQuestionId, ResultStatus.Ok)]
        [DataRow(NotExisitngQuestionId, ResultStatus.NotFound)]
        [DataRow(DeletedQuestionId, ResultStatus.NotFound)]
        [DataRow(OtherUserQuestionId, ResultStatus.Unauthorized)]
        public void DeleteQuestionWithAnswers(long questionId, ResultStatus expectedResult)
        {
            Result result = serviceUnderTest.DeleteQuestionWithAnswers(OwnerId, questionId);
            Assert.AreEqual(expectedResult, result.Status);
        }
    }
}

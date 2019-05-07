using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Tests;
using TestMe.TestCreation.App.Tests.Input;
using TestMe.TestCreation.Persistence;
using static TestMe.TestCreation.TestUtils;

namespace TestMe.TestCreation.Tests.App
{
    [TestClass]
    public class TestsServiceTests : BaseFixture
    {
        private TestCreationDbContext TestCreationDbContext;
        private TestsService serviceUnderTest;

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
            serviceUnderTest = new TestsService(new TestReader(TestCreationDbContext), uow);
        }
        [TestCleanup]
        public void TestCleanup()
        {
            TestCreationDbContext.Dispose();
        }


        [TestMethod]
        [DataRow(ValidTestsCatalogId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestsCatalogId, ResultStatus.NotFound)]
        [DataRow(ValidQuestionsCatalogId, ResultStatus.NotFound)]
        [DataRow(DeletedTestsCatalogId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestsCatalogId, ResultStatus.Unauthorized)]
        public void ReadTestHeaders(long catalogId, ResultStatus expectedResult)
        {
            Result result = serviceUnderTest.ReadTestHeaders(OwnerId, catalogId);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherUserTestId, ResultStatus.Unauthorized)]
        public void ReadTestWithQuestionItemsAndQuestionHeaders(long testId, ResultStatus expectedResult)
        {
            Result result = serviceUnderTest.ReadTestWithQuestionItemsAndQuestionHeaders(OwnerId, testId);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestsCatalogId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestsCatalogId, ResultStatus.Error)]
        [DataRow(ValidQuestionsCatalogId, ResultStatus.Error)]
        [DataRow(DeletedTestsCatalogId, ResultStatus.Error)]
        [DataRow(OtherOwnerTestsCatalogId, ResultStatus.Unauthorized)]
        public void CreateTest(long catalogId, ResultStatus expectedResult)
        {
            var command = new CreateTest()
            {
                Title = "Roberto Carlos",
                CatalogId = catalogId,
            };
            Result result = serviceUnderTest.CreateTest(OwnerId, command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestId, ResultStatus.Ok)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(OtherUserTestId, ResultStatus.Unauthorized)]
        public void UpdateTest(long testId, ResultStatus expectedResult)
        {
            var command = new UpdateTest()
            {
                Title = "Roberto Carlos",
                CatalogId = ValidTestsCatalogId,
            };
            Result result = serviceUnderTest.UpdateTest(OwnerId, testId, command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherUserTestId, ResultStatus.Unauthorized)]
        public void DeleteTest(long testId, ResultStatus expectedResult)
        {
            Result result = serviceUnderTest.DeleteTest(OwnerId, testId);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherUserTestId, ResultStatus.Unauthorized)]      
        public void CreateQuestionItem_WhenGivenTestId(long testId, ResultStatus expectedResult)
        {
            var command = new AddQuestionItem()
            {                
                QuestionId = ValidQuestionId,
            };
            Result result = serviceUnderTest.CreateQuestionItem(OwnerId, testId , command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidQuestionId, ResultStatus.Ok)]
        [DataRow(NotExisitngQuestionId, ResultStatus.Error)]
        [DataRow(DeletedQuestionId, ResultStatus.Error)]
        [DataRow(OtherUserQuestionId, ResultStatus.Unauthorized)]
        public void CreateQuestionItem_WhenGivenQuestionId(long questionId, ResultStatus expectedResult)
        {
            var command = new AddQuestionItem()
            {
                QuestionId = questionId,
            };
            Result result = serviceUnderTest.CreateQuestionItem(OwnerId, ValidTestId, command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherUserTestId, ResultStatus.Unauthorized)]
        public void UpdateQuestionItem_WhenGivenTestId(long testId, ResultStatus expectedResult)
        {
            var command = new UpdateQuestionItem()
            {
               
            };
            Result result = serviceUnderTest.UpdateQuestionItem(OwnerId, testId, ValidTestItemId, command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestItemId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestItemId, ResultStatus.NotFound)]
        [DataRow(OtherTestItemId, ResultStatus.NotFound)]      
        public void UpdateQuestionItem_WhenGivenQuestionItemId(long questionItemId, ResultStatus expectedResult)
        {
            var command = new UpdateQuestionItem()
            {
                
            };
            Result result = serviceUnderTest.UpdateQuestionItem(OwnerId, ValidTestId, questionItemId, command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherUserTestId, ResultStatus.Unauthorized)]
        public void DeleteQuestionItem_WhenGivenTestId(long testId, ResultStatus expectedResult)
        {            
            Result result = serviceUnderTest.DeleteQuestionItem(OwnerId, testId, ValidTestItemId);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestItemId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestItemId, ResultStatus.NotFound)]
        [DataRow(OtherTestItemId, ResultStatus.NotFound)]
        public void DeleteQuestionItem_WhenGivenQuestionItemId(long questionItemId, ResultStatus expectedResult)
        {           
            Result result = serviceUnderTest.DeleteQuestionItem(OwnerId, ValidTestId, questionItemId);
            Assert.AreEqual(expectedResult, result.Status);
        }
    }
}

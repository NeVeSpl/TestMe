using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.App;
using TestMe.BuildingBlocks.Tests;
using TestMe.TestCreation.App.Tests;
using TestMe.TestCreation.App.Tests.Input;
using TestMe.TestCreation.Persistence;
using static TestMe.TestCreation.TestUtils;

namespace TestMe.TestCreation.Tests.App
{
    [TestClass]
    public class TestsServiceTests : BaseFixture
    {
        private TestCreationDbContext testCreationDbContext;
        private TestsService serviceUnderTest;

        private protected override FakeDatabaseType GetDatabaseType()
        {
            return FakeDatabaseType.SQLiteInMemory;
        }        

        [TestInitialize]
        public void TestInitialize()
        {            
            testCreationDbContext = CreateTestCreationDbContext();
            var uow = TestUtils.CreateTestCreationUoW(testCreationDbContext);
            serviceUnderTest = new TestsService(new TestReader(CreateReadOnlyTestCreationDbContext()), uow);
        }
        [TestCleanup]
        public void TestCleanup()
        {
            testCreationDbContext.Dispose();
        }


        [TestMethod]
        [DataRow(ValidTestsCatalog1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestsCatalogId, ResultStatus.NotFound)]
        [DataRow(ValidQuestionsCatalog2Id, ResultStatus.NotFound)]
        [DataRow(DeletedTestsCatalogId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestsCatalogId, ResultStatus.Unauthorized)]
        public void ReadTestHeaders(long catalogId, ResultStatus expectedResult)
        {
            var result = serviceUnderTest.ReadTestHeaders(OwnerId, catalogId, new OffsetPagination());
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]
        public void ReadTestWithQuestionItemsAndQuestionHeaders(long testId, ResultStatus expectedResult)
        {
            var result = serviceUnderTest.ReadTestWithQuestionItemsAndQuestionHeaders(OwnerId, testId);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestsCatalog1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestsCatalogId, ResultStatus.Error)]
        [DataRow(ValidQuestionsCatalog2Id, ResultStatus.Error)]
        [DataRow(DeletedTestsCatalogId, ResultStatus.Error)]
        [DataRow(OtherOwnerTestsCatalogId, ResultStatus.Unauthorized)]
        public void CreateTest(long catalogId, ResultStatus expectedResult)
        {
            var command = new CreateTest()
            {
                UserId = OwnerId,
                Title = "Roberto Carlos",
                CatalogId = catalogId,
            };
            var result = serviceUnderTest.CreateTest(command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]
        public void UpdateTest(long testId, ResultStatus expectedResult)
        {
            var command = new UpdateTest()
            {
                UserId = OwnerId,
                Title = "Roberto Carlos",
                CatalogId = ValidTestsCatalog1Id,
                TestId = testId
            };
            Result result = serviceUnderTest.UpdateTest(command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]
        public void DeleteTest(long testId, ResultStatus expectedResult)
        {
            var command = new DeleteTest()
            {
                UserId = OwnerId,
                TestId = testId,
            };
            Result result = serviceUnderTest.DeleteTest(command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]      
        public void CreateQuestionItem_WhenGivenTestId(long testId, ResultStatus expectedResult)
        {
            var command = new CreateQuestionItem()
            {
                UserId = OwnerId,
                QuestionId = ValidQuestion1Id,
                TestId = testId
            };
            var result = serviceUnderTest.CreateQuestionItem(command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidQuestion1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngQuestionId, ResultStatus.Error)]
        [DataRow(DeletedQuestionId, ResultStatus.Error)]
        [DataRow(OtherOwnerQuestionId, ResultStatus.Unauthorized)]
        public void CreateQuestionItem_WhenGivenQuestionId(long questionId, ResultStatus expectedResult)
        {
            var command = new CreateQuestionItem()
            {
                UserId = OwnerId,
                QuestionId = questionId,
                TestId = ValidTest1Id,
            };
            var result = serviceUnderTest.CreateQuestionItem(command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]
        public void UpdateQuestionItem_WhenGivenTestId(long testId, ResultStatus expectedResult)
        {
            var command = new UpdateQuestionItem()
            {
                UserId = OwnerId,
                TestId = testId,
                QuestionItemId = ValidTestItemId,
            };
            Result result = serviceUnderTest.UpdateQuestionItem(command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestItemId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestItemId, ResultStatus.NotFound)]
        [DataRow(OtherTestTestItemId, ResultStatus.NotFound)]      
        public void UpdateQuestionItem_WhenGivenQuestionItemId(long questionItemId, ResultStatus expectedResult)
        {
            var command = new UpdateQuestionItem()
            {
                UserId = OwnerId,
                TestId = ValidTest1Id,
                QuestionItemId = questionItemId,
            };
            Result result = serviceUnderTest.UpdateQuestionItem(command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]
        public void DeleteQuestionItem_WhenGivenTestId(long testId, ResultStatus expectedResult)
        {
            var command = new DeleteQuestionItem()
            {
                UserId = OwnerId,
                TestId = testId,
                QuestionItemId = ValidTestItemId
            };
            Result result = serviceUnderTest.DeleteQuestionItem(command);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestItemId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestItemId, ResultStatus.NotFound)]
        [DataRow(OtherTestTestItemId, ResultStatus.NotFound)]
        public void DeleteQuestionItem_WhenGivenQuestionItemId(long questionItemId, ResultStatus expectedResult)
        {
            var command = new DeleteQuestionItem()
            {
                UserId = OwnerId,
                TestId = ValidTest1Id,
                QuestionItemId = questionItemId
            };
            Result result = serviceUnderTest.DeleteQuestionItem(command);
            Assert.AreEqual(expectedResult, result.Status);
        }
    }
}

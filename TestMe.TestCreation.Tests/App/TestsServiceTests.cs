using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.App;
using TestMe.BuildingBlocks.Tests;
using TestMe.TestCreation.App.RequestHandlers.Tests.CreateTestItem;
using TestMe.TestCreation.App.RequestHandlers.Tests.CreateTest;
using TestMe.TestCreation.App.RequestHandlers.Tests.DeleteTestItem;
using TestMe.TestCreation.App.RequestHandlers.Tests.DeleteTest;
using TestMe.TestCreation.App.RequestHandlers.Tests.ReadTestItems;
using TestMe.TestCreation.App.RequestHandlers.Tests.ReadTest;
using TestMe.TestCreation.App.RequestHandlers.Tests.ReadTests;
using TestMe.TestCreation.App.RequestHandlers.Tests.UpdateTestItem;
using TestMe.TestCreation.App.RequestHandlers.Tests.UpdateTest;
using TestMe.TestCreation.Persistence;
using static TestMe.TestCreation.TestUtils;

namespace TestMe.TestCreation.Tests.App
{
    [TestClass]
    public class TestsServiceTests : BaseFixture
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
            uow = TestUtils.CreateTestCreationUoW(testCreationDbContext);
            readOnlyTestCreationDbContext = CreateReadOnlyTestCreationDbContext();           
        }
        [TestCleanup]
        public void TestCleanup()
        {
            testCreationDbContext.Dispose();
        }


        [TestMethod]
        [DataRow(OwnerId, ResultStatus.Ok)]
        [DataRow(OtherOwnerId, ResultStatus.Unauthorized)]       
        public async Task ReadTests(long userId, ResultStatus expectedResult)
        {
            var handler = new ReadTestsHandler(readOnlyTestCreationDbContext);
            var query = new ReadTestsQuery(OwnerId, new OffsetPagination()) { UserId = userId };             
            var result = await handler.Handle(query, default);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]
        public async Task ReadTest(long testId, ResultStatus expectedResult)
        {          
            var handler = new ReadTestHandler(readOnlyTestCreationDbContext);
            var query = new ReadTestQuery(testId) { UserId = OwnerId };
            var result = await handler.Handle(query, default);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(OwnerId, ResultStatus.Ok)]      
        [DataRow(OtherOwnerId, ResultStatus.Unauthorized)]
        public async Task CreateTest(long userId, ResultStatus expectedResult)
        {
            var handler = new CreateTestHandler(uow);
            var command = new CreateTestCommand()
            {
                UserId = OwnerId,
                Title = "Roberto Carlos",
                OwnerId = userId

            };
            var result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]
        public async Task UpdateTest(long testId, ResultStatus expectedResult)
        {
            var handler = new UpdateTestHandler(uow);
            var command = new UpdateTestCommand()
            {
                UserId = OwnerId,
                Title = "Roberto Carlos",               
                TestId = testId
            };
            var result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]
        public async Task DeleteTest(long testId, ResultStatus expectedResult)
        {
            var handler = new DeleteTestHandler(uow);
            var command = new DeleteTestCommand()
            {
                UserId = OwnerId,
                TestId = testId,
            };
            var result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }


        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]
        public async Task ReadTestItems(long testId, ResultStatus expectedResult)
        {
            var handler = new ReadTestItemsHandler(readOnlyTestCreationDbContext);
            var query = new ReadTestItemsQuery(testId) { UserId = OwnerId };
            var result = await handler.Handle(query, default);
            Assert.AreEqual(expectedResult, result.Status);
        }


        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]      
        public async Task CreateTestItems_WhenGivenTestId(long testId, ResultStatus expectedResult)
        {
            var handler = new CreateTestItemHandler(uow);
            var command = new CreateTestItemCommand()
            {
                UserId = OwnerId,
                QuestionId = ValidQuestion1Id,
                TestId = testId
            };
            var result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidQuestion1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngQuestionId, ResultStatus.Error)]
        [DataRow(DeletedQuestionId, ResultStatus.Error)]
        [DataRow(OtherOwnerQuestionId, ResultStatus.Unauthorized)]
        public async Task CreateTestItems_WhenGivenQuestionId(long questionId, ResultStatus expectedResult)
        {
            var handler = new CreateTestItemHandler(uow);
            var command = new CreateTestItemCommand()
            {
                UserId = OwnerId,
                QuestionId = questionId,
                TestId = ValidTest1Id,
            };
            var result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]
        public async Task UpdateTestItems_WhenGivenTestId(long testId, ResultStatus expectedResult)
        {
            var handler = new UpdateTestItemHandler(uow);
            var command = new UpdateTestItemCommand()
            {
                UserId = OwnerId,
                TestId = testId,
                TestItemId = ValidTestItemId,
            };
            var result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestItemId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestItemId, ResultStatus.NotFound)]
        [DataRow(OtherTestTestItemId, ResultStatus.NotFound)]      
        public async Task UpdateTestItems_WhenGivenQuestionItemId(long questionItemId, ResultStatus expectedResult)
        {
            var handler = new UpdateTestItemHandler(uow);
            var command = new UpdateTestItemCommand()
            {
                UserId = OwnerId,
                TestId = ValidTest1Id,
                TestItemId = questionItemId,
            };
            var result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTest1Id, ResultStatus.Ok)]
        [DataRow(NotExisitngTestId, ResultStatus.NotFound)]
        [DataRow(DeletedTestId, ResultStatus.NotFound)]
        [DataRow(OtherOwnerTestId, ResultStatus.Unauthorized)]
        public async Task DeleteTestItems_WhenGivenTestId(long testId, ResultStatus expectedResult)
        {
            var handler = new DeleteTestItemHandler(uow);
            var command = new DeleteTestItemCommand()
            {
                UserId = OwnerId,
                TestId = testId,
                TestItemId = ValidTestItemId
            };
            var result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }

        [TestMethod]
        [DataRow(ValidTestItemId, ResultStatus.Ok)]
        [DataRow(NotExisitngTestItemId, ResultStatus.NotFound)]
        [DataRow(OtherTestTestItemId, ResultStatus.NotFound)]
        public async Task DeleteTestItems_WhenGivenQuestionItemId(long questionItemId, ResultStatus expectedResult)
        {               
            var handler = new DeleteTestItemHandler(uow);
            var command = new DeleteTestItemCommand()
            {
                UserId = OwnerId,
                TestId = ValidTest1Id,
                TestItemId = questionItemId
            };
            var result = await handler.Handle(command, default);
            Assert.AreEqual(expectedResult, result.Status);
        }
    }
}

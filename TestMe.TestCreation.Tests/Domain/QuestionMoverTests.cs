using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestMe.BuildingBlocks.Domain;
using TestMe.BuildingBlocks.Tests;
using TestMe.TestCreation.Domain;
using static TestMe.TestCreation.TestUtils;

namespace TestMe.TestCreation.Tests.Domain
{
    [TestClass]
    public class QuestionMoverTests : BaseFixture
    {
        private protected override FakeDatabaseType GetDatabaseType()
        {
            return FakeDatabaseType.EFInMemory;
        }

       
        [TestMethod]
        [DataRow(ValidQuestion1Id, 2)]
        public void MoveQuestionToCatalog_HappyPathIsSuccessful(long questionId, long catalogId)
        {
            const int maxNumberOfQuestionsInCatalog = 3;
            Mock<IAddQuestionPolicy> policyMock = new Mock<IAddQuestionPolicy>();
            policyMock.Setup(x => x.CanAddQuestion(It.IsAny<int>())).Returns<int>(x => x < maxNumberOfQuestionsInCatalog);

            using (var context = CreateTestCreationDbContext())
            {
                var uow = TestUtils.CreateTestCreationUoW(context);
                Question question = uow.Questions.GetByIdWithAnswers(questionId);

                QuestionMover.MoveQuestionToCatalog(question, catalogId, uow.QuestionsCatalogs, policyMock.Object);
                uow.Save();
            }

            using (var context = CreateTestCreationDbContext())
            {
                Question question = context.Questions.Find(questionId);
                Assert.AreEqual(catalogId, question.CatalogId);
            }
        }

        [TestMethod]
        [DataRow(DeletedQuestionsCatalogId, "Catalog not found")]
        [DataRow(ValidTestsCatalog1Id, "Catalog not found")]
        [DataRow(NotExisitngQuestionsCatalogId, "Catalog not found")]
        [DataRow(OtherOwnerQuestionsCatalogId, "Question can not be moved to catalog that you do not own")]
        public void MoveQuestionToCatalog_ErrorPathThrows(long catalogId, string expectedErrorMessage)
        {
            using (var context = CreateTestCreationDbContext())
            {
                var uow = TestUtils.CreateTestCreationUoW(context);
                Question question = uow.Questions.GetByIdWithAnswers(ValidQuestion1Id);
                var exception = Assert.ThrowsException<DomainException>(() => QuestionMover.MoveQuestionToCatalog(question, catalogId, uow.QuestionsCatalogs, null));
                Assert.AreEqual(expectedErrorMessage, exception.Message);
            }
        }
    }
}

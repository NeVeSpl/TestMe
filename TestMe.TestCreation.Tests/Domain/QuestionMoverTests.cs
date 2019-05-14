using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestMe.SharedKernel.Domain;
using TestMe.TestCreation.Domain;
using static TestMe.TestCreation.TestUtils;

namespace TestMe.TestCreation.Tests.Domain
{
    [TestClass]
    public class QuestionMoverTests : BaseFixture
    {
        private protected override DatabaseType GetDatabaseType()
        {
            return DatabaseType.EFInMemory;
        }


        [TestInitialize]
        public void TestInitialize()
        {
            using (var context = CreateTestCreationDbContext())
            {
                TestUtils.Seed(context);
            }
        }

        [TestMethod]
        [DataRow(ValidQuestionId, 2)]
        public void MoveQuestionToCatalog_HappyPath(long questionId, long catalogId)
        {
            const int maxNumberOfquestionsIncatalog = 3;
            Mock<IAddQuestionPolicy> policyMock = new Mock<IAddQuestionPolicy>();
            policyMock.Setup(x => x.CanAddQuestion(It.IsAny<int>())).Returns<int>(x => x < maxNumberOfquestionsIncatalog);

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
        [DataRow(ValidTestsCatalogId, "Catalog not found")]
        [DataRow(NotExisitngQuestionsCatalogId, "Catalog not found")]
        [DataRow(OtherOwnerQuestionsCatalogId, "Question can not be moved to catalog that you do not own")]
        public void MoveQuestionToCatalog_ErrorPath(long catalogId, string expectedErrorMessage)
        {
            using (var context = CreateTestCreationDbContext())
            {
                var uow = TestUtils.CreateTestCreationUoW(context);
                Question question = uow.Questions.GetByIdWithAnswers(ValidQuestionId);
                var exception = Assert.ThrowsException<DomainException>(() => QuestionMover.MoveQuestionToCatalog(question, catalogId, uow.QuestionsCatalogs, null));
                Assert.AreEqual(expectedErrorMessage, exception.Message);
            }
        }
    }
}

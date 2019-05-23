using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestMe.SharedKernel.Domain;
using TestMe.TestCreation.Domain;
using static TestMe.TestCreation.TestUtils;

namespace TestMe.TestCreation.Tests.Domain
{
    [TestClass]
    public class QuestionsCatalogTests : BaseFixture
    {
        private protected override DatabaseType GetDatabaseType()
        {
            return DatabaseType.EFInMemory;
        }


        [TestMethod]      
        public void AddQuestion_ShouldThrowErrorWhenExceededTheLimitOfQuestionsInCatalog()
        {
            const int maxNumberOfQuestionsInCatalog = 3;
            Mock<IAddQuestionPolicy> policyMock = new Mock<IAddQuestionPolicy>();
            policyMock.Setup(x => x.CanAddQuestion(It.IsAny<int>())).Returns<int>(x => x < maxNumberOfQuestionsInCatalog);

            using (var context = CreateTestCreationDbContext())
            {
                var uow = TestUtils.CreateTestCreationUoW(context);
                var catalog = uow.QuestionsCatalogs.GetById(ValidQuestionsCatalogId);

                var q1 = Question.Create("Q1", OwnerId);               
                Assert.ThrowsException<DomainException>(() => catalog.AddQuestion(q1, policyMock.Object));
                uow.Save();
            }
        }

        [TestMethod]
        public void AddQuestion_ShouldIncreaseQuestionsCount()
        {
            const int maxNumberOfQuestionsInCatalog = 4;
            Mock<IAddQuestionPolicy> policyMock = new Mock<IAddQuestionPolicy>();
            policyMock.Setup(x => x.CanAddQuestion(It.IsAny<int>())).Returns<int>(x => x < maxNumberOfQuestionsInCatalog);

            using (var context = CreateTestCreationDbContext())
            {
                var uow = TestUtils.CreateTestCreationUoW(context);
                var catalog = uow.QuestionsCatalogs.GetById(ValidQuestionsCatalogId);

                var q1 = Question.Create("Q1", OwnerId);

                catalog.AddQuestion(q1, policyMock.Object);               
                uow.Save();
            }

            using (var context = CreateTestCreationDbContext())
            {
                var catalog = context.QuestionsCatalogs.Find(ValidQuestionsCatalogId);
                Assert.AreEqual(maxNumberOfQuestionsInCatalog, catalog.QuestionsCount);
            }
        }
    }
}

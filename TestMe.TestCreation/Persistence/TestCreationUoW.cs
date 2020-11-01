using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    internal sealed class TestCreationUoW : ITestCreationUoW, IDisposable
    {
        private readonly TestCreationDbContext context;
        private readonly Lazy<IQuestionRepository> questions;
        private readonly Lazy<IQuestionsCatalogRepository> questionsCatalogs;
        private readonly Lazy<ITestsCatalogRepository> testsCatalogs;
        private readonly Lazy<ITestRepository> tests;
        private readonly Lazy<IOwnerRepository> owners;

        public IQuestionRepository Questions { get => questions.Value; }
        public IQuestionsCatalogRepository QuestionsCatalogs { get => questionsCatalogs.Value; }
        public ITestsCatalogRepository TestsCatalogs { get => testsCatalogs.Value; }
        public ITestRepository Tests { get => tests.Value; }
        public IOwnerRepository Owners { get => owners.Value; }


        public TestCreationUoW(TestCreationDbContext context)
        {
            this.context = context;
            questions = new Lazy<IQuestionRepository>(() => new QuestionRepository(context));
            questionsCatalogs = new Lazy<IQuestionsCatalogRepository>(() => new QuestionsCatalogRepository(context));
            testsCatalogs = new Lazy<ITestsCatalogRepository>(() => new TestsCatalogRepository(context));
            tests = new Lazy<ITestRepository>(() => new TestRepository(context));
            owners = new Lazy<IOwnerRepository>(() => new OwnerRepository(context));
        }

        public Task Save()
        {
            UpdateQuestionWhenAnyAnswerChanged();
            return context.SaveChangesAsync();
        }

        private void UpdateQuestionWhenAnyAnswerChanged()
        {
            /* We need this to generate a new concurrency token for parent entity/aggregate root(Question) when any of owned entities(Answer) has been changed.
             * Current ef core (2.2) implementation of owned entities doesn't update owner when any of owned was changed :(.
             */
            foreach (EntityEntry<Question> questionEntry in context.ChangeTracker.Entries<Question>())
            {
                if (questionEntry.State == EntityState.Unchanged)
                {
                    bool ownedEntitesWereChanged = false;
                    foreach (Answer answer in questionEntry.Entity.Answers)
                    {
                        var answerEntry = context.Entry(answer);
                        ownedEntitesWereChanged = answerEntry.State != EntityState.Unchanged;
                        if (ownedEntitesWereChanged) break;
                    }

                    if (ownedEntitesWereChanged)
                    {
                        questionEntry.State = EntityState.Modified;
                    }
                }
            }
        }



        public void Dispose()
        {
            context.Dispose();
        }
    }
}

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    internal sealed class TestCreationUoW : ITestCreationUoW, IDisposable
    {
        private readonly TestCreationDbContext context;

        public IQuestionRepository Questions { get; }
        public IQuestionsCatalogRepository QuestionsCatalogs { get; }
        public ITestsCatalogRepository TestsCatalogs { get; }
        public ITestRepository Tests { get; }
        public IOwnerRepository Owners { get; }


        public TestCreationUoW(TestCreationDbContext context)
        {
            this.context = context;
            Questions = new QuestionRepository(context);
            QuestionsCatalogs = new QuestionsCatalogRepository(context);
            TestsCatalogs = new TestsCatalogRepository(context);
            Tests = new TestRepository(context);
            Owners = new OwnerRepository(context);

        }

        public void Save()
        {
            UpdateQuestionWhenAnyAnswerChanged();
            context.SaveChanges();
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

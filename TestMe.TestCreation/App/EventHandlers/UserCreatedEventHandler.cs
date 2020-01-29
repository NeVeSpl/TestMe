using System.Threading.Tasks;
using TestMe.BuildingBlocks.EventBus;
using TestMe.SharedKernel.Domain;
using TestMe.TestCreation.Domain;
using TestMe.UserManagement.IntegrationEvents;

namespace TestMe.TestCreation.App.EventHandlers
{
    internal class UserCreatedEventHandler : IEventHandler<UserCreatedV1>
    {
        private readonly ITestCreationUoW uow;

        public UserCreatedEventHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }



        public Task Handle(UserCreatedV1 @event)
        {
            var newOwner = Owner.Create(@event.UserId, @event.MembershipLevel);
            uow.Owners.Add(newOwner);
            SeedDataForNewOwner(newOwner);
            uow.Save();
            return Task.CompletedTask;
        }

        private void SeedDataForNewOwner(Owner owner)
        {
            var addQuestionsCatalogPolicy = AddQuestionsCatalogPolicyFactory.Create(owner.MembershipLevel);
            var addQuestionPolicy = AddQuestionPolicyFactory.Create(owner.MembershipLevel);

            var footballCategory = owner.AddQuestionsCatalog("Football", addQuestionsCatalogPolicy);
            var generalKnowledgeCategory = owner.AddQuestionsCatalog("Basic General Knowledge", addQuestionsCatalogPolicy);
            var famousPeople = owner.AddQuestionsCatalog("Famous people", addQuestionsCatalogPolicy);

            var q1 = Question.Create("Entomology is the science that studies", owner.OwnerId);
            q1.AddAnswer("Behavior of human beings", false);
            q1.AddAnswer("Insects", true);
            q1.AddAnswer("The origin and history of technical and scientific terms", false);
            q1.AddAnswer("The formation of rocks", false);
            generalKnowledgeCategory.AddQuestion(q1, addQuestionPolicy);

            var q2 = Question.Create("For which of the following disciplines is Nobel Prize awarded?", owner.OwnerId);
            q2.AddAnswer("Physics and Chemistry", false);
            q2.AddAnswer("Physiology or Medicine", false);
            q2.AddAnswer("Literature, Peace and Economics", false);
            q2.AddAnswer("All of the above", true);
            generalKnowledgeCategory.AddQuestion(q2, addQuestionPolicy);

            var q3 = Question.Create("Grand Central Terminal, Park Avenue, New York is the world's", owner.OwnerId);
            q3.AddAnswer("largest railway station", true);
            q3.AddAnswer("highest railway station", false);
            q3.AddAnswer("longest railway station", false);
            q3.AddAnswer("none of the above", false);
            generalKnowledgeCategory.AddQuestion(q3, addQuestionPolicy);

            var q4 = Question.Create("Which insect inspired the term \"computer bug\"?", owner.OwnerId);
            q4.AddAnswer("Moth1", false);
            q4.AddAnswer("Cockroach3", true);
            q4.AddAnswer("Fly2", false);
            q4.AddAnswer("Beetle", false);
            generalKnowledgeCategory.AddQuestion(q4, addQuestionPolicy);

            var q5 = Question.Create("Who is the patron saint of Spain?", owner.OwnerId);
            q5.AddAnswer("Saint James", false);
            q5.AddAnswer("Saint Peter", true);
            q5.AddAnswer("Saint John", false);
            q5.AddAnswer("Saint Percy", false);
            generalKnowledgeCategory.AddQuestion(q5, addQuestionPolicy);

            var q6 = Question.Create("Frederick Sanger is a twice recipient of the Nobel Prize for", owner.OwnerId);
            q6.AddAnswer("Chemistry in 1958 and 1980", true);
            q6.AddAnswer("Physics in 1956 and 1972", false);
            q6.AddAnswer("Chemistry in 1954 and Peace in 1962", false);
            q6.AddAnswer("Physics in 1903 and Chemistry in 1911", false);
            famousPeople.AddQuestion(q6, addQuestionPolicy);

            var q7 = Question.Create("Galileo was an Italian astronomer who", owner.OwnerId);
            q7.AddAnswer("developed the telescope", false);
            q7.AddAnswer("discovered four satellites of Jupiter", false);
            q7.AddAnswer("discovered that the movement of pendulum produces a regular time measurement", false);
            q7.AddAnswer("all of the above", true);
            famousPeople.AddQuestion(q7, addQuestionPolicy);
        }
    }
}

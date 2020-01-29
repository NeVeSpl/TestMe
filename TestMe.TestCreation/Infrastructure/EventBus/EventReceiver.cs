using System;
using System.Linq;
using System.Threading.Tasks;
using TestMe.BuildingBlocks.EventBus;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.Infrastructure.EventBus
{
    internal sealed class EventReceiver : IEventInterceptor
    {
        private readonly TestCreationDbContext testCreationDbContext;

        public EventReceiver(TestCreationDbContext testCreationDbContext)
        {
            this.testCreationDbContext = testCreationDbContext;
        }


        public async Task ReceiveEvent(Event @event, Func<Task> handleEvent)
        {
            using (var transaction = testCreationDbContext.Database.BeginTransaction())
            {
                bool eventAlreadyProcessed = testCreationDbContext.Inbox.Any(x => x.EventId == @event.EventId);
                if (!eventAlreadyProcessed)
                {
                    await handleEvent();
                    @event.ReceivedDateTime = DateTimeOffset.UtcNow;
                    testCreationDbContext.Inbox.Add(@event);
                    testCreationDbContext.SaveChanges();
                    transaction.Commit();
                }
            }
        }
    }
}

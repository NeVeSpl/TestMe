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
            bool eventAlreadyProcessed = testCreationDbContext.Inbox.Any(x => x.EventId == @event.EventId);
            if (!eventAlreadyProcessed)
            {
                using (var transaction = await testCreationDbContext.Database.BeginTransactionAsync())
                {
                    await handleEvent();
                    @event.ReceivedDateTime = DateTimeOffset.UtcNow;
                    testCreationDbContext.Inbox.Add(@event);
                    await testCreationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
        }
    }
}

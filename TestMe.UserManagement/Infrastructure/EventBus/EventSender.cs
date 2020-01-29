using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestMe.BuildingBlocks.EventBus;
using TestMe.UserManagement.Persistence;

namespace TestMe.UserManagement.Infrastructure.EventBus
{
    internal sealed class EventSender : IEventSender
    {
        private readonly UserManagementDbContext dbContext;
        private readonly IEventBus eventBus;


        public EventSender(UserManagementDbContext dbContext, IEventBus eventBus)
        {
            this.dbContext = dbContext;
            this.eventBus = eventBus;
        }


        public async Task SendEvents(CancellationToken cancellationToken)
        {
            var eventsToSend = dbContext.Outbox.Where(x => x.PostDateTime == null).OrderBy(x => x.EventId).Take(100);
            foreach(var eventToSend in eventsToSend)
            {
                eventToSend.PostDateTime = DateTimeOffset.UtcNow;
                bool wasPublished = await eventBus.Publish(eventToSend);
                if (wasPublished)
                {
                    await dbContext.SaveChangesAsync();
                } 
                else
                {
                    break;
                }
            }
        }
    }
}
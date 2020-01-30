using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestMe.BuildingBlocks.EventBus;

namespace TestMe.Presentation.API.BackgroundServices
{
    public sealed class PostManService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<PostManService> logger;
        private readonly static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly Config config;


        public PostManService(IServiceProvider services, ILogger<PostManService> logger, IOptions<Config> config)
        {
            this.serviceProvider = services;
            this.logger = logger;
            this.config = config.Value;
        }


        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await DispatchMessages(cancellationToken);
                await Task.Delay(config.DelayTimeBetweenDispatches, cancellationToken);
            }
        }

        public async Task DispatchMessages(CancellationToken cancellationToken)
        {
            await semaphore.WaitAsync();
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var eventSenders = scope.ServiceProvider.GetServices<IEventSender>();
                    foreach (var eventSender in eventSenders)
                    {
                        try
                        {
                            await eventSender.SendEvents(cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "PostManService failed to send events.");
                        }
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }



        public class Config
        {
            public int DelayTimeBetweenDispatches { get; set; }



            public Config()
            {
                DelayTimeBetweenDispatches = 1000;
            }
        }
    }
}

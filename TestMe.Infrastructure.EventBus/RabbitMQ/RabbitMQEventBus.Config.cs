namespace TestMe.Infrastructure.EventBus.RabbitMQ
{
    public sealed partial class RabbitMQEventBus
    {
        public class Config
        {
            public string HostName { get; set; }


            public Config()
            {
                HostName = "localhost";
            }
        }
    }
}

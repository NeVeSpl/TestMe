using System;
using System.Net.Sockets;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace TestMe.Infrastructure.EventBus.RabbitMQ
{
    public class RabbitMQPersistentConnection
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly object syncRoot = new object();
        private IConnection? connection;

        public bool IsConnected
        {
            get
            {
                return connection != null && connection.IsOpen;
            }
        }


        public RabbitMQPersistentConnection(string hostName)
        {
            connectionFactory = new ConnectionFactory() { HostName = hostName, DispatchConsumersAsync = true };
        }
               

        public IModel CreateChannel()
        {
            TryConnect();

            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return connection!.CreateModel();
        }

        private bool TryConnect()
        {
            if (IsConnected)
            {
                return true;
            }           

            lock (syncRoot)
            {
                if (IsConnected)
                {
                    return true;
                }

                //_logger.LogInformation("RabbitMQ Client is trying to connect");

                if (connection != null)
                {
                    connection.ConnectionShutdown -= OnConnectionShutdown;
                    connection.CallbackException -= OnCallbackException;
                    connection.ConnectionBlocked -= OnConnectionBlocked;
                    connection.Dispose();
                }

                var policy = RetryPolicy.Handle<SocketException>()
                                        .Or<BrokerUnreachableException>()
                                        .WaitAndRetry(retryCount: 3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        //_logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                    }
                ) ;

                policy.Execute(() =>
                {
                    connection = connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    connection!.ConnectionShutdown += OnConnectionShutdown;
                    connection!.CallbackException += OnCallbackException;
                    connection!.ConnectionBlocked += OnConnectionBlocked;

                    //_logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);
                    return true;
                }
                else
                {
                    //_logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                    return false;
                }
            }

            void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
            {                
                //_logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");
                TryConnect();
            }
            void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
            {               
                //_logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");
                TryConnect();
            }
            void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
            {             
                //_logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");
                TryConnect();
            }
        }
    }
}
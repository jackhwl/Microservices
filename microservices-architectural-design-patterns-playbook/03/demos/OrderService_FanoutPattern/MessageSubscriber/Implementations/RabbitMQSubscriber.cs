using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageSubscriber.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using MessageSubscriber.Models;

namespace MessageSubscriber.Implementations
{
    public class RabbitMQSubscriber : ISubscriber
    {
        private bool _subscribing;
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        private const string Exchange = "OrderFanoutExchange";
        private IMessageProcessor _messageProcessor;
        private string _queueName;

        public bool Subscribing
        {
            get { return _subscribing; }
            set { _subscribing = value; }
        }

        public RabbitMQSubscriber(IMessageProcessor messageProcessor)
        {
            _subscribing = true;
            _messageProcessor = messageProcessor;
        }

        public void Subscribe()
        {
            ConnectToMessageBroker();

            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    SetupChannel(channel);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(_queueName, true, consumer);

                    Logger.LogInfo("Connected and listening...");
                    while (_subscribing)
                    {
                        GetAMessageFromQueue(consumer, channel);
                    }
                }
            }
        }

        private void GetAMessageFromQueue(QueueingBasicConsumer consumer, IModel channel)
        {
            var m = consumer.Queue.Dequeue();
            var message = (Message)m.Body.DeSerializeFromByteArray(typeof(Message));

            try
            {
                _messageProcessor.Process(message);
                //channel.BasicAck(m.DeliveryTag, false);
            } catch (Exception e)
            {
                Logger.LogError(e);
                //channel.BasicNack(m.DeliveryTag, false, true);
            }
        }

        private void SetupChannel(IModel channel)
        {
            channel.ExchangeDeclare(Exchange, "fanout");
            _queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(_queueName, Exchange, "");

            channel.BasicQos(0, 1, false);
        }

        private void ConnectToMessageBroker()
        {
            Logger.LogInfo("Connecting to Message Broker...");
            _factory = new ConnectionFactory { HostName = "192.168.0.20", UserName = "test", Password = "test" };
        }

        public void UnSubscribe()
        {
            _subscribing = false;
        }
    }
}

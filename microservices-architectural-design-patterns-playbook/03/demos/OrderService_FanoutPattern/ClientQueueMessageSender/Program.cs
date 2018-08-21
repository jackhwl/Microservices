using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientQueueMessageSender
{
    class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;
        private const string ExchangeName = "OrderFanoutExchange";

        static void Main(string[] args)
        {
            string customerUserName;
            int NoOfOrders;
            Message queueMessage;
            Order order;

            CreateConnection();

            while (true)
            {
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Please enter customer username:");
                customerUserName = Console.ReadLine();

                Console.WriteLine("Please enter no. of orders:");
                NoOfOrders = int.Parse(Console.ReadLine());

                for (int i = 1; i <= NoOfOrders; i++)
                {
                    order = new Order(i, customerUserName);
                    queueMessage = CreateMessageForQueuing(order);
                    SendMessage(queueMessage);
                }


            }
        }

        private static Message CreateMessageForQueuing(Order order)
        {
            return new Message { CorrelationId = Guid.NewGuid(), Data = order.SerializeToJSON() };
        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = "192.168.0.20", UserName = "test", Password = "test" };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, "fanout", false);
        }

        private static void SendMessage(Message message)
        {
            _model.BasicPublish(ExchangeName, "", null, message.SerializeToByteArray());
            //Console.WriteLine(" Payment Sent {0}, £{1}", message.CardNumber, message.AmountToPay);
        }
    }
}


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
        private const string ExchangeName = "EmailExchange";

        static void Main(string[] args)
        {
            string emailaddress;
            string emailsubject;
            string emailmessage;
            Message queueMessage;

            CreateConnection();

            while (true)
            {
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Please enter recipient email address:");
                emailaddress = Console.ReadLine();

                Console.WriteLine("Please enter email subject:");
                emailsubject = Console.ReadLine();

                Console.WriteLine("Please enter email message:");
                emailmessage = Console.ReadLine();

                var EmailMessage = new Email(emailaddress, emailsubject, emailmessage);
                queueMessage = CreateMessageForQueuing(EmailMessage);
                SendMessage(queueMessage);
            }
        }

        private static Message CreateMessageForQueuing(Email emailMessage)
        {
            return new Message { CorrelationId = new System.Guid(), Data = emailMessage.SerializeToJSON() };
        }

        private static void CreateConnection()
        {
            _factory = new ConnectionFactory { HostName = "192.168.0.20", UserName = "test", Password = "test" };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, "direct", false);
        }

        private static void SendMessage(Message message)
        {
            _model.BasicPublish(ExchangeName, "", null, message.SerializeToByteArray());
            //Console.WriteLine(" Payment Sent {0}, £{1}", message.CardNumber, message.AmountToPay);
        }
    }
}


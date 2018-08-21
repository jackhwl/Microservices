using System;
using System.Collections.Generic;
using System.Text;

namespace ClientQueueMessageSender
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public string DataType { get; set; }
        public string Data { get; set; }
        public string Sender { get; set; }
        public DateTime SentDateTime { get; set; }
        public int Retries { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }
}

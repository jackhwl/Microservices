using MessageSubscriber.Interfaces;
using MessageSubscriber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderPaymentService.Business
{
    public class OrderPaymentProcessor : IMessageProcessor
    {
        public void Process(Message message)
        {
            var order = (Order)message.Data.DeSerializeFromJSON(typeof(Order));
            Logger.LogInfoWithColor($"Processing Order Payment for order: {order.OrderNo} for {order.CustomerUserName}", ((order.OrderNo % 2 == 0)? ConsoleColor.Green:ConsoleColor.Yellow));
            System.Threading.Thread.Sleep(1000);
        }
    }
}

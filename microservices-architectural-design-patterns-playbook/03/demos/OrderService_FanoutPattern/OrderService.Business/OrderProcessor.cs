using MessageSubscriber.Interfaces;
using MessageSubscriber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Business
{
    public class OrderProcessor : IMessageProcessor
    {
        public void Process(Message message)
        {
            var order = (Order)message.Data.DeSerializeFromJSON(typeof(Order));
            Logger.LogInfoWithColor($"Processing Order: {order.OrderNo} for {order.CustomerUserName}", ((order.OrderNo % 2 == 0)? ConsoleColor.DarkGreen:ConsoleColor.DarkCyan));
            System.Threading.Thread.Sleep(1000);
        }
    }
}

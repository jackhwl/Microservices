using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageSubscriber.Interfaces;
using MessageSubscriber.Implementations;
using OrderTrackingService.Business;

namespace OrderTrackingService
{
    public static class DIContainer
    {
        public static IContainer Setup()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<RabbitMQSubscriber>().As<ISubscriber>();
            builder.RegisterType<OrderTrackingProcessor>().As<IMessageProcessor>();

            builder.RegisterType<OrderTrackingServiceRunner>();
            var container = builder.Build();
            return container;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageSubscriber.Implementations;
using MessageSubscriber.Interfaces;

namespace OrderTrackingService
{
    public class OrderTrackingServiceRunner
    {
        private ISubscriber _subscriber;

        public OrderTrackingServiceRunner(ISubscriber subscriber)
        {
            _subscriber = subscriber;
        }

        public void Start() {
            Logger.LogInfo($"Order Tracking Service Started");
            _subscriber.Subscribe();
        }

        public void Stop() {
            Logger.LogInfo($"Order Tracking Service Stopped");
        }
    }
}

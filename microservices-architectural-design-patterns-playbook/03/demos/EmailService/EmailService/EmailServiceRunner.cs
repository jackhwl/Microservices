using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageSubscriber.Implementations;
using MessageSubscriber.Interfaces;

namespace EmailService
{
    public class EmailServiceRunner
    {
        private ISubscriber _subscriber;

        public EmailServiceRunner(ISubscriber subscriber)
        {
            _subscriber = subscriber;
        }

        public void Start() {
            Logger.LogInfo($"Email Service Started");
            _subscriber.Subscribe();
        }

        public void Stop() {
            Logger.LogInfo($"Email Service Stopped");
        }
    }
}

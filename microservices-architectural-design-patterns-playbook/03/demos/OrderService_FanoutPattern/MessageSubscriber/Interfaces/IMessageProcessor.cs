using MessageSubscriber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSubscriber.Interfaces
{
    public interface IMessageProcessor
    {
        void Process(Message message);
    }
}

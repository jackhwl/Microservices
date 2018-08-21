using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSubscriber.Interfaces
{
    public interface ISubscriber
    {
        bool Subscribing { get; set; }
        void Subscribe();
        void UnSubscribe();
    }
}

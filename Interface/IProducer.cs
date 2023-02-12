using EventCluster.EventBus.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCluster.Interface
{
    internal interface IProducer
    {
        void Connect(string host, int port = 0);
        void Produce(Event @event);
    }
}

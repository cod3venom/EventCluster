using EventCluster.EventBus.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCluster.Interface
{
    public interface IConsumerProvider
    {
        event EventHandler<Event> OnDataReceived;
        void Connect(string host, int port = 0);
    }
}

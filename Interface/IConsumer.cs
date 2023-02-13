using EventCluster.EventBus.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCluster.Interface
{
    internal interface IConsumer
    {
        void On(string topic, Action<Event> callBack);
    }
}

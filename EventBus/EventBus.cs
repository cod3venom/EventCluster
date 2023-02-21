using EventCluster.EventBus.Type;
using EventCluster.Interface;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCluster.EventBus
{
    internal class EventBus
    {
        private Dictionary<string, Action<Event>> _eventStorage;
        private IConsumerProvider _consumerProvider;
        private IProducerProvider _producerProvider;
        public EventBus(IConsumerProvider consumerProvider)
        {
            this._eventStorage = new Dictionary<string, Action<Event>>();
            this._consumerProvider = consumerProvider;
            this.ListenForEvents();
        }
 
        public EventBus(IProducerProvider producerProvider)
        {
            this._producerProvider = producerProvider;
            this._eventStorage = new Dictionary<string, Action<Event>>();
        }
        public void On(string topic, Action<Event> callBack)
        {
            if (this._eventStorage.ContainsKey(topic)) {
                return;
            }

            this._eventStorage.Add(topic, callBack);
        }


        public void Produce(Event @event)
        {
            this._producerProvider.Produce(@event);
        }

        private void ListenForEvents()
        {
            this._consumerProvider.OnDataReceived += (delegate (object sender, Event @event)
            {
                foreach(KeyValuePair<string, Action<Event>> pair in _eventStorage)
                {
                    if (pair.Key != @event.Topic) {
                        continue;
                    }

                    pair.Value(@event);
                }
            });
        }
    }
}

using EventCluster.EventBus.Type;
using EventCluster.Interface;
using System;


namespace EventCluster
{
    public class EventConsumer
    {
        public event EventHandler<Event> OnDataReceived;

        private EventBus.EventBus _eventBus;
        private IConsumerProvider _consumerProvider;
        public EventConsumer(IConsumerProvider consumerProvider)
        {
            this._consumerProvider = consumerProvider;
            this._eventBus = new EventBus.EventBus(consumerProvider);
        }

        public void Connect(string host, int port = 0)
        {
            this._consumerProvider.Connect(host, port);
            this._consumerProvider.OnDataReceived += (delegate (object sender, Event @event)
            {
                this.OnDataReceived?.Invoke(this, @event);
            });
        }

        public void On(string topic, Action<Event> callBack)
        {
             this._eventBus.On(topic, callBack);
        }
    }
}

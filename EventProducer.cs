using EventCluster.EventBus.Type;
using EventCluster.Interface;


namespace EventCluster
{
    public class EventProducer 
    {
        private EventBus.EventBus _eventBus;
        private IProducerProvider _producerProvider;

        public EventProducer(IProducerProvider producerProvider)
        {
            this._producerProvider = producerProvider;
            this._eventBus = new EventBus.EventBus(producerProvider);
        }

        public void Connect(string host, int port = 0)
        {
            this._producerProvider.Connect(host, port);
        }

        public void Produce(Event @event)
        {
            this._eventBus.Produce(@event);
        }
    }
}

using EventCluster.EventBus.Type;
using EventCluster.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Channels;


namespace EventCluster.Provider.RabbitMQProvider
{
    public class RabbitConsumerProvider :  IConsumerProvider
    {
        public event EventHandler<Event> OnDataReceived;

        private ConnectionFactory _connectionFactory;
        
        private IConnection _connection;
        
        private IModel _channel;

        private readonly string _queueName;
        private readonly string _exchangeName;
        private readonly string _exchangeType;
        public RabbitConsumerProvider(string queueName = "", string exchangeName = "", string exchangeType = ExchangeType.Fanout)
        {
            this._queueName = queueName;
            this._exchangeName = exchangeName;
            this._exchangeType = exchangeType;
        }

        public void Connect(string host, int port = 0)
        {
            this._connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            this._connection = this._connectionFactory.CreateConnection();
            this._channel = this._connection.CreateModel();

            this._channel.ExchangeDeclare(this._exchangeName, this._exchangeType);

            QueueDeclareOk declarationResult = this._channel.QueueDeclare(queue: this._queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            this._channel.QueueBind(declarationResult.QueueName, this._exchangeName, "");

            new Thread(() =>
            {
                var consumer = new EventingBasicConsumer(this._channel);
                consumer.Received += ListenForEvents;

                this._channel.BasicConsume(
                    queue: this._queueName,
                    autoAck: true,
                    consumer: consumer
                );

                for (; ; );
            }).Start();

           
        }

        private void ListenForEvents(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            Event @event = Event.FromJSON(message);
            this.OnDataReceived?.Invoke(this, @event);
        }
    }
}

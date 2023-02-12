using EventCluster.EventBus.Type;
using EventCluster.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventCluster.Provider.RabbitMQProvider
{
    public class RabbitProducerProvider : IProducerProvider
    {
        private ConnectionFactory _connectionFactory;

        private IConnection _connection;

        private IModel _channel;

        private readonly string _queueName;
        private readonly string _exchangeName;
        private readonly string _exchangeType;
        public RabbitProducerProvider(string queueName = "", string exchangeName = "", string exchange = ExchangeType.Fanout)
        {
            this._queueName = queueName;
            this._exchangeName = exchangeName;
            this._exchangeType = exchange;
        }

        public void Connect(string host, int port = 0)
        {
            this._connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            this._connection = this._connectionFactory.CreateConnection();
            this._channel = this._connection.CreateModel();
            this._channel.ExchangeDeclare(this._exchangeName, this._exchangeType);

            QueueDeclareOk declarationResult = this._channel.QueueDeclare( queue: this._queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            this._channel.QueueBind(declarationResult.QueueName, this._exchangeName, "");
        }

        public void Produce(Event @event)
        {
            this._channel.BasicPublish(
               exchange: this._exchangeName,
               routingKey: "",
               basicProperties: null,
               body: @event.ToByte()
           );
        }
    }
}

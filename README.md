# EventCluster


#### Usage

##### For Event Producer

```
EventProducer producer = new EventProducer(new RabbitProducerProvider("", "test"));
producer.Connect("localhost");

while (true)
{
    Console.WriteLine("Provide event data #> "); // data should be JSON format input.
    string msg = Console.ReadLine();

    producer.Produce(new Event(
        "test_event",
        msg
    ));
}
```

##### For Event Consumer

```
EventConsumer consumer = new EventConsumer(new RabbitConsumerProvider("", "test"));
consumer.Connect("localhost");

// To see what events are consumed
consumer.OnDataReceived += (delegate (object sender, Event @e)
{
    Console.WriteLine(e.ToString());
    Console.WriteLine(Environment.NewLine);
});

// To listen for specific event
consumer.On("test_event", (Event @event) =>
{
    Console.WriteLine("Event Detected");
    Console.WriteLine(@event);
});
```

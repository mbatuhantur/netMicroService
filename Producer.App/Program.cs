// See https://aka.ms/new-console-template for more information
using Producer.App;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

Console.WriteLine("Producer");

var connectionFactory = new ConnectionFactory();
//connectionFactory.Uri = new Uri("amqps://udijpcsq:rjruXr0KRc3P27pfV6Oygxq5WvL0eY3V@shark.rmq.cloudamqp.com/udijpcsq");
connectionFactory.HostName="localhost";
var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();
channel.ConfirmSelect();

channel.ExchangeDeclare("topic-exchange",ExchangeType.Topic,true,false,null);


Enumerable.Range(1, 100).ToList().ForEach(x =>
{
    try
    {
        var userCreatedEvent = new UserCreatedEvent() { Id = x, Name = "ahmet", Email = "Ahmet@outlook.com" };

        var properties = channel.CreateBasicProperties();

        properties.Persistent = true;
        var userCreatedEventAsJson = JsonSerializer.Serialize(userCreatedEvent);
        var userCreatedEventAsByteArray = Encoding.UTF8.GetBytes(userCreatedEventAsJson);

        channel.BasicPublish("topic-exchange", "info.debug.critical", properties, userCreatedEventAsByteArray);
        channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(2));
    }
    catch (Exception ex)
    {

        Console.WriteLine(ex.ToString());
    }
  
});




Console.WriteLine("Mesaj gönderildi");

// See https://aka.ms/new-console-template for more information
using netMicroService;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

Console.WriteLine("Producer");

var connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("amqps://fpwimrxb:Ri5QFoYAbHhoyosnIUzpYODVKl59qNBp@shark.rmq.cloudamqp.com/fpwimrxb");
var connection = connectionFactory.CreateConnection();
//kuyruk ve mesajlar ayrı ayrı kalıı olmalı
var channel = connection.CreateModel();
channel.ConfirmSelect();

channel.ExchangeDeclare("topic-exchange",ExchangeType.Topic, true, false, null);

/*stringq kuyruk ismi, durable diske 
 * yazıldıysa tutar true yani değilse hepsi gider
 * , exclusive bir connectionda birden falza channel açmak istiyoruz false yaparsak
 * başka projeden görürüz
 * outodelete false olaak silinmemesi için
 * IDic kuyruk boyutu ve ömrü configrasyon yeri
 * kuyuk her çalışmada tekrar oluşmaz sadece konfgi değiştirirsen san uyarı verir
 */


Enumerable.Range(1, 100).ToList().ForEach(x =>
{
	try
	{
        var userCreatedEvent = new UserCreatedEvent() { Id = 1, UserName = "batuhan", Email = "mbt.@gmail.com" };
        var userCreatedEventAsJson = JsonSerializer.Serialize(userCreatedEvent);
        var userCreatedEventAsByteArray = Encoding.UTF8.GetBytes(userCreatedEventAsJson);
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true; // Rabbitmq sunucusu çökse bile artık diske yazılıyor mesajlar silinmiyor

        channel.BasicPublish("topic-exchange", "info.debug.critical", properties, userCreatedEventAsByteArray);
        channel.WaitForConfirms(TimeSpan.FromSeconds(2));
    }
    catch (Exception ex)
	{

        Console.WriteLine(ex.ToString());
	}
    
});

/*
 * overload metodlardan 4.olan 
 * rooutkey kuyruk ismi
 * body kısmı mesaj içeriği önce json byte olacak
 */


Console.WriteLine("Mesaj Gönderildi");

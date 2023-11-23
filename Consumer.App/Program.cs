// See https://aka.ms/new-console-template for more information
using Consumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

Console.WriteLine("Consumer");

var connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("amqps://fpwimrxb:Ri5QFoYAbHhoyosnIUzpYODVKl59qNBp@shark.rmq.cloudamqp.com/fpwimrxb");
var connection = connectionFactory.CreateConnection();

var channel = connection.CreateModel();

channel.BasicQos(0,20,false);



var consumer = new EventingBasicConsumer(channel);

channel.QueueDeclare("topic-queue", true, false, false, null);
channel.QueueBind("topic-queue", "topic-exchange", "*.warning.critical", null);

consumer.Received += (object? sender, BasicDeliverEventArgs? args) =>
{
        var userCreatedEventAsJson = Encoding.UTF8.GetString(args.Body.ToArray());
        var userCreatedEvent = JsonSerializer.Deserialize<UserCreatedEvent>(userCreatedEventAsJson);

        Console.WriteLine($"Gelen mesaj:id:{userCreatedEvent?.Id},name:{userCreatedEvent?.UserName},email:{userCreatedEvent.Email}");
		channel.BasicAck(args.DeliveryTag, true);//normalde rabbitmq tüm mesajları toplar öyle gönderir 10 20 gibi true demek önemli 

};
//eğer true verirsek rabbitq mesajjlar geldiği anda bizden hemen siler
//bazı eventlerin kaybolmasının önemli olmadığı durumlarda true olmalı
channel.BasicConsume("topic-queue", true, consumer);



Console.ReadLine();
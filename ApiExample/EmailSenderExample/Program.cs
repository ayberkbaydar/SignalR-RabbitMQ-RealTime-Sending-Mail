using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmailSenderExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HubConnection hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:5001/messagehub").Build();
            hubConnection.StartAsync();

            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("");//(AMQP URL)
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare("messagequeue", false, false, false);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("messagequeue", true, consumer);

            consumer.Received += async (s, e) =>
            {
                //Email operasyonları burada gerçekleşecek.
                //e.Body.Span

                string serializeData = Encoding.UTF8.GetString(e.Body.Span);
                User user = JsonSerializer.Deserialize<User>(serializeData); //apiden serialize edilmiş data geldiğinde apideki user'a karşılık gelen user'a deserialize ederek kullanmamız gerekiyor.
                EmailSender.Send(user.Email, user.Message);
                Console.WriteLine($"{user.Email} mail gönderilmiştir.");
                await hubConnection.InvokeAsync("SendMessageAsync", $"{user.Email} mail gönderilmiştir.");

            };

            Console.Read();
        }
    }
}
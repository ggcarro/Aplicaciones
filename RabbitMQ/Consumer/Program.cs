using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Definimos IP y creamos conexión
            var factory = new ConnectionFactory() { HostName = "10.115.1.81" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    bool durable = true;
                    channel.QueueDeclare("ColaTareas", durable, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        int num = Int32.Parse(message);
                        Console.WriteLine("Recibido {0}", num);
                        Thread.Sleep(num*1000);
                        channel.BasicAck(ea.DeliveryTag, false);
                    };
                    channel.BasicQos(0, 1, false);
                    channel.BasicConsume("ColaTareas", false, consumer);
                    Console.WriteLine("Press enter to exit");
                    Console.ReadLine();

                }
            }
        }
    }
}

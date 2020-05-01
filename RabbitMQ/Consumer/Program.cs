using System;
using System.Text;
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
                    channel.QueueDeclare("ColaAT", false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine("Recibido {0}", message);
                    };
                    channel.BasicConsume(queue: "ColaAT",
                                      autoAck: true,
                                      consumer: consumer);
                    Console.WriteLine("Press enter to exit");
                    Console.ReadLine();

                }
            }
        }
    }
}

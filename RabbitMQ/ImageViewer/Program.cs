using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using iVocabulary;

namespace ImageViewer
{
    class Program
    {
        const string IP = "10.115.1.81";
        const string BINDING_KEY = "Image.*";
        const string EXCHANGE = "Image";
        const string QUEUENAME = "iViewer";

        static void Main(string[] args)
        {
            // Definimos IP y creamos conexión
            var factory = new ConnectionFactory() { HostName = IP };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    // Se declara un intercambiador de tipo topic denominado "Alertas"
                    channel.ExchangeDeclare(EXCHANGE, "topic");

                    // Se crea la cola temporal
                    var queueName = channel.QueueDeclare(QUEUENAME, true, false, false, null);

                    // Se desean recibir todas las alertas
                    channel.QueueBind(QUEUENAME, EXCHANGE, BINDING_KEY);

                    BinaryImageCodec iCodec = new BinaryImageCodec();

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;

                        Image image = iCodec.Decode(body.ToArray());
                        if (ea.RoutingKey == "Image.Raw") { 
                            Console.WriteLine("Image Raw: {0} - {1}", image.Num, image.Text);
                        }
                        else
                        {
                            Console.Write("Imagen Final: {0} - {1}", image.Num, image.Text);
                        }
                    };
                    channel.BasicConsume(
                    queue: QUEUENAME,
                    autoAck: true,
                    consumer: consumer);

                    Console.WriteLine("Press enter to exit");
                    Console.ReadLine();

                }
            }
        }
    }
}

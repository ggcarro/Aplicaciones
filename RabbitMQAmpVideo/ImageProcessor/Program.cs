using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using iVocabulary;

namespace ImageProcessor
{
    class Program
    {
        const string IP = "127.0.0.1";
        const string BINDING_KEY = "Image.Raw";
        const string EXCHANGE = "Image";
        const string ROUTING_KEY = "ImageWorkQueue";
        const string QUEUENAME = "iProcessor";

        static void Main(string[] args)
        {
            Console.WriteLine("iProcessor");

            // Definimos IP y creamos conexión
            var factory = new ConnectionFactory() { HostName = IP };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // Se generan propiedades de persistencia
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

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

                        //Image image = iCodec.Decode(body.ToArray());

                        //byte[] bodySend = iCodec.Encode(image);

                        channel.BasicPublish(EXCHANGE, ROUTING_KEY, properties, body);

                        Console.WriteLine("Frame enviado a ImageWorkQueue");

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

using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using iVocabulary;

namespace ImageWorker
{
    class Program
    {
        const string IP = "10.115.1.81";
        const string BINDING_KEY = "ImageWorkQueue";
        const string EXCHANGE = "Image";
        const string ROUTING_KEY = "Image.Result";
        const string QUEUENAME = "iWorker";

        static void Main(string[] args)
        {
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

                        Image image = iCodec.Decode(body.ToArray());

                        // ESPACIO RESERVADO PARA EL PROCESAMIENTO - SIMULAMOS CON UN THREAD SLEEP (1s - 10s)
                        Random random = new Random();
                        Thread.Sleep(1000 * random.Next(1, 10));

                        byte[] bodySend = iCodec.Encode(image);

                        channel.BasicPublish(EXCHANGE, ROUTING_KEY, properties, bodySend);

                        Console.WriteLine("Imagen procesada y enviada a ImageViewer");

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

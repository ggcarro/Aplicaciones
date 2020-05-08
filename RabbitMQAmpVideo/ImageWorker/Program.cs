using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using iVocabulary;
using System.Threading;
using System.IO;

namespace ImageWorker
{
    class Program
    {
        const string IP = "127.0.0.1";
        const string BINDING_KEY = "ImageWorkQueue";
        const string EXCHANGE = "Image";
        const string ROUTING_KEY = "Image.Result";
        const string QUEUENAME = "iWorker";


        static void Main(string[] args)
        {
            Console.WriteLine("iWorker");

            // Definimos IP y creamos conexión
            var factory = new ConnectionFactory() { HostName = IP };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // Se generan propiedades de persistencia
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    // Se declara un intercambiador de tipo topic denominado "Image"
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

                        Console.WriteLine("Worker");

                        Image image = iCodec.Decode(body.ToArray());

                        // ESPACIO RESERVADO PARA EL PROCESAMIENTO - SIMULAMOS CON UN THREAD SLEEP (1s - 10s)
                        
                        FaceDetec face = new FaceDetec();
                        image.Mat = face.Detec(image.Mat);
                        
                        byte[] bodySend = iCodec.Encode(image);

                        channel.BasicPublish(EXCHANGE, ROUTING_KEY, properties, bodySend);

                        Console.WriteLine("Frame procesado y enviado a ImageViewer");


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

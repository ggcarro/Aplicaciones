using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using iVocabulary;
using System.IO;
using OpenCvSharp;


namespace ImageViewer
{
    class Program
    {
        const string IP = "10.115.1.169";
        const string BINDING_KEY = "Image.*";
        const string EXCHANGE = "Image";
        const string QUEUENAME = "iViewer";

        static void Main(string[] args)
        {
            Console.WriteLine("iViewer");

            // Definimos IP y creamos conexión
            var factory = new ConnectionFactory() { HostName = IP };
            using (var connection = factory.CreateConnection())
            {
                using (Window window = new Window("capture"))
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
                        var route = ea.RoutingKey;
                        Image image = iCodec.Decode(body.ToArray());

                        Console.WriteLine("RoutingKey: {0}", route);

                        if (route == "Image.Raw")
                        {
                            Console.WriteLine("Image Raw");

                            using (Mat mat = image.Mat)
                            {
                                window.ShowImage(mat);
                                Cv2.WaitKey(1000);
                                Console.WriteLine("Video ejecutado");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Imagen Final");
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

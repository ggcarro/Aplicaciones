using System;
using System.Text;
using RabbitMQ.Client;
using iVocabulary;
using OpenCvSharp;

namespace ImageProducer
{
    class Program
    {
        const string IP = "10.115.1.169";
        const string BINDING_KEY = "Image.Raw";
        const string EXCHANGE = "Image";

        static void Main(string[] args)
        {
            Console.WriteLine("iProducer");

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

                    BinaryImageCodec iCodec = new BinaryImageCodec();
                    int seq = 0;

                    VideoCapture capture = new VideoCapture(0);

                    int sleepTime = (int)Math.Round(500 / capture.Fps);  //Captura cada xtiempo

                    using (Window window = new Window("capture"))
                    using (Mat mat = new Mat()) // Frame image buffer
                    {
                        // When the movie playback reaches end, Mat.data becomes NULL.
                        while (true)
                        {
                            capture.Read(mat); // same as cvQueryFrame
                            if (mat.Empty())
                                break;

                            Image image = new Image(mat, seq);

                            byte[] body = iCodec.Encode(image);

                            // Se publica el mensaje
                            channel.BasicPublish(EXCHANGE, BINDING_KEY, properties, body);

                            Console.WriteLine("Enviada - Seq: {0}", seq);
                        }
                    }

                }
            }
        }
    }
}
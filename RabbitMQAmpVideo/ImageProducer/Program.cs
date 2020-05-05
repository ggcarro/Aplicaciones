using System;
using System.Text;
using RabbitMQ.Client;
using iVocabulary;

namespace ImageProducer
{
    class Program
    {
        const string IP = "10.115.1.81";
        const string BINDING_KEY = "Image.Raw";
        const string EXCHANGE = "Image";

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

                    BinaryImageCodec iCodec = new BinaryImageCodec();

                    string text = "/Users/gonzalo/Desktop/input.jpg";

                    Image image = new Image(text);
                    byte[] body = iCodec.Encode(image);

                    // Se publica el mensaje
                    channel.BasicPublish(EXCHANGE, BINDING_KEY, properties, body);

                    Console.WriteLine("Enviada");

                }
            }
        }
    }
}
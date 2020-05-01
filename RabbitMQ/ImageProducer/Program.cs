using System;
using System.Text;
using RabbitMQ.Client;
using iVocabulary;

namespace ImageProducer
{
    class Program
    {
        const string IP = "10.115.1.81";
        const string ROUTING_KEY = "Image.Raw";
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

                    int num = 0;
                    BinaryImageCodec iCodec = new BinaryImageCodec();

                    while (true)
                    {
                        Console.WriteLine("Introduce un mensaje");
                        Console.WriteLine("q para finalizar");
                        string text = Console.ReadLine();
                        if(text == "q") { break; }
                        num++;

                        Image image = new Image(text, num);
                        byte[] body = iCodec.Encode(image);

                        // Se publica el mensaje
                        channel.BasicPublish(EXCHANGE, ROUTING_KEY, properties, body);
                    }

                    Console.WriteLine("Enviado el numero: {0}", num);
                }
            }
        }
    }
}
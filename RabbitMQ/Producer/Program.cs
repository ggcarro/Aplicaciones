using System;
using System.Text;
using RabbitMQ.Client;

namespace Producer
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

                    string message = DateTime.Now.ToString() + " - Mensaje de prueba";
                    byte[] body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", "ColaAT", null, body);

                    Console.WriteLine("Enviado el mensaje: {0}", message);
                }
            }
        }
    }
}
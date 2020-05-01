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
                    bool durable = true;
                    channel.QueueDeclare("ColaTareas", durable, false, false, null);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    Random random = new Random();

                    for(int i =0;i<9; i++)
                    {
                        int numRandom = 1;
                        string message = numRandom.ToString();
                        byte[] body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish("", "ColaTareas", properties, body);
                        Console.WriteLine("Enviado el numero: {0}", numRandom);

                    }

                    int num = 10;
                    string mess = num.ToString();
                    byte[] bo = Encoding.UTF8.GetBytes(mess);

                    channel.BasicPublish("", "ColaTareas", null, bo);


                    Console.WriteLine("Enviado el numero: {0}", num);
                }
            }
        }
    }
}
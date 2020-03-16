using System;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        { 
            try
            {
                SFFSender sender = new SFFSender("127.0.0.1", 23456, "hola.c"); //pasamos el puerto por el constructor
                sender.Run();
                Console.WriteLine("Write something to END");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
        }
    }
}

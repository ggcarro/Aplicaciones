using System;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        { 
            try
            {
                Console.WriteLine("Args[0]: {0}, Args[1]: {1}, Args[2]: {2}", args[0], args[1], args[2]);
                SFFSender sender = new SFFSender("127.0.0.1", 23456, "C:/Users/UO258767/Desktop/FaviconMo.png"); //pasamos el puerto por el constructor
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

using System;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        { 
            try
            {
                SFFSender sender = new SFFSender("127.0.0.1", 23456, "C:/Users/UO258767/Desktop/FaviconMo.png"); //pasamos el puerto por el constructor
                sender.Run();
                Console.WriteLine("Transfer Finished. Introduce a Key to close the Sender Program");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
        }
    }
}

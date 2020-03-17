using System;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        { 
            try
            {
                SFFSender sender = new SFFSender("127.0.0.1", 23457, "C:/Users/UO258767/source/repos/Aplicaciones/SFF/Client/FaviconAV.png"); //pasamos el puerto por el constructor
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

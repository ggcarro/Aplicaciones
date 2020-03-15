using System;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SFFServer server = new SFFServer(23456); //pasamos el puerto por el constructor
                server.Run();
                Console.WriteLine("");
                Console.ReadKey(); 
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
        }
    }
}

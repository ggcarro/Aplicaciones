using System;
using System.Threading;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SFFReceiver receiver = new SFFReceiver(23456); //pasamos el puerto por el constructor
                receiver.Run();
                Console.WriteLine("Transfer Finished. Introduce a Key to close the Receiver program");
                Console.ReadKey(); 
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
        }
    }
}

using System;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        { 
            try
            {
                SFFSender sender = new SFFSender(args[0], Int32.Parse(args[1]), args[2]); //pasamos el puerto por el constructor
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

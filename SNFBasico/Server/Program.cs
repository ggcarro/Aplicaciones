using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SNFServer server = new SNFServer();
            server.Run();

        }
    }
}

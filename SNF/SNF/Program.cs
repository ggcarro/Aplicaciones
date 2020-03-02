using System;
using SNFVocabulary;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            SNFClient client = new SNFClient();
            client.Run();
            
        }
    }
}

using System;
using SNFVocabulary;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            
            SNFClient client = new SNFClient("C:/Users/UO258767/Desktop/cc.bin");
            client.Run();
            
        }
    }
}

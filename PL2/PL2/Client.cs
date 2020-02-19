using System;
using SNFVocabulary;

namespace PL2
{
    class Client
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Client Started");
            const int N = 100; // Suponemos que se quiere enviar este cantidad de números

            BinarySNFMessageCodec codec = new BinarySNFMessageCodec();
            SNFMessage message = null;

            for (int i =0; i < N; i++)
            {
                
            }
        }
    }
}

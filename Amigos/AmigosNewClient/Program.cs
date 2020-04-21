using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Amigos.Models;

namespace AmigosNewClient
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("Bienvenido al servicio amigos!\n");

            PrintList();
            AddFriend();
            PrintList();
        }

        static public void PrintList()
        {
            //Se imprime la lista de amigos
            HttpResponseMessage response = client.GetAsync("api/amigo").Result;
            if (response.IsSuccessStatusCode)
            {
                Amigo[] amigos = response.Content.ReadAsAsync<Amigo[]>().Result;
                foreach (Amigo amigo in amigos)
                    Console.WriteLine("{0}: {1}", amigo.name, amigo.ID);
            }
        }

        static public void ShowFriend()
        {
            // Se imprime 1 amigo en concreto
            HttpResponseMessage response = client.GetAsync("api/amigo/1").Result;
            if (response.IsSuccessStatusCode)
            {
                Amigo amigo = response.Content.ReadAsAsync<Amigo>().Result;
                Console.WriteLine("{0}: {1}", amigo.name, amigo.ID);
            }
        }

        static public void AddFriend()
        {
            // Se crea un nuevo amigo
            Amigo amigo = new Amigo()
            {
                name = "Rubén",
                longi = "123",
                lati = "456"
            };
            HttpResponseMessage response = client.PostAsJsonAsync("api/amigo", amigo).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Amigo creado con éxito. URL: {0}", response.Headers.Location);
            }
            else
            {
                Console.WriteLine("Ha ocurrido un eror");
            }

        }

        static public void ModifyFriend(int id)
        {
            // Se modifica un amigo

            HttpResponseMessage response = client.GetAsync("api/amigo").Result;
            Amigo amigo = new Amigo();
            Amigo[] amigos = response.Content.ReadAsAsync<Amigo[]>().Result;

            foreach (Amigo findAmigo in amigos){    //Se encuentra al amigo que se quiere modificar
                if(findAmigo.ID == id)
                {
                    amigo.ID = 1;
                    amigo.name = "RUF";
                }
            }
            
            response = client.PutAsJsonAsync("api/amigo/" + id, amigo).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Amigo modificado con éxito.");
            }
            else
            {
                Console.WriteLine("Ha ocurrido un eror.");
            }
         
        }

        static public void EliminateFriend(int id)
        { 
            //Elimina a un amigo
            HttpResponseMessage response = client.DeleteAsync("api/amigo/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Amigo borrado con éxito.");
            }

        }
    }
}

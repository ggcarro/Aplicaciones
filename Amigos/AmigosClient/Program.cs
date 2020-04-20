using Amigos.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AmigosClient
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            //Configuración del cliente
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("Bienvenido Carlos al cliente de consola de Teleamigos");
            string value = "10";
            do
            {
                Console.WriteLine("\n Si desea mostrar la lista de amigos introduce 1" +
                    "\n Si lo que deseas es añadir un amigo para que te haga compañia en la cuarentena intoduce 2" +
                    "\n En caso de querer editar a algun amigo introduce 3" +
                    "\n Si lo que deseas es eliminar a algun amigo por pesado introduce 4" +
                    "\n Y si solo quieres terminar con esto y jugar a la play introduce cualquier otro numero");
                value = Console.ReadLine();
                switch (value)
                {
                    case "1":
                        ListAmigos();
                        break;
                    default:
                        value="0";
                        break;
                }
            } while (value != "0");

        }

        static public void ListAmigos()
        {
            Console.WriteLine("A M I G O S");
            HttpResponseMessage response = client.GetAsync("api/amigo").Result;
            Console.WriteLine("Status Code: {0}", response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                Amigo[] amigos = response.Content.ReadAsAsync<Amigo[]>().Result;
                foreach (Amigo amigo in amigos)
                    Console.WriteLine("{0}: {1}", amigo.name, amigo.ID);
            }
            else
            {
                Console.WriteLine("Hubo un problema al crear el amigo");
            }
        }

        static public void NewAmigo(string name, string longi, string lati)
        {
            Amigo amigo = new Amigo();
            amigo.name = name;
            amigo.longi = longi;
            amigo.lati = lati;

            HttpResponseMessage response = client.PostAsJsonAsync("api/amigo", amigo).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Amigo creado correctamente");
            }
            else
            {
                Console.WriteLine("Hubo un problema al crear el amigo");
            }
        }

        static public void ModifyAmigo(int id, string name, string longi, string lati)
        {
            Amigo amigo = new Amigo();
            amigo.ID = id;
            amigo.name = name;
            amigo.longi = longi;
            amigo.lati = lati;

            HttpResponseMessage response = client.PostAsJsonAsync("api/amigo"+id, amigo).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Amigo modificado correctamente");
            }
            else
            {
                Console.WriteLine("Hubo un problema al modificar el amigo");
            }
        }


        static public void DeleteAmigo(int id)
        {
            HttpResponseMessage response = client.DeleteAsync("api/amigo/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Amigo borrado correctamente.");
            }
            else
            {
                Console.WriteLine("Hubo un problema al eliminar el amigo");
            }
        }
    }
}

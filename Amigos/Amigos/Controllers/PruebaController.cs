using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Amigos.Controllers
{
    public class PruebaController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public string Hola(string valor, int veces)
        {
            string str = "";
            for(int i=0; i<veces; i++)
            {
                str = str + " " + valor;
            }
            return str;
        }
        public IActionResult Adios(string valor, int veces)
        {
            ViewBag.valor = valor;
            ViewBag.valor = veces;

            return View();

        }
    }
}

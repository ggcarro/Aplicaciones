using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Amigos.Controllers
{
    public class PruebaController : Controller
    {
        private readonly IInc _inc;

        public PruebaController(IInc inc)
        {
            _inc = inc;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Adios(string valor, int veces)
        {
            ViewBag.valor = valor;
            ViewBag.veces = veces;
            return View();
        }

        public string hola(string valor, int veces)
        {
            string repeated = "";
            for (int i = 0; i < veces; i++)
            {
                repeated += valor;
            }
            return repeated;
        }

    }
}
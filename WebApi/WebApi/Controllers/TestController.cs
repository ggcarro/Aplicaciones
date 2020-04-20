using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Cadena retornada por get";
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            return id;
        }
        
        [HttpPost]
        public string PostJsonString([FromBody]Dummy d)
        {
            if (!ModelState.IsValid)
                return "El modelo no es válido";
            return "I:" + d.i + " S:" + d.s + " D:" + d.d;
        }

    [HttpPut]
        public ActionResult<string> Put()
        {
            return "Método Put invocado";
        }

        [HttpDelete]
        public ActionResult<string> Delete()
        {
            return "Método Delete invocado";
        }
    }
}
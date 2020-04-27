using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Progeto.Lua;
using Progeto.API.Models;

namespace Progeto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgetoController : ControllerBase
    {
        LuaInterpreter interpreter = new LuaInterpreter();

        [HttpPost]
        public Answer Post([FromBody] ProgetoModel program)
        {
            // Iniciamos temporizador 
            Stopwatch temp = new Stopwatch();
            temp.Start();
            string[] intResult = interpreter.RunProgram(program.code).ToArray();
            temp.Stop();
            return new Answer(intResult[0], intResult[1], temp.Elapsed.TotalMilliseconds.ToString());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Progeto.Lua;
using Progeto.API.Models;
using System.IO;

namespace Progeto.API.Controllers
{
    [Route("api/save")]
    [ApiController]
    public class SaveController : ControllerBase
    {
        LuaInterpreter interpreter = new LuaInterpreter();

        [HttpPost]
        public Answer Save([FromBody] ProgetoModel program)
        {
            // Iniciamos temporizador 
            Stopwatch temp = new Stopwatch();
            temp.Start();
            string[] intResult = interpreter.RunProgram(program.code).ToArray();
            temp.Stop();
            string path = "/Users/gonzalo/Desktop/Server/file0.txt";
            int i = 1;
            while (System.IO.File.Exists(path))
            {
                path= "/Users/gonzalo/Desktop/Server/file"+i+".txt";
                i++;
            }
            System.IO.File.WriteAllText(path, program.code);

            return new Answer(intResult[0], intResult[1], temp.Elapsed.TotalMilliseconds.ToString());
        }
    }

}
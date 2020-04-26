using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Progeto.Lua;

namespace Progeto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgetoAPIController : ControllerBase
    {
        LuaInterpreter interpreter = new LuaInterpreter();
    }
}
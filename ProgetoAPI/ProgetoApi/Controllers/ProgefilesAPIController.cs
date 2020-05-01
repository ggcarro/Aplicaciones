using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Progeto.API.Models;
using Progeto.Lua;
using ProgetoApi.DataAccessLayer;
using ProgetoApi.Models;

namespace ProgetoApi.Controllers.API
{
   [Route("api/progefiles")]
   [ApiController]
   public class ProgefileController: ControllerBase
    {
        private readonly ProgefileDBContext _context;
        public ProgefileController(ProgefileDBContext context)
        {
            _context = context;
        }

        // GET: api/progefiles
        [HttpGet]
        public IEnumerable<Progefile> GetFiles()
        {
            return _context.Files;

        }

        // GET: api/progefile/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAmigo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pfile = await _context.Files.FindAsync(id);

            if (pfile == null)
            {
                return NotFound();
            }

            return Ok(pfile);
        }


        // POST: api/progefiles
        [HttpPost]
        public async Task<IActionResult> PostFile([FromBody] Progefile progefile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Files.Add(progefile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAmigo", new { id = progefile.ID }, progefile);
        }

        
    }
}
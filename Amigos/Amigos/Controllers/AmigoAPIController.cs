using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Amigos.DataAccessLayer;
using Amigos.Models;

namespace Amigos.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmigoController : ControllerBase
    {
        private readonly AmigoDBContext _context;

        public AmigoController(AmigoDBContext context)
        {
            _context = context;
        }

        // GET: api/Amigo
        [HttpGet]
        public IEnumerable<Amigo> GetAmigos()
        {
            return _context.Amigos;
        }

        // GET: api/Amigo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAmigo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var amigo = await _context.Amigos.FindAsync(id);

            if (amigo == null)
            {
                return NotFound();
            }

            return Ok(amigo);
        }

        // PUT: api/Amigo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmigo([FromRoute] int id, [FromBody] Amigo amigo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != amigo.ID)
            {
                return BadRequest();
            }

            _context.Entry(amigo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmigoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Amigo
        [HttpPost]
        public async Task<IActionResult> PostAmigo([FromBody] Amigo amigo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Amigos.Add(amigo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAmigo", new { id = amigo.ID }, amigo);
        }

        // DELETE: api/Amigo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmigo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var amigo = await _context.Amigos.FindAsync(id);
            if (amigo == null)
            {
                return NotFound();
            }

            _context.Amigos.Remove(amigo);
            await _context.SaveChangesAsync();

            return Ok(amigo);
        }

        private bool AmigoExists(int id)
        {
            return _context.Amigos.Any(e => e.ID == id);
        }
    }
}
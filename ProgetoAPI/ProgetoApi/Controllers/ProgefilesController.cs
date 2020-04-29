using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProgetoApi.DataAccessLayer;
using ProgetoApi.Models;

namespace ProgetoApi.Controllers
{
    public class ProgefilesController : Controller
    {
        private readonly ProgefileDBContext _context;

        public ProgefilesController(ProgefileDBContext context)
        {
            _context = context;
        }

        // GET: Progefiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Files.ToListAsync());
        }

        // GET: Progefiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progefile = await _context.Files
                .FirstOrDefaultAsync(m => m.ID == id);
            if (progefile == null)
            {
                return NotFound();
            }

            return View(progefile);
        }

        // GET: Progefiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Progefiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,code,path,filename")] Progefile progefile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(progefile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(progefile);
        }

        
        // POST: Progefiles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,code,path,filename")] Progefile progefile)
        {
            if (id != progefile.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(progefile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgefileExists(progefile.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(progefile);
        }

        // GET: Progefiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progefile = await _context.Files
                .FirstOrDefaultAsync(m => m.ID == id);
            if (progefile == null)
            {
                return NotFound();
            }

            return View(progefile);
        }

        // POST: Progefiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var progefile = await _context.Files.FindAsync(id);
            _context.Files.Remove(progefile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgefileExists(int id)
        {
            return _context.Files.Any(e => e.ID == id);
        }
    }
}

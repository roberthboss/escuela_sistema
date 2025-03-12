using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EscuelaAppSimbana.Controllers
{
    [Authorize]
    public class ClasesController : Controller
    {
        private readonly AppDbContext _context;

        public ClasesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Clases
        public async Task<IActionResult> Index()
        {
            var clases = await _context.clases
                .Include(c => c.profesor)
                .ToListAsync();

            return View(clases);
        }


        // GET: Clases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var clase = await _context.clases
                .Include(c => c.profesor)
                .FirstOrDefaultAsync(m => m.clase_id == id);

            if (clase == null) return NotFound();

            return View(clase);
        }

        // GET: Clases/Create
        public IActionResult Create()
        {
            var listaProfesores = _context.profesores
                .Select(p => new {
                    p.profesor_id,
                    NombreCompleto = p.nombre + " " + p.apellido
                })
                .ToList();

            ViewData["profesor_id"] = new SelectList(listaProfesores, "profesor_id", "NombreCompleto");
            return View();
        }

        // POST: Clases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("clase_id,nombre_clase,profesor_id")] Clase clase)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(clase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var listaProfesores = _context.profesores
                .Select(p => new {
                    p.profesor_id,
                    NombreCompleto = p.nombre + " " + p.apellido
                })
                .ToList();
            ViewData["profesor_id"] = new SelectList(listaProfesores, "profesor_id", "NombreCompleto", clase.profesor_id);

            return View(clase);
        }

        // GET: Clases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var clase = await _context.clases
                .Include(c => c.profesor)
                .FirstOrDefaultAsync(c => c.clase_id == id);

            if (clase == null) return NotFound();

            var listaProfesores = _context.profesores
                .Select(p => new {
                    p.profesor_id,
                    NombreCompleto = p.nombre + " " + p.apellido
                })
                .ToList();

            ViewData["profesor_id"] = new SelectList(listaProfesores, "profesor_id", "NombreCompleto", clase.profesor_id);

            return View(clase);
        }

        // POST: Clases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("clase_id,nombre_clase,profesor_id")] Clase clase)
        {
            if (id != clase.clase_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaseExists(clase.clase_id))
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
            var listaProfesores = _context.profesores
                .Select(p => new {
                    p.profesor_id,
                    NombreCompleto = p.nombre + " " + p.apellido
                })
                .ToList();
            ViewData["profesor_id"] = new SelectList(listaProfesores, "profesor_id", "NombreCompleto", clase.profesor_id);

            return View(clase);
        }

        // GET: Clases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clase = await _context.clases
                .Include(c => c.profesor)
                .FirstOrDefaultAsync(m => m.clase_id == id);
            if (clase == null)
            {
                return NotFound();
            }

            return View(clase);
        }

        // POST: Clases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clase = await _context.clases.FindAsync(id);
            if (clase != null)
            {
                _context.clases.Remove(clase);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ClaseExists(int id)
        {
            return _context.clases.Any(e => e.clase_id == id);
        }
    }
}

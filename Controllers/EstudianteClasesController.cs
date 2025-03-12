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
    public class EstudianteClasesController : Controller
    {
        private readonly AppDbContext _context;

        public EstudianteClasesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EstudianteClases
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.estudiante_clases
                .Include(ec => ec.estudiante)
                .Include(ec => ec.clase);

            return View(await appDbContext.ToListAsync());
        }

        // GET: EstudianteClases/Details
        public async Task<IActionResult> Details(int? estudiante_id, int? clase_id)
        {
            if (estudiante_id == null || clase_id == null)
            {
                return NotFound();
            }

            var estudianteClase = await _context.estudiante_clases
                .Include(e => e.clase)
                .Include(e => e.estudiante)
                .FirstOrDefaultAsync(m => m.estudiante_id == estudiante_id && m.clase_id == clase_id);

            if (estudianteClase == null)
            {
                return NotFound();
            }

            return View(estudianteClase);
        }

        // GET: EstudianteClases/Create
        public IActionResult Create()
        {
            ViewData["clase_id"] = new SelectList(_context.clases, "clase_id", "nombre_clase");

            var listaEstudiantes = _context.estudiantes
                .Select(e => new {
                    e.estudiante_id,
                    NombreCompleto = e.nombre + " " + e.apellido
                })
                .ToList();
            ViewData["estudiante_id"] = new SelectList(listaEstudiantes, "estudiante_id", "NombreCompleto");

            return View();
        }

        // POST: EstudianteClases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("estudiante_id,clase_id")] EstudianteClase estudianteClase)
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
                _context.Add(estudianteClase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["clase_id"] = new SelectList(_context.clases, "clase_id", "nombre_clase", estudianteClase.clase_id);
            var listaEstudiantes = _context.estudiantes
                .Select(e => new {
                    e.estudiante_id,
                    NombreCompleto = e.nombre + " " + e.apellido
                })
                .ToList();
            ViewData["estudiante_id"] = new SelectList(listaEstudiantes, "estudiante_id", "NombreCompleto", estudianteClase.estudiante_id);

            return View(estudianteClase);
        }

        // GET: EstudianteClases/Edit
        public async Task<IActionResult> Edit(int? estudiante_id, int? clase_id)
        {
            if (estudiante_id == null || clase_id == null)
            {
                return NotFound();
            }

            var estudianteClase = await _context.estudiante_clases.FindAsync(estudiante_id, clase_id);
            if (estudianteClase == null)
            {
                return NotFound();
            }

            ViewData["clase_id"] = new SelectList(_context.clases, "clase_id", "nombre_clase", estudianteClase.clase_id);

            var listaEstudiantes = _context.estudiantes
                .Select(e => new {
                    e.estudiante_id,
                    NombreCompleto = e.nombre + " " + e.apellido
                })
                .ToList();
            ViewData["estudiante_id"] = new SelectList(listaEstudiantes, "estudiante_id", "NombreCompleto", estudianteClase.estudiante_id);

            return View(estudianteClase);
        }

        // POST: EstudianteClases/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int estudiante_id, int clase_id, [Bind("estudiante_id,clase_id")] EstudianteClase estudianteClase)
        {
            if (estudiante_id != estudianteClase.estudiante_id || clase_id != estudianteClase.clase_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudianteClase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteClaseExists(estudianteClase.estudiante_id, estudianteClase.clase_id))
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

            ViewData["clase_id"] = new SelectList(_context.clases, "clase_id", "nombre_clase", estudianteClase.clase_id);
            var listaEstudiantes = _context.estudiantes
                .Select(e => new {
                    e.estudiante_id,
                    NombreCompleto = e.nombre + " " + e.apellido
                })
                .ToList();
            ViewData["estudiante_id"] = new SelectList(listaEstudiantes, "estudiante_id", "NombreCompleto", estudianteClase.estudiante_id);

            return View(estudianteClase);
        }

        // GET: EstudianteClases/Delete
        public async Task<IActionResult> Delete(int? estudiante_id, int? clase_id)
        {
            if (estudiante_id == null || clase_id == null)
            {
                return NotFound();
            }

            var estudianteClase = await _context.estudiante_clases
                .Include(e => e.clase)
                .Include(e => e.estudiante)
                .FirstOrDefaultAsync(m => m.estudiante_id == estudiante_id && m.clase_id == clase_id);

            if (estudianteClase == null)
            {
                return NotFound();
            }

            return View(estudianteClase);
        }

        // POST: EstudianteClases/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int estudiante_id, int clase_id)
        {
            var estudianteClase = await _context.estudiante_clases.FindAsync(estudiante_id, clase_id);
            if (estudianteClase != null)
            {
                _context.estudiante_clases.Remove(estudianteClase);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteClaseExists(int estudiante_id, int clase_id)
        {
            return _context.estudiante_clases.Any(e => e.estudiante_id == estudiante_id && e.clase_id == clase_id);
        }
    }
}

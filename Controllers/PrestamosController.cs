using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiblioCampus.Models;

namespace BiblioCampus.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly BiblioCampusDbContext _context;

        public PrestamosController(BiblioCampusDbContext context)
        {
            _context = context;
        }

        // GET: Prestamos
        public async Task<IActionResult> Index()
        {
            var prestamos = await _context.Prestamos
                .Include(p => p.Estudiante)
                .Include(p => p.Libro)
                    .ThenInclude(l => l.Autor)
                .OrderByDescending(p => p.FechaPrestamo)
                .ThenByDescending(p => p.Id)
                .ToListAsync();
            return View(prestamos);
        }

        // GET: Prestamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var prestamo = await _context.Prestamos
                .Include(p => p.Estudiante)
                .Include(p => p.Libro)
                    .ThenInclude(l => l.Autor)
                .Include(p => p.Libro)
                    .ThenInclude(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prestamo == null) return NotFound();

            return View(prestamo);
        }

        // GET: Prestamos/Create
        public async Task<IActionResult> Create()
        {
            await CargarListasRelacionadasAsync();
            return View(new Prestamo { FechaPrestamo = DateTime.Today, Estado = "Activo" });
        }

        // POST: Prestamos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaPrestamo,Estado,EstudianteId,LibroId")] Prestamo prestamo)
        {
            ModelState.Remove("Estudiante");
            ModelState.Remove("Libro");

            if (ModelState.IsValid)
            {
                _context.Add(prestamo);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Préstamo registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarListasRelacionadasAsync(prestamo.EstudianteId, prestamo.LibroId, prestamo.Estado);
            return View(prestamo);
        }

        // GET: Prestamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null) return NotFound();

            await CargarListasRelacionadasAsync(prestamo.EstudianteId, prestamo.LibroId, prestamo.Estado);
            return View(prestamo);
        }

        // POST: Prestamos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaPrestamo,Estado,EstudianteId,LibroId")] Prestamo prestamo)
        {
            if (id != prestamo.Id) return NotFound();

            ModelState.Remove("Estudiante");
            ModelState.Remove("Libro");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = $"Préstamo #{prestamo.Id} actualizado correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await CargarListasRelacionadasAsync(prestamo.EstudianteId, prestamo.LibroId, prestamo.Estado);
            return View(prestamo);
        }

        // GET: Prestamos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var prestamo = await _context.Prestamos
                .Include(p => p.Estudiante)
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prestamo == null) return NotFound();

            return View(prestamo);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
                await _context.SaveChangesAsync();
                TempData["Exito"] = $"Préstamo #{id} eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarListasRelacionadasAsync(int? estudianteId = null, int? libroId = null, string estado = "Activo")
        {
            var estudiantes = await _context.Estudiantes
                .OrderBy(e => e.NombreCompleto)
                .ToListAsync();

            var libros = await _context.Libros
                .OrderBy(l => l.Titulo)
                .ToListAsync();

            ViewData["EstudianteId"] = new SelectList(estudiantes, "Id", "NombreCompleto", estudianteId);
            ViewData["LibroId"] = new SelectList(libros, "Id", "Titulo", libroId);
            ViewData["Estados"] = new SelectList(new[] { "Activo", "Devuelto", "Atrasado" }, estado);
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.Id == id);
        }
    }
}

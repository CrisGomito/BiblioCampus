using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiblioCampus.Models;

namespace BiblioCampus.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly BiblioCampusDbContext _context;

        public EstudiantesController(BiblioCampusDbContext context)
        {
            _context = context;
        }

        // GET: Estudiantes
        public async Task<IActionResult> Index()
        {
            var estudiantes = await _context.Estudiantes
                .Include(e => e.Prestamos)
                .OrderBy(e => e.NombreCompleto)
                .ToListAsync();
            return View(estudiantes);
        }

        // GET: Estudiantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var estudiante = await _context.Estudiantes
                .Include(e => e.Prestamos)
                    .ThenInclude(p => p.Libro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (estudiante == null) return NotFound();

            return View(estudiante);
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estudiantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cedula,NombreCompleto,CorreoElectronico,Carrera")] Estudiante estudiante)
        {
            ModelState.Remove("Prestamos");

            if (ModelState.IsValid)
            {
                if (await _context.Estudiantes.AnyAsync(e => e.Cedula == estudiante.Cedula))
                {
                    ModelState.AddModelError("Cedula", "Ya existe un estudiante registrado con esta cédula.");
                    return View(estudiante);
                }

                if (await _context.Estudiantes.AnyAsync(e => e.CorreoElectronico == estudiante.CorreoElectronico))
                {
                    ModelState.AddModelError("CorreoElectronico", "Ya existe un estudiante registrado con este correo electrónico.");
                    return View(estudiante);
                }

                _context.Add(estudiante);
                await _context.SaveChangesAsync();
                TempData["Exito"] = $"Estudiante '{estudiante.NombreCompleto}' registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null) return NotFound();

            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cedula,NombreCompleto,CorreoElectronico,Carrera")] Estudiante estudiante)
        {
            if (id != estudiante.Id) return NotFound();

            ModelState.Remove("Prestamos");

            if (ModelState.IsValid)
            {
                if (await _context.Estudiantes.AnyAsync(e => e.Cedula == estudiante.Cedula && e.Id != estudiante.Id))
                {
                    ModelState.AddModelError("Cedula", "Ya existe otro estudiante con esta cédula.");
                    return View(estudiante);
                }

                if (await _context.Estudiantes.AnyAsync(e => e.CorreoElectronico == estudiante.CorreoElectronico && e.Id != estudiante.Id))
                {
                    ModelState.AddModelError("CorreoElectronico", "Ya existe otro estudiante con este correo electrónico.");
                    return View(estudiante);
                }

                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = $"Estudiante '{estudiante.NombreCompleto}' actualizado correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        // GET: Estudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var estudiante = await _context.Estudiantes
                .Include(e => e.Prestamos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (estudiante == null) return NotFound();

            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.Prestamos)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (estudiante == null) return RedirectToAction(nameof(Index));

            if (estudiante.Prestamos.Any())
            {
                TempData["Error"] = $"No se puede eliminar al estudiante '{estudiante.NombreCompleto}' porque registra {estudiante.Prestamos.Count} préstamo(s).";
                return RedirectToAction(nameof(Index));
            }

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            TempData["Exito"] = $"Estudiante '{estudiante.NombreCompleto}' eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiantes.Any(e => e.Id == id);
        }
    }
}

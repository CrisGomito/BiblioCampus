using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiblioCampus.Models;

namespace BiblioCampus.Controllers
{
    public class AutoresController : Controller
    {
        private readonly BiblioCampusDbContext _context;

        public AutoresController(BiblioCampusDbContext context)
        {
            _context = context;
        }

        // GET: Autores
        public async Task<IActionResult> Index()
        {
            var autores = await _context.Autores
                .Include(a => a.Libros)
                .OrderBy(a => a.Apellidos)
                .ThenBy(a => a.Nombres)
                .ToListAsync();
            return View(autores);
        }

        // GET: Autores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var autor = await _context.Autores
                .Include(a => a.Libros)
                    .ThenInclude(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (autor == null) return NotFound();

            return View(autor);
        }

        // GET: Autores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombres,Apellidos,Nacionalidad,CorreoContacto")] Autor autor)
        {
            ModelState.Remove("Libros");

            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                TempData["Exito"] = $"Autor '{autor.NombreCompleto}' registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        // GET: Autores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var autor = await _context.Autores.FindAsync(id);
            if (autor == null) return NotFound();

            return View(autor);
        }

        // POST: Autores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombres,Apellidos,Nacionalidad,CorreoContacto")] Autor autor)
        {
            if (id != autor.Id) return NotFound();

            ModelState.Remove("Libros");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = $"Autor '{autor.NombreCompleto}' actualizado correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        // GET: Autores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var autor = await _context.Autores
                .Include(a => a.Libros)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (autor == null) return NotFound();

            return View(autor);
        }

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autores
                .Include(a => a.Libros)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (autor == null) return RedirectToAction(nameof(Index));

            if (autor.Libros.Any())
            {
                TempData["Error"] = $"No se puede eliminar al autor '{autor.NombreCompleto}' porque posee {autor.Libros.Count} libro(s) registrado(s).";
                return RedirectToAction(nameof(Index));
            }

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
            TempData["Exito"] = $"Autor '{autor.NombreCompleto}' eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _context.Autores.Any(e => e.Id == id);
        }
    }
}

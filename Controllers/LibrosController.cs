using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiblioCampus.Models;

namespace BiblioCampus.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BiblioCampusDbContext _context;

        public LibrosController(BiblioCampusDbContext context)
        {
            _context = context;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var libros = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Include(l => l.Prestamos)
                .OrderBy(l => l.Titulo)
                .ToListAsync();
            return View(libros);
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var libro = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Include(l => l.Prestamos)
                    .ThenInclude(p => p.Estudiante)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libro == null) return NotFound();

            return View(libro);
        }

        // GET: Libros/Create
        public async Task<IActionResult> Create()
        {
            await CargarListasRelacionadasAsync();
            return View();
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Isbn,AutorId,CategoriaId")] Libro libro)
        {
            ModelState.Remove("Autor");
            ModelState.Remove("Categoria");
            ModelState.Remove("Prestamos");

            if (ModelState.IsValid)
            {
                bool isbnDuplicado = await _context.Libros.AnyAsync(l => l.Isbn == libro.Isbn);
                if (isbnDuplicado)
                {
                    ModelState.AddModelError("Isbn", "Ya existe un libro registrado con ese código ISBN.");
                    await CargarListasRelacionadasAsync(libro.AutorId, libro.CategoriaId);
                    return View(libro);
                }

                _context.Add(libro);
                await _context.SaveChangesAsync();
                TempData["Exito"] = $"Libro '{libro.Titulo}' registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarListasRelacionadasAsync(libro.AutorId, libro.CategoriaId);
            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();

            await CargarListasRelacionadasAsync(libro.AutorId, libro.CategoriaId);
            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Isbn,AutorId,CategoriaId")] Libro libro)
        {
            if (id != libro.Id) return NotFound();

            ModelState.Remove("Autor");
            ModelState.Remove("Categoria");
            ModelState.Remove("Prestamos");

            if (ModelState.IsValid)
            {
                bool isbnDuplicado = await _context.Libros
                    .AnyAsync(l => l.Isbn == libro.Isbn && l.Id != libro.Id);
                if (isbnDuplicado)
                {
                    ModelState.AddModelError("Isbn", "Ya existe otro libro con ese código ISBN.");
                    await CargarListasRelacionadasAsync(libro.AutorId, libro.CategoriaId);
                    return View(libro);
                }

                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = $"Libro '{libro.Titulo}' actualizado correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await CargarListasRelacionadasAsync(libro.AutorId, libro.CategoriaId);
            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var libro = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Include(l => l.Prestamos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libro == null) return NotFound();

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros
                .Include(l => l.Prestamos)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (libro == null) return RedirectToAction(nameof(Index));

            if (libro.Prestamos.Any())
            {
                TempData["Error"] = $"No se puede eliminar '{libro.Titulo}' porque tiene {libro.Prestamos.Count} préstamo(s) en el historial.";
                return RedirectToAction(nameof(Index));
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
            TempData["Exito"] = $"Libro '{libro.Titulo}' eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarListasRelacionadasAsync(int? autorId = null, int? categoriaId = null)
        {
            var autores = await _context.Autores
                .OrderBy(a => a.Apellidos)
                .ThenBy(a => a.Nombres)
                .ToListAsync();

            var categorias = await _context.Categorias
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            ViewData["AutorId"] = new SelectList(autores, "Id", "NombreCompleto", autorId);
            ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Nombre", categoriaId);
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}

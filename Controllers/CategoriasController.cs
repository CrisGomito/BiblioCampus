using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiblioCampus.Models;

namespace BiblioCampus.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly BiblioCampusDbContext _context;

        public CategoriasController(BiblioCampusDbContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias
                .Include(c => c.Libros)
                .OrderBy(c => c.Codigo)
                .ToListAsync();
            return View(categorias);
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .Include(c => c.Libros)
                    .ThenInclude(l => l.Autor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Nombre,Descripcion,UbicacionEstante")] Categoria categoria)
        {
            ModelState.Remove("Libros");

            if (ModelState.IsValid)
            {
                bool codigoDuplicado = await _context.Categorias.AnyAsync(c => c.Codigo == categoria.Codigo);
                if (codigoDuplicado)
                {
                    ModelState.AddModelError("Codigo", "Ya existe una categoría registrada con este código.");
                    return View(categoria);
                }

                _context.Add(categoria);
                await _context.SaveChangesAsync();
                TempData["Exito"] = $"Categoría '{categoria.Nombre}' creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nombre,Descripcion,UbicacionEstante")] Categoria categoria)
        {
            if (id != categoria.Id) return NotFound();

            ModelState.Remove("Libros");

            if (ModelState.IsValid)
            {
                bool codigoDuplicado = await _context.Categorias
                    .AnyAsync(c => c.Codigo == categoria.Codigo && c.Id != categoria.Id);
                if (codigoDuplicado)
                {
                    ModelState.AddModelError("Codigo", "Ya existe otra categoría con este código.");
                    return View(categoria);
                }

                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = $"Categoría '{categoria.Nombre}' actualizada correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .Include(c => c.Libros)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Libros)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null) return RedirectToAction(nameof(Index));

            if (categoria.Libros.Any())
            {
                TempData["Error"] = $"No se puede eliminar la categoría '{categoria.Nombre}' porque tiene {categoria.Libros.Count} libro(s) asociado(s).";
                return RedirectToAction(nameof(Index));
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            TempData["Exito"] = $"Categoría '{categoria.Nombre}' eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}

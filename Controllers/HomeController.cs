using System.Diagnostics;
using BiblioCampus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiblioCampus.Controllers
{
    public class HomeController : Controller
    {
        private readonly BiblioCampusDbContext _context;

        public HomeController(BiblioCampusDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalCategorias = await _context.Categorias.CountAsync();
            ViewBag.TotalAutores = await _context.Autores.CountAsync();
            ViewBag.TotalLibros = await _context.Libros.CountAsync();
            ViewBag.TotalEstudiantes = await _context.Estudiantes.CountAsync();
            ViewBag.TotalPrestamos = await _context.Prestamos.CountAsync();
            ViewBag.PrestamosActivos = await _context.Prestamos.CountAsync(p => p.Estado == "Activo");

            var recientes = await _context.Prestamos
                .Include(p => p.Estudiante)
                .Include(p => p.Libro)
                    .ThenInclude(l => l.Autor)
                .OrderByDescending(p => p.FechaPrestamo)
                .ThenByDescending(p => p.Id)
                .Take(6)
                .ToListAsync();

            return View(recientes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

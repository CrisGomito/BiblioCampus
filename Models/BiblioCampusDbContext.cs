using Microsoft.EntityFrameworkCore;

namespace BiblioCampus.Models
{
    /// <summary>
    /// Contexto de Entity Framework Core para SQL Server (BiblioCampusDB).
    /// Administra las 5 entidades relacionadas y sus reglas de integridad referencial.
    /// </summary>
    public class BiblioCampusDbContext : DbContext
    {
        public BiblioCampusDbContext(DbContextOptions<BiblioCampusDbContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índices únicos para garantizar integridad de datos
            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Codigo)
                .IsUnique();

            modelBuilder.Entity<Libro>()
                .HasIndex(l => l.Isbn)
                .IsUnique();

            modelBuilder.Entity<Estudiante>()
                .HasIndex(e => e.Cedula)
                .IsUnique();

            modelBuilder.Entity<Estudiante>()
                .HasIndex(e => e.CorreoElectronico)
                .IsUnique();

            // Relaciones explícitas con DeleteBehavior.Restrict para evitar borrados en cascada accidentales
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Libros)
                .HasForeignKey(l => l.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Categoria)
                .WithMany(c => c.Libros)
                .HasForeignKey(l => l.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Estudiante)
                .WithMany(e => e.Prestamos)
                .HasForeignKey(p => p.EstudianteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

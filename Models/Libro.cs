using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioCampus.Models
{
    /// <summary>
    /// Entidad 3: Libro del catálogo universitario (5 atributos).
    /// Se relaciona N:1 con Autor, N:1 con Categoria y 1:N con Prestamo.
    /// </summary>
    [Table("Libros")]
    public class Libro
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título del libro es obligatorio.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "El título debe tener entre 2 y 150 caracteres.")]
        [Display(Name = "Título del Libro")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El código ISBN es obligatorio.")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "El ISBN debe tener entre 10 y 20 caracteres.")]
        [RegularExpression(@"^[\d\-X]{10,20}$", ErrorMessage = "El ISBN solo puede contener dígitos, guiones y la letra X (ej. 978-0-13-235088-4).")]
        [Display(Name = "ISBN")]
        public string Isbn { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un autor.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un autor válido.")]
        [ForeignKey("Autor")]
        [Display(Name = "Autor")]
        public int AutorId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
        [ForeignKey("Categoria")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        // Propiedades de navegación
        public Autor Autor { get; set; }
        public Categoria Categoria { get; set; }
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}

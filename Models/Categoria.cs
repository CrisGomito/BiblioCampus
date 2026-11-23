using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioCampus.Models
{
    /// <summary>
    /// Entidad 1: Categoría bibliográfica (5 atributos).
    /// Se relaciona 1:N con la entidad Libro.
    /// </summary>
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El código de la categoría es obligatorio.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "El código debe tener entre 3 y 10 caracteres.")]
        [RegularExpression(@"^[A-Z]{3}-\d{2,4}$", ErrorMessage = "Formato inválido. Use tres letras mayúsculas, guion y números (ej. ING-01).")]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
        [Display(Name = "Nombre de Categoría")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 250 caracteres.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La ubicación en estante es obligatoria.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "La ubicación debe tener entre 3 y 40 caracteres.")]
        [Display(Name = "Ubicación en Estante")]
        public string UbicacionEstante { get; set; }

        // Propiedad de navegación (Relación 1:N -> Libros)
        public ICollection<Libro> Libros { get; set; } = new List<Libro>();
    }
}

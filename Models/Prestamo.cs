using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioCampus.Models
{
    /// <summary>
    /// Entidad 5: Registro de préstamo bibliográfico (5 atributos).
    /// Conecta N:1 con Estudiante y N:1 con Libro.
    /// </summary>
    [Table("Prestamos")]
    public class Prestamo
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de préstamo es obligatoria.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Préstamo")]
        public DateTime FechaPrestamo { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "El estado del préstamo es obligatorio.")]
        [StringLength(20, ErrorMessage = "El estado no puede exceder los 20 caracteres.")]
        [RegularExpression(@"^(Activo|Devuelto|Atrasado)$", ErrorMessage = "El estado debe ser Activo, Devuelto o Atrasado.")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        [Required(ErrorMessage = "Debe seleccionar un estudiante.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estudiante válido.")]
        [ForeignKey("Estudiante")]
        [Display(Name = "Estudiante")]
        public int EstudianteId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un libro.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un libro válido.")]
        [ForeignKey("Libro")]
        [Display(Name = "Libro")]
        public int LibroId { get; set; }

        // Propiedades de navegación
        public Estudiante Estudiante { get; set; }
        public Libro Libro { get; set; }
    }
}

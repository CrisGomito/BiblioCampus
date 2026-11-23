using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioCampus.Models
{
    /// <summary>
    /// Entidad 4: Estudiante universitario habilitado para préstamos (5 atributos).
    /// Se relaciona 1:N con la entidad Prestamo.
    /// </summary>
    [Table("Estudiantes")]
    public class Estudiante
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La cédula del estudiante es obligatoria.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La cédula debe tener exactamente 10 dígitos.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "La cédula debe contener únicamente 10 dígitos numéricos.")]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El nombre completo del estudiante es obligatorio.")]
        [StringLength(120, MinimumLength = 5, ErrorMessage = "El nombre completo debe tener entre 5 y 120 caracteres.")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo electrónico institucional es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese una dirección de correo electrónico válida.")]
        [StringLength(120, ErrorMessage = "El correo no puede exceder los 120 caracteres.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La carrera universitaria es obligatoria.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "La carrera debe tener entre 4 y 100 caracteres.")]
        [Display(Name = "Carrera")]
        public string Carrera { get; set; }

        // Propiedad de navegación (Relación 1:N -> Préstamos)
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}

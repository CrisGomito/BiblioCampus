using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioCampus.Models
{
    /// <summary>
    /// Entidad 2: Autor de obras bibliográficas (5 atributos).
    /// Se relaciona 1:N con la entidad Libro.
    /// </summary>
    [Table("Autores")]
    public class Autor
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Los nombres del autor son obligatorios.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Los nombres deben tener entre 2 y 80 caracteres.")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos del autor son obligatorios.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 80 caracteres.")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "La nacionalidad del autor es obligatoria.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "La nacionalidad debe tener entre 3 y 60 caracteres.")]
        [Display(Name = "Nacionalidad")]
        public string Nacionalidad { get; set; }

        [Required(ErrorMessage = "El correo de contacto es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese una dirección de correo electrónico válida.")]
        [StringLength(120, ErrorMessage = "El correo no puede exceder los 120 caracteres.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo de Contacto")]
        public string CorreoContacto { get; set; }

        // Propiedad auxiliar no mapeada a SQL Server para listas desplegables
        [NotMapped]
        [Display(Name = "Autor")]
        public string NombreCompleto => $"{Nombres} {Apellidos}";

        // Propiedad de navegación (Relación 1:N -> Libros)
        public ICollection<Libro> Libros { get; set; } = new List<Libro>();
    }
}

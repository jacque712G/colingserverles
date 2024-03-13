using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo CI es requerido")]
        [StringLength(maximumLength: 20)]
        public string CI { get; set; } = null!;

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(maximumLength: 70)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El campo Apellidos es requerido")]
        [StringLength(maximumLength: 100)]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "El campo Fecha Nacimiento es requerido")]
        public DateTime FechaNacimiento { get; set; }

        [StringLength(maximumLength: 250)]
        public string? Foto { get; set; }

        [Required(ErrorMessage = "El campo Estado es requerido")]
        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } = null!;
    }
}

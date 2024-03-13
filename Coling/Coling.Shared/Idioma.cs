using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Idioma
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El campo Nombre Idioma es requerido")]
        [StringLength(maximumLength:50)]
        public string NombreIdioma { get; set; } = null!;

        [Required(ErrorMessage = "El campo Estado es requerido")]
        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } = null!;
    }
}

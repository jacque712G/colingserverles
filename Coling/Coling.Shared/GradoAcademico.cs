using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class GradoAcademico
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo Nombre Grado es requerido")]
        [StringLength(maximumLength:100)]
        public string NombreGrado { get; set; } = null!;

        [Required(ErrorMessage = "El campo Estado es requerido")]
        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } = null!;
    }
}

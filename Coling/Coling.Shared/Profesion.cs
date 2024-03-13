using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Profesion
    {
        [Key]
       public int Id { get; set; }

       [Required(ErrorMessage ="El campo Nombre Profesion es requerido")]
       [StringLength(maximumLength:150)]
       public string NombreProfesion { get; set; } = null!;
       public int IdGrado { get; set; }

       [Required(ErrorMessage = "El campo Estado es requerido")]
       [StringLength(maximumLength: 20)]
       public string Estado { get; set; } = null!;

        [ForeignKey("IdGrado")]
        public virtual GradoAcademico? GradoAcademico { get;set; }

    }
}

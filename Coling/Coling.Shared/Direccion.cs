using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Direccion
    {
        [Key]
        public int Id { get; set; }
        public int IdPersona { get; set; }

        [Required(ErrorMessage = "El campo Descripcion es requerido")]
        [StringLength(maximumLength: 150)]
        public string descripcion { get; set; } = null!;

        [Required(ErrorMessage = "El campo Estado es requerido")]
        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } = null!;

        [ForeignKey("IdPersona")]
        public virtual Persona? Persona { get; set; }
    }
}

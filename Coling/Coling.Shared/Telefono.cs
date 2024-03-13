using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Telefono
    {
        [Key]
        public int Id { get; set; }
        public int IdPersona { get; set; }

        [Required(ErrorMessage = "El campo Nro Telefono es requerido")]
        [StringLength(maximumLength: 60)]
        public string NroTelefono { get; set; } = null!;

        [Required(ErrorMessage = "El campo Estado es requerido")]
        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } = null!;

        [ForeignKey("IdPersona")]
        public virtual Persona? Persona { get; set; }
    }
}

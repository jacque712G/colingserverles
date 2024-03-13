using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class Afiliado
    {
        [Key]
        public int Id { get; set; }
        public int IdPersona { get; set; }

        [Required(ErrorMessage = "El campo Fecha Afilacion es requerido")]
        public DateTime FechaAfilacion { get; set; }

        [Required(ErrorMessage = "El campo Codigo Afiliado es requerido")]
        [StringLength(maximumLength: 50)]
        public string CodigoAfiliado { get; set; } = null!;

        [Required(ErrorMessage = "El campo Nro Titulo Provisional es requerido")]
        [StringLength(maximumLength: 50)]
        public string NroTituloProvisional { get; set; } = null!;

        [Required(ErrorMessage = "El campo Estado es requerido")]
        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } = null!;

        [ForeignKey("IdPersona")]
        public virtual Persona Persona { get; set; }=null!;
    }
}

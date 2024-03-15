using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class ProfesionAfiliado
    {
        [Key]
        public int Id { get; set; }
        public int IdAfiliado { get; set; }
        public string IdProfesion { get; set; } = null!;

        [Required(ErrorMessage ="El campo Fecha Asignacion es requerido")] 
        public DateTime FechaAsignacion { get; set; }

        [Required(ErrorMessage = "El campo Nro Sellos SIB es requerido")]
        [StringLength(maximumLength:25)]
        public string NroSelloSib { get; set; } = null!;

        [Required(ErrorMessage = "El campo Estado es requerido")]
        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } = null!;

        [ForeignKey("IdAfiliado")]
        public virtual Afiliado? Afiliado { get; set; }

        
    }
}

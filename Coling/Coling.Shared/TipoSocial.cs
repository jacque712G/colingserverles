using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class TipoSocial
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre Social es requerido")]
        [StringLength(maximumLength: 50)]
        public string NombreSocial { get; set; } = null!;

        [Required(ErrorMessage = "El campo Estado es requerido")]
        [StringLength(maximumLength: 20)]
        public string Estado { get; set; } = null!;
    }
}

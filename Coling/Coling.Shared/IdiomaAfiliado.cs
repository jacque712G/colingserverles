using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public class IdiomaAfiliado
    {
        [Key]
        public int Id { get; set; }
        public int IdAfiliado { get; set; }
        public int IdIdioma { get; set; }
        public string Estado { get; set; } = null!;

        [ForeignKey("IdAfiliado")]
        public virtual Afiliado? Afiliado { get; set; }

        [ForeignKey("IdIdioma")]
        public virtual Idioma? Idioma { get; set; }

    }
}

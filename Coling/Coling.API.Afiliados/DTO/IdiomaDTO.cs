using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.DTO
{
    public class IdiomaDTO
    {
        public int Id { get; set; }
        public int IdAfiliado { get; set; }
        public int IdIdioma { get; set; }
        public string? NombreIdioma { get; set; }
        public string Estado { get; set; } = null!;
    }
}

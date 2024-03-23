using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.DTO
{
    public class ProfesionAfiliadoDTO
    {
        public int Id { get; set; }
        public int IdAfiliado { get; set; }
        public string IdProfesion { get; set; } = null!;
        public DateTime FechaAsignacion { get; set; }
        public string NroSelloSib { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.DTO
{
    public class TipoSocialDTO
    {
        public int Id { get; set; }
        public int IdTipoSocial { get; set; }
        public int IdPersona { get; set; }
        public string? NombreTipo { get; set; }
        public string Estado { get; set; } = null!;
    }
}

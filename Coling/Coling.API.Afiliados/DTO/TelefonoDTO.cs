using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.DTO
{
    public class TelefonoDTO
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public string NroTelefono { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}

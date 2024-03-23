using Coling.API.Afiliados.DTO;
using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface ITelefonoLogic
    {
        public Task<bool> InsertarTelefono(TelefonoDTO telefono);
        public Task<bool> ModificarTelefono(TelefonoDTO telefono, int id);
        public Task<bool> EliminarTelefono(int id);
        public Task<List<TelefonoDTO>> ListarTelefonoTodos();
        public Task<TelefonoDTO> ObtenerTelefonoById(int id);
        public Task<List<TelefonoDTO>> BuscarPersonaTelefonos(int idPersona);
    }
}

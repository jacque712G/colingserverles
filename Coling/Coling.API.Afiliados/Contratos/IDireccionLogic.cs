using Coling.API.Afiliados.DTO;
using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface IDireccionLogic
    {
        public Task<bool> InsertarDireccion(DireccionDTO direccion);
        public Task<bool> ModificarDireccion(DireccionDTO direccion,int id);
        public Task<bool> EliminarDireccion(int id);
        public Task<List<DireccionDTO>> ListarDireccionTodas();
        public Task<DireccionDTO> ObtenerDireccionById(int id);

        public Task<List<DireccionDTO>> BuscarPersonaDirecciones(int idPersona);
    }
}

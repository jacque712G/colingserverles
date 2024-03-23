using Coling.API.Afiliados.DTO;
using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface IIdiomaAfiliadoLogic
    {
        public Task<bool> InsertarIdiomaAfiliado(IdiomaAfiliadoDTO idiomaAfiliado);
        public Task<bool> ModificarIdiomaAfiliado(IdiomaAfiliadoDTO idiomaAfiliado,int id);
        public Task<bool> EliminarIdiomaAfiliado(int id);
        public Task<List<IdiomaAfiliadoDTO>> ListarIdiomaAfiliadoTodos();
        public Task<IdiomaAfiliadoDTO> ObtenerIdiomaAfiliadoById(int id);

        public Task<List<IdiomaDTO>> BuscarAfiliadoIdiomas(int idAfiliado);
    }
}

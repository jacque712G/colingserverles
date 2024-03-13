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
        public Task<bool> InsertarIdiomaAfiliado(IdiomaAfiliado idiomaAfiliado);
        public Task<bool> ModificarIdiomaAfiliado(IdiomaAfiliado idiomaAfiliado,int id);
        public Task<bool> EliminarIdiomaAfiliado(int id);
        public Task<List<IdiomaAfiliado>> ListarIdiomaAfiliadoTodos();
        public Task<IdiomaAfiliado> ObtenerIdiomaAfiliadoById(int id);
    }
}

using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Contratos
{
    public interface IInstitucionLogic
    {
        public Task<bool> InsertarInstitucion(Institucion institucion);
        public Task<bool> ModificarInstitucion(Institucion institucion);
        public Task<bool> EliminarInstitucion(string id);
        public Task<List<Institucion>> ListarInstitucionTodos();
        public Task<Institucion> ObtenerInstitucionById(string id);
    }
}

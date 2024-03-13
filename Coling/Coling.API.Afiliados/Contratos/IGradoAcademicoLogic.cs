using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface IGradoAcademicoLogic
    {
        public Task<bool> InsertarGradoAcademico(GradoAcademico grado);
        public Task<bool> ModificarGradoAcademico(GradoAcademico grado,int id);
        public Task<bool> EliminarGradoAcademico(int id);
        public Task<List<GradoAcademico>> ListarGradoTodos();
        public Task<GradoAcademico> ObtenerGradoById(int id);
    }
}

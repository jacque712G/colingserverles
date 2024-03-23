using Coling.API.Afiliados.DTO;
using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface IProfesionAfiliadoLogic
    {
        public Task<bool> InsertarProfesionAfiliado(ProfesionAfiliadoDTO profesionAfiliado);
        public Task<bool> ModificarProfesionAfiliado(ProfesionAfiliadoDTO profesionAfiliado, int id);
        public Task<bool> EliminarProfesionAfiliado(int id);
        public Task<List<ProfesionAfiliadoDTO>> ListarProfesionAfliadoTodos();
        public Task<ProfesionAfiliadoDTO> ObtenerProfesionAfiliadoById(int id);
        public Task<List<ProfesionAfiliadoDTO>> BuscarAfiliadoProfesiones(int idAfiliado);
    }
}

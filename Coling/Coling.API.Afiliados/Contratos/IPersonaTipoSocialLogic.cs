using Coling.API.Afiliados.DTO;
using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Contratos
{
    public interface IPersonaTipoSocialLogic
    {
        public Task<bool> InsertarPersonaTipoSocial(PersonaTipoSocialDTO personaTipoSocial);
        public Task<bool> ModificarPersonaTipoSocial(PersonaTipoSocialDTO personaTipoSocial, int id);
        public Task<bool> EliminarPersonaTipoSocial(int id);
        public Task<List<PersonaTipoSocialDTO>> ListarPersonaTipoSocialTodos();
        public Task<PersonaTipoSocialDTO> ObtenerPersonaTipoSocialById(int id);
        public Task<List<TipoSocialDTO>> BuscarPersonaTiposSociales(int idPersona);
    }
}

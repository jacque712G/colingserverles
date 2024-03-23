using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.DTO;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    public class PersonaTipoSocialLogic : IPersonaTipoSocialLogic
    {
        private readonly Contexto contexto;
        public PersonaTipoSocialLogic(Contexto _contexto)
        {
            this.contexto = _contexto;
        }
        public async Task<bool> EliminarPersonaTipoSocial(int id)
        {
            bool sw = false;
            PersonaTipoSocial? existe = await contexto.PersonasTiposSociales.FindAsync(id);
            if (existe!=null)
            {
                contexto.PersonasTiposSociales.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> InsertarPersonaTipoSocial(PersonaTipoSocialDTO personaTipoSocial)
        {
            bool sw = false;
            contexto.PersonasTiposSociales.Add(map(personaTipoSocial));
            int response = await contexto.SaveChangesAsync();
            if (response==1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<PersonaTipoSocialDTO>> ListarPersonaTipoSocialTodos()
        {
            var lista = await contexto.PersonasTiposSociales
                              .Select(x=> new PersonaTipoSocialDTO 
                              { 
                               Id = x.Id,
                               IdPersona = x.IdPersona,
                               IdTipoSocial = x.IdTipoSocial,
                               Estado = x.Estado,
                              }).ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarPersonaTipoSocial(PersonaTipoSocialDTO personaTipoSocial, int id)
        {
            bool sw = false;
            PersonaTipoSocial? existe = await contexto.PersonasTiposSociales.FindAsync(id);
            if (existe!=null) 
            {
                existe.IdTipoSocial = personaTipoSocial.IdTipoSocial;
                existe.IdPersona = personaTipoSocial.IdPersona;
                existe.Estado=personaTipoSocial.Estado;
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<PersonaTipoSocialDTO> ObtenerPersonaTipoSocialById(int id)
        {
            PersonaTipoSocial? personaTipoSocial = await contexto.PersonasTiposSociales.FirstOrDefaultAsync(x=>x.Id==id);
            PersonaTipoSocialDTO perTipoSocial = null!;
            if (personaTipoSocial!=null)
            {
                perTipoSocial = new PersonaTipoSocialDTO();
                perTipoSocial.Id = personaTipoSocial.Id;
                perTipoSocial.IdPersona = personaTipoSocial.IdPersona;
                perTipoSocial.IdTipoSocial = personaTipoSocial.IdTipoSocial;
                perTipoSocial.Estado = personaTipoSocial.Estado;
            }
          
            return perTipoSocial;
        }
        public async Task<List<TipoSocialDTO>> BuscarPersonaTiposSociales(int idPersona)
        {
            var lista = await contexto.PersonasTiposSociales
                              .Where(p=>p.IdPersona==idPersona)
                              .Select(x => new TipoSocialDTO
                              {
                                  Id = x.Id,
                                  IdPersona = x.IdPersona,
                                  IdTipoSocial = x.IdTipoSocial,
                                  NombreTipo=x.TipoSocial!.NombreSocial,
                                  Estado = x.Estado,
                              }).ToListAsync();
            return lista;
        }
        public PersonaTipoSocial map(PersonaTipoSocialDTO personaTipoSocial) 
        {
            PersonaTipoSocial perTipoSocial = new PersonaTipoSocial();
            perTipoSocial.IdPersona = personaTipoSocial.IdPersona;
            perTipoSocial.IdTipoSocial = personaTipoSocial.IdTipoSocial;
            perTipoSocial.Estado = personaTipoSocial.Estado;
            perTipoSocial.Persona = null;
            perTipoSocial.TipoSocial = null;
            return perTipoSocial;
        }

       
    }
}

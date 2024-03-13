using Coling.API.Afiliados.Contratos;
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

        public async Task<bool> InsertarPersonaTipoSocial(PersonaTipoSocial personaTipoSocial)
        {
            bool sw = false;
            contexto.PersonasTiposSociales.Add(personaTipoSocial);
            int response = await contexto.SaveChangesAsync();
            if (response==1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<PersonaTipoSocial>> ListarPersonaTipoSocialTodos()
        {
            var lista = await contexto.PersonasTiposSociales.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarPersonaTipoSocial(PersonaTipoSocial personaTipoSocial, int id)
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

        public async Task<PersonaTipoSocial> ObtenerPersonaTipoSocialById(int id)
        {
            PersonaTipoSocial? personaTipoSocial = await contexto.PersonasTiposSociales.FirstOrDefaultAsync(x=>x.Id==id);
            return personaTipoSocial;
        }
    }
}

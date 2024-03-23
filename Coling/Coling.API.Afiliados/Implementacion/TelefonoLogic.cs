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
    public class TelefonoLogic : ITelefonoLogic
    {
        private readonly Contexto contexto;
        public TelefonoLogic(Contexto _contexto)
        {
            this.contexto = _contexto;
        }
        public async Task<bool> EliminarTelefono(int id)
        {
            bool sw = false;
            Telefono? existe = await contexto.Telefonos.FindAsync(id);
            if (existe!=null)
            {
                contexto.Telefonos.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> InsertarTelefono(TelefonoDTO telefono)
        {
            bool sw = false;
            contexto.Telefonos.Add(map(telefono));
            int response = await contexto.SaveChangesAsync();
            if (response==1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<TelefonoDTO>> ListarTelefonoTodos()
        {
            var lista = await contexto.Telefonos
                                      .Select(x=> new TelefonoDTO 
                                      { 
                                       Id= x.Id,
                                       IdPersona= x.IdPersona,
                                       NroTelefono= x.NroTelefono,
                                       Estado= x.Estado
                                      }).ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarTelefono(TelefonoDTO telefono, int id)
        {
            bool sw = false;
            Telefono? existe = await contexto.Telefonos.FindAsync(id);
            if (existe!=null) 
            {
                existe.IdPersona = telefono.IdPersona;
                existe.NroTelefono = telefono.NroTelefono;
                existe.Estado=telefono.Estado;
                await contexto.SaveChangesAsync();
                sw= true;
            }
            return sw;
        }

        public async Task<TelefonoDTO> ObtenerTelefonoById(int id)
        {
            Telefono? telefono = await contexto.Telefonos.FirstOrDefaultAsync(x=>x.Id==id);
            TelefonoDTO tel = null!;
            if (telefono!=null)
            {
                tel = new TelefonoDTO();
                tel.Id = telefono.Id;
                tel.IdPersona = telefono.IdPersona;
                tel.NroTelefono = telefono.NroTelefono;
                tel.Estado = telefono.Estado;
            }
           
            return tel;
        }

        public async Task<List<TelefonoDTO>> BuscarPersonaTelefonos(int idPersona)
        {
            var lista = await contexto.Telefonos
                                      .Where(p=>p.IdPersona==idPersona)
                                      .Select(x => new TelefonoDTO
                                      {
                                          Id = x.Id,
                                          IdPersona = x.IdPersona,
                                          NroTelefono = x.NroTelefono,
                                          Estado = x.Estado
                                      }).ToListAsync();
            return lista;
        }

        public Telefono map(TelefonoDTO telefono) 
        {
            Telefono tel=new Telefono();
            tel.IdPersona= telefono.IdPersona;
            tel.NroTelefono = telefono.NroTelefono;
            tel.Estado= telefono.Estado;
            tel.Persona = null;
            return tel;
        }
    }
}

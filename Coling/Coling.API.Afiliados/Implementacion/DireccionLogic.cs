using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.DTO;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Afiliados.Implementacion
{
    public class DireccionLogic : IDireccionLogic
    {
        private readonly Contexto contexto;
        public DireccionLogic(Contexto _contexto)
        {
            this.contexto= _contexto;
        }
        public async Task<bool> EliminarDireccion(int id)
        {
            bool sw = false;
            Direccion? existe= await contexto.Direcciones.FindAsync(id);
            if (existe!=null)
            {
                contexto.Direcciones.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> InsertarDireccion(DireccionDTO direccion)
        {
            bool sw = false;          
            contexto.Direcciones.Add(map(direccion));
            int response = await contexto.SaveChangesAsync();
            if (response==1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<DireccionDTO>> ListarDireccionTodas()
        {
            var lista = await contexto.Direcciones
                        .Select(x=> new DireccionDTO
                        {
                            Id = x.Id,
                            IdPersona=x.IdPersona,
                            descripcion=x.descripcion,
                            Estado=x.Estado
                        }).ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarDireccion(DireccionDTO direccion, int id)
        {
            bool sw = false;
            Direccion? existe = await contexto.Direcciones.FindAsync(id);
            if (existe!=null)
            {
                existe.IdPersona = direccion.IdPersona;
                existe.descripcion = direccion.descripcion;
                existe.Estado=direccion.Estado;

                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }
       
     
        public async Task<DireccionDTO> ObtenerDireccionById(int id)
        {
            Direccion? dir = await contexto.Direcciones.FirstOrDefaultAsync(x=>x.Id==id);
            DireccionDTO direccion= null!;
            if (dir!=null)
            {
                direccion = new DireccionDTO();
                direccion.Id = dir.Id;
                direccion.IdPersona = dir!.IdPersona;
                direccion.descripcion = dir.descripcion;
                direccion.Estado = dir.Estado;
            }
                       
            return direccion;
        }
        public async Task<List<DireccionDTO>> BuscarPersonaDirecciones(int idPersona)
        {
            var lista = await contexto.Direcciones
                        .Where(p=>p.IdPersona==idPersona)
                        .Select(x => new DireccionDTO
                        {
                            Id = x.Id,
                            IdPersona = x.IdPersona,
                            descripcion = x.descripcion,
                            Estado = x.Estado
                        }).ToListAsync();
            return lista;
        }
        public Direccion map(DireccionDTO direccion) 
        {
            Direccion dir = new Direccion();
            dir.IdPersona=direccion.IdPersona;
            dir.descripcion=direccion.descripcion;
            dir.Estado = direccion.Estado;
            dir.Persona = null;
            return dir;
        }
    }
}

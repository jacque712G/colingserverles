using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public async Task<bool> InsertarDireccion(Direccion direccion)
        {
            bool sw = false;
            contexto.Direcciones.Add(direccion);
            int response = await contexto.SaveChangesAsync();
            if (response==1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Direccion>> ListarDireccionTodas()
        {
            var lista = await contexto.Direcciones.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarDireccion(Direccion direccion, int id)
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

        public async Task<Direccion> ObtenerDireccionById(int id)
        {
            Direccion? direccion = await contexto.Direcciones.FirstOrDefaultAsync(x=>x.Id==id);
            return direccion;
        }
    }
}

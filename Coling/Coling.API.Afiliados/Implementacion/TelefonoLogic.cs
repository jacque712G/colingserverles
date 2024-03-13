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

        public async Task<bool> InsertarTelefono(Telefono telefono)
        {
            bool sw = false;
            contexto.Telefonos.Add(telefono);
            int response = await contexto.SaveChangesAsync();
            if (response==1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Telefono>> ListarTelefonoTodos()
        {
            var lista = await contexto.Telefonos.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarTelefono(Telefono telefono, int id)
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

        public async Task<Telefono> ObtenerTelefonoById(int id)
        {
            Telefono? telefono = await contexto.Telefonos.FirstOrDefaultAsync(x=>x.Id==id);
            return telefono;
        }
    }
}

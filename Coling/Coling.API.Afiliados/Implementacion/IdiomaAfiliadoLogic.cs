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
    public class IdiomaAfiliadoLogic : IIdiomaAfiliadoLogic
    {
        private readonly Contexto contexto;
        public IdiomaAfiliadoLogic(Contexto _contexto)
        {
            this.contexto = _contexto;
        }
        public async Task<bool> EliminarIdiomaAfiliado(int id)
        {
            bool sw=false;
            IdiomaAfiliado? existe=await contexto.IdiomasAfiliados.FindAsync(id);
            if (existe!=null)
            {
                contexto.IdiomasAfiliados.Remove(existe);
                await contexto.SaveChangesAsync();
                sw=true;    
            }
            return sw;
        }

        public async Task<bool> InsertarIdiomaAfiliado(IdiomaAfiliado idiomaAfiliado)
        {
            bool sw = false;
            contexto.IdiomasAfiliados.Add(idiomaAfiliado);
            int response = await contexto.SaveChangesAsync();
            if (response==1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<IdiomaAfiliado>> ListarIdiomaAfiliadoTodos()
        {
            var lista = await contexto.IdiomasAfiliados.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarIdiomaAfiliado(IdiomaAfiliado idiomaAfiliado, int id)
        {
            bool sw = false;
            IdiomaAfiliado? existe = await contexto.IdiomasAfiliados.FindAsync(id);
            if (existe != null)
            {
                existe.IdAfiliado = idiomaAfiliado.IdAfiliado;
                existe.Idioma = idiomaAfiliado.Idioma;
                existe.Estado = idiomaAfiliado.Estado;

                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<IdiomaAfiliado> ObtenerIdiomaAfiliadoById(int id)
        {
            IdiomaAfiliado? idiomaAfiliado = await contexto.IdiomasAfiliados.FirstOrDefaultAsync(x=>x.Id==id);
            return idiomaAfiliado;
        }
    }
}

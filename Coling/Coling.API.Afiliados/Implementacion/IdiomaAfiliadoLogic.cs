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

        public async Task<bool> InsertarIdiomaAfiliado(IdiomaAfiliadoDTO idiomaAfiliado)
        {
            bool sw = false;
            contexto.IdiomasAfiliados.Add(map(idiomaAfiliado));
            int response = await contexto.SaveChangesAsync();
            if (response==1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<IdiomaAfiliadoDTO>> ListarIdiomaAfiliadoTodos()
        {
            var lista = await contexto.IdiomasAfiliados
                              .Select(x=>new IdiomaAfiliadoDTO 
                              {
                               Id=x.Id,
                               IdAfiliado=x.IdAfiliado,
                               IdIdioma=x.IdIdioma,
                               Estado=x.Estado
                              }).ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarIdiomaAfiliado(IdiomaAfiliadoDTO idiomaAfiliado, int id)
        {
            bool sw = false;
            IdiomaAfiliado? existe = await contexto.IdiomasAfiliados.FindAsync(id);
            if (existe != null)
            {
                existe.IdAfiliado = idiomaAfiliado.IdAfiliado;
                existe.IdIdioma = idiomaAfiliado.IdIdioma;
                existe.Estado = idiomaAfiliado.Estado;

                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<IdiomaAfiliadoDTO> ObtenerIdiomaAfiliadoById(int id)
        {
            IdiomaAfiliado? idiomaAfiliado = await contexto.IdiomasAfiliados.FirstOrDefaultAsync(x=>x.Id==id);
            IdiomaAfiliadoDTO idiomaAfil =null!;
            if (idiomaAfiliado!=null)
            {
                idiomaAfil = new IdiomaAfiliadoDTO();
                idiomaAfil.Id = idiomaAfiliado!.Id;
                idiomaAfil.IdAfiliado = idiomaAfiliado.Id;
                idiomaAfil.IdIdioma = idiomaAfiliado.IdAfiliado;
                idiomaAfil.Estado = idiomaAfiliado.Estado;
            }
           
            return idiomaAfil;
        }
        public async Task<List<IdiomaDTO>> BuscarAfiliadoIdiomas(int idAfiliado)
        {
            var lista = await contexto.IdiomasAfiliados
                              .Where(a=>a.IdAfiliado==idAfiliado)
                              .Select(x => new IdiomaDTO
                              {
                                  Id = x.Id,
                                  IdAfiliado = x.IdAfiliado,
                                  IdIdioma = x.IdIdioma,
                                  NombreIdioma=x.Idioma!.NombreIdioma,
                                  Estado = x.Estado
                              }).ToListAsync();
            return lista;
        }
        public IdiomaAfiliado map(IdiomaAfiliadoDTO idiomaAfiliado) 
        {
            IdiomaAfiliado idiomaAfil=new IdiomaAfiliado();
            idiomaAfil.IdAfiliado = idiomaAfiliado.IdAfiliado;
            idiomaAfil.IdIdioma = idiomaAfiliado.IdIdioma;
            idiomaAfil.Estado = idiomaAfiliado.Estado;
            idiomaAfil.Idioma = null;
            idiomaAfil.Afiliado = null;
            return idiomaAfil;
        }

       
    }
}

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
    public class ProfesionAfiliadoLogic : IProfesionAfiliadoLogic
    {
        private readonly Contexto contexto;
        public ProfesionAfiliadoLogic(Contexto _contexto)
        {
            this.contexto = _contexto;
        }
        public async Task<bool> EliminarProfesionAfiliado(int id)
        {
            bool sw = false;
            ProfesionAfiliado? existe = await contexto.ProfesionesAfiliados.FindAsync(id);
            if (existe != null) 
            {
                contexto.ProfesionesAfiliados.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> InsertarProfesionAfiliado(ProfesionAfiliadoDTO profesionAfiliado)
        {
            bool sw = false;
            contexto.ProfesionesAfiliados.Add(map(profesionAfiliado));
            int response = await contexto.SaveChangesAsync();
            if (response==1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<ProfesionAfiliadoDTO>> ListarProfesionAfliadoTodos()
        {
            var lista = await contexto.ProfesionesAfiliados
                                       .Select(x=>new ProfesionAfiliadoDTO 
                                       { 
                                        Id= x.Id,
                                        IdAfiliado= x.IdAfiliado,
                                        IdProfesion= x.IdProfesion,
                                        FechaAsignacion= x.FechaAsignacion,
                                        NroSelloSib= x.NroSelloSib,
                                        Estado= x.Estado
                                       }).ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarProfesionAfiliado(ProfesionAfiliadoDTO profesionAfiliado, int id)
        {
            bool sw = false;
            ProfesionAfiliado? existe = await contexto.ProfesionesAfiliados.FindAsync(id);
            if (existe!=null)
            {
                existe.IdAfiliado = profesionAfiliado.IdAfiliado;
                existe.IdProfesion = profesionAfiliado.IdProfesion;
                existe.FechaAsignacion = profesionAfiliado.FechaAsignacion;
                existe.NroSelloSib = profesionAfiliado.NroSelloSib;
                existe.Estado = profesionAfiliado.Estado;
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<ProfesionAfiliadoDTO> ObtenerProfesionAfiliadoById(int id)
        {
            ProfesionAfiliado? profesionAfiliado = await contexto.ProfesionesAfiliados.FirstOrDefaultAsync(x=>x.Id==id);
            ProfesionAfiliadoDTO profAfiliado =null!;
            if (profesionAfiliado!=null)
            {
                profAfiliado = new ProfesionAfiliadoDTO();
                profAfiliado.Id = profesionAfiliado.Id;
                profAfiliado.IdAfiliado = profesionAfiliado.IdAfiliado;
                profAfiliado.IdProfesion = profesionAfiliado.IdProfesion;
                profAfiliado.FechaAsignacion = profesionAfiliado.FechaAsignacion;
                profAfiliado.NroSelloSib = profesionAfiliado.NroSelloSib;
                profAfiliado.Estado = profesionAfiliado.Estado;
            }
           

            return profAfiliado;
        }
        public async Task<List<ProfesionAfiliadoDTO>> BuscarAfiliadoProfesiones(int idAfiliado)
        {
            var lista = await contexto.ProfesionesAfiliados
                                      .Where(a=>a.IdAfiliado==idAfiliado)
                                      .Select(x => new ProfesionAfiliadoDTO
                                      {
                                          Id = x.Id,
                                          IdAfiliado = x.IdAfiliado,
                                          IdProfesion = x.IdProfesion,
                                          FechaAsignacion = x.FechaAsignacion,
                                          NroSelloSib = x.NroSelloSib,
                                          Estado = x.Estado
                                      }).ToListAsync();
            return lista;
        }
        public ProfesionAfiliado map(ProfesionAfiliadoDTO profesionAfiliado) 
        {
            ProfesionAfiliado profAfiliado=new ProfesionAfiliado();
            profAfiliado.IdAfiliado= profesionAfiliado.IdAfiliado;
            profAfiliado.IdProfesion = profesionAfiliado.IdProfesion;
            profAfiliado.FechaAsignacion = profesionAfiliado.FechaAsignacion;
            profAfiliado.NroSelloSib = profesionAfiliado.NroSelloSib;
            profAfiliado.Estado = profesionAfiliado.Estado;
            return profAfiliado;
        }

        
    }
}

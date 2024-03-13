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

        public async Task<bool> InsertarProfesionAfiliado(ProfesionAfiliado profesionAfiliado)
        {
            bool sw = false;
            contexto.ProfesionesAfiliados.Add(profesionAfiliado);
            int response = await contexto.SaveChangesAsync();
            if (response==1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<ProfesionAfiliado>> ListarProfesionAfliadoTodos()
        {
            var lista = await contexto.ProfesionesAfiliados.ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarProfesionAfiliado(ProfesionAfiliado profesionAfiliado, int id)
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

        public async Task<ProfesionAfiliado> ObtenerProfesionAfiliadoById(int id)
        {
            ProfesionAfiliado? profesionAfiliado = await contexto.ProfesionesAfiliados.FirstOrDefaultAsync(x=>x.Id==id);
            return profesionAfiliado;
        }
    }
}

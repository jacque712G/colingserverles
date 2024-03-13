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
    public class GradoAcademicoLogic : IGradoAcademicoLogic
    {
        private readonly Contexto contexto;
        public GradoAcademicoLogic(Contexto _contexto)
        {
            this.contexto = _contexto;
        }
        public async Task<bool> EliminarGradoAcademico(int id)
        {
            bool sw = false;
            GradoAcademico? existe=await contexto.GradosAcademicos.FindAsync(id);
            if (existe!=null)
            {
                contexto.GradosAcademicos.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<bool> InsertarGradoAcademico(GradoAcademico grado)
        {
            bool sw = false;
            contexto.GradosAcademicos.Add(grado);
            int response= await contexto.SaveChangesAsync();
            if (response==1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<GradoAcademico>> ListarGradoTodos()
        {
            var lista = await contexto.GradosAcademicos.ToListAsync();
            return lista;
        }

        public async  Task<bool> ModificarGradoAcademico(GradoAcademico grado, int id)
        {
            bool sw = false;
            GradoAcademico? existe = await contexto.GradosAcademicos.FindAsync(id);
            if (existe!=null)
            {
                existe.NombreGrado=grado.NombreGrado;
                existe.Estado=grado.Estado;

                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<GradoAcademico> ObtenerGradoById(int id)
        {
            GradoAcademico? grado = await contexto.GradosAcademicos.FirstOrDefaultAsync(x=>x.Id==id);
            return grado;
        }
    }
}

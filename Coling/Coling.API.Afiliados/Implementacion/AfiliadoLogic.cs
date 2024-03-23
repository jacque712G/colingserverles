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
    public class AfiliadoLogic : IAfiliadoLogic
    {
        private readonly Contexto contexto;

        public AfiliadoLogic(Contexto _contexto)
        {
            this.contexto = _contexto;
        }
        public async Task<bool> EliminarAfiliado(int id)
        {
            bool sw = false;
            Afiliado? existe=await contexto.Afiliados.FindAsync(id);
            if (existe!=null)
            {
                contexto.Afiliados.Remove(existe);
                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;


        }

        public async Task<bool> InsertarAfiliado(Afiliado afiliado)
        {
            bool sw = false;
            contexto.Afiliados.Add(afiliado);
            int response = await contexto.SaveChangesAsync();
            if (response == 2)
            {
                sw = true;
            }
          
            return sw;
        }

        public async Task<List<Afiliado>> ListarAfiliadoTodos()
        {
            var lista = await contexto.Afiliados.Include(x=>x.Persona).ToListAsync();
            return lista;
        }

        public async Task<bool> ModificarAfiliado(Afiliado afiliado, int id)
        {
            bool sw = false;
            Afiliado? existe = await contexto.Afiliados.FindAsync(id);
            if (existe!=null)
            {
                Persona? per = await contexto.Personas.FindAsync(existe.IdPersona);
                per!.CI = afiliado.Persona!.CI;
                per!.Nombre = afiliado.Persona!.Nombre;
                per!.Apellidos = afiliado.Persona!.Apellidos;
                per!.FechaNacimiento = afiliado.Persona!.FechaNacimiento;
                per!.Foto = afiliado.Persona!.Foto;

                existe.FechaAfilacion = afiliado.FechaAfilacion;
                existe.CodigoAfiliado = afiliado.CodigoAfiliado;
                existe.NroTituloProvisional = afiliado.NroTituloProvisional;
                existe.Estado=afiliado.Estado;

                await contexto.SaveChangesAsync();
                sw = true;
            }
            return sw;
        }

        public async Task<Afiliado> ObtenerAfiliadoById(int id)
        {
            Afiliado? afiliado = await contexto.Afiliados.Include(x=>x.Persona).FirstOrDefaultAsync(x => x.Id == id);
            return afiliado!;
        }
    }
}

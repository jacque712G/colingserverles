using Coling.API.Bolsatrabajo.Context;
using Coling.API.Bolsatrabajo.Contratos;
using Coling.Shared;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Implementacion
{
    public class InstitucionLogic : IInstitucionLogic
    {
        private readonly Contexto contexto;

        public InstitucionLogic(Contexto _contexto)
        {
            this.contexto = _contexto;
        }
        public async Task<bool> InsertarInstitucion(Institucion institucion)
        {
            bool sw = false;
                     
            try
            {
                await contexto.Instituciones.InsertOneAsync(institucion);
                sw = true;
            }
            catch (Exception)
            {

                sw = false;
            }
            

            return sw;
        }
        public async Task<bool> ModificarInstitucion(Institucion institucion)
        {
            bool sw = false;
            var existe = Builders<Institucion>.Filter.Eq(x => x.Id, institucion.Id);
            if (existe!=null)
            {
                await contexto.Instituciones.ReplaceOneAsync(existe, institucion);
                sw= true;
            }
            return sw;  

        }
        public async Task<bool> EliminarInstitucion(string id)
        {
            bool sw = false;
            var existe = Builders<Institucion>.Filter.Eq(x => x.Id, id);
            if (existe!=null) 
            {
                await contexto.Instituciones.DeleteOneAsync(existe);
                sw= true;

            }
            return sw;

        }
        public async Task<List<Institucion>> ListarInstitucionTodos()
        {
            var lista = await contexto.Instituciones.Find(Builders<Institucion>.Filter.Empty).ToListAsync();
            return lista;
        }

        public async Task<Institucion> ObtenerInstitucionById(string id)
        {
            var filter = Builders<Institucion>.Filter.Eq(x => x.Id, id);
            Institucion? institucion = await contexto.Instituciones.Find(filter).SingleOrDefaultAsync();
            return institucion;

        }
    }
}

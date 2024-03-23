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
    public class OfertaLogic : IOfertaLogic
    {
        private readonly Contexto contexto;

        public OfertaLogic(Contexto _contexto)
        {
            this.contexto = _contexto;
        }

        public async Task<bool> InsertarOferta(OfertaLaboral oferta)
        {
            bool sw = false;

            try
            {
                await contexto.Ofertas.InsertOneAsync(oferta);
                sw = true;
            }
            catch (Exception)
            {

                sw = false;
            }
            return sw;
        }
        public async Task<bool> ModificarOferta(OfertaLaboral oferta)
        {
            bool sw = false;
            var existe = Builders<OfertaLaboral>.Filter.Eq(x => x.Id, oferta.Id);
            if (existe != null)
            {
                await contexto.Ofertas.ReplaceOneAsync(existe, oferta);
                sw = true;
            }
            return sw;
        }
        public async Task<bool> EliminarOferta(string id)
        {
            bool sw = false;
            var existe = Builders<OfertaLaboral>.Filter.Eq(x => x.Id, id);
            if (existe != null)
            {
                await contexto.Ofertas.DeleteOneAsync(existe);
                sw = true;

            }
            return sw;
        }
        public async Task<List<OfertaLaboral>> ListarOfertaTodas()
        {
            var lista = await contexto.Ofertas.Find(Builders<OfertaLaboral>.Filter.Empty).ToListAsync();
            return lista;
        }
       
        public async Task<OfertaLaboral> ObtenerOfertaById(string id)
        {
            var filter = Builders<OfertaLaboral>.Filter.Eq(x => x.Id, id);
            OfertaLaboral? oferta = await contexto.Ofertas.Find(filter).SingleOrDefaultAsync();
            return oferta;
        }
        public async Task<List<OfertaLaboral>> BuscarInstitucionOfertas(string idInstitucion)
        {
            var filter = Builders<OfertaLaboral>.Filter.Eq(x => x.IdInstitucion, idInstitucion);
            var lista = await contexto.Ofertas.Find(filter).ToListAsync();
            return lista;
        }
    }
}

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
    public class SolicitudLogic : ISolicitudLogic
    {
        private readonly Contexto contexto;

        public SolicitudLogic(Contexto _contexto)
        {
            this.contexto = _contexto;
        }
        public async Task<bool> InsertarSolicitud(Solicitud solicitud)
        {
            bool sw = false;

            try
            {
                await contexto.Solicitudes.InsertOneAsync(solicitud);
                sw = true;
            }
            catch (Exception)
            {

                sw = false;
            }
            return sw;
        }
        public async Task<bool> ModificarSolicitud(Solicitud solicitud)
        {
            bool sw = false;
            var existe = Builders<Solicitud>.Filter.Eq(x => x.Id, solicitud.Id);
            if (existe != null)
            {
                await contexto.Solicitudes.ReplaceOneAsync(existe, solicitud);
                sw = true;
            }
            return sw;
        }
        public async Task<bool> EliminarSolicitud(string id)
        {
            bool sw = false;
            var existe = Builders<Solicitud>.Filter.Eq(x => x.Id, id);
            if (existe != null)
            {
                await contexto.Solicitudes.DeleteOneAsync(existe);
                sw = true;

            }
            return sw;
        }       

        public async Task<List<Solicitud>> ListarSolicitudTodas()
        {
            var lista = await contexto.Solicitudes.Find(Builders<Solicitud>.Filter.Empty).ToListAsync();
            return lista;
        }
       
        public async Task<Solicitud> ObtenerSolicitudById(string id)
        {
            var filter = Builders<Solicitud>.Filter.Eq(x => x.Id, id);
            Solicitud? solicitud = await contexto.Solicitudes.Find(filter).SingleOrDefaultAsync();
            return solicitud;
        }
        public async Task<List<Solicitud>> BuscarAfiliadoSolicitudes(int idAfiliado)
        {
            var filter = Builders<Solicitud>.Filter.Eq(x => x.IdAfiliado, idAfiliado);
            var lista = await contexto.Solicitudes.Find(filter).ToListAsync();
            return lista;
        }

        public async Task<List<Solicitud>> BuscarOfertaSolicitudes(string idOferta)
        {
            var filter = Builders<Solicitud>.Filter.Eq(x => x.IdOferta, idOferta);
            var lista = await contexto.Solicitudes.Find(filter).ToListAsync();
            return lista;
        }
    }
}

using Coling.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Context
{
    public class Contexto
    {
        private readonly IMongoDatabase _basedatos;
       
        public Contexto()
        {
            
            string cadenaConexion = Environment.GetEnvironmentVariable("cadenaConexion")!.ToString();
            string basedatosNombre = Environment.GetEnvironmentVariable("databaseName")!.ToString();
            var client = new MongoClient(cadenaConexion);
            if (client != null)
                this._basedatos = client.GetDatabase(basedatosNombre);
            
        }
        public IMongoCollection<Institucion> Instituciones
        {
            get
            {
                return _basedatos.GetCollection<Institucion>("institucion");
            }
        }
        public IMongoCollection<OfertaLaboral> Ofertas
        {
            get
            {
                return _basedatos.GetCollection<OfertaLaboral>("ofertalaboral");
            }
        }
        public IMongoCollection<Solicitud> Solicitudes
        {
            get
            {
                return _basedatos.GetCollection<Solicitud>("solicitud");
            }
        }



    }
}

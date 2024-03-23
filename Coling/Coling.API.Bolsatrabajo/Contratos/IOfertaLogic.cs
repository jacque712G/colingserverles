using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Contratos
{
    public interface IOfertaLogic
    {
        public Task<bool> InsertarOferta(OfertaLaboral oferta);
        public Task<bool> ModificarOferta(OfertaLaboral oferta);
        public Task<bool> EliminarOferta(string id);
        public Task<List<OfertaLaboral>> ListarOfertaTodas();
        public Task<OfertaLaboral> ObtenerOfertaById(string id);
        public Task<List<OfertaLaboral>> BuscarInstitucionOfertas(string idInstitucion);
    }
}

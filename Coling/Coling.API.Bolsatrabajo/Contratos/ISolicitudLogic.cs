using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Bolsatrabajo.Contratos
{
    public interface ISolicitudLogic
    {
        public Task<bool> InsertarSolicitud(Solicitud solicitud);
        public Task<bool> ModificarSolicitud(Solicitud solicitud);
        public Task<bool> EliminarSolicitud(string id);
        public Task<List<Solicitud>> ListarSolicitudTodas();
        public Task<Solicitud> ObtenerSolicitudById(string id);
        public Task<List<Solicitud>> BuscarAfiliadoSolicitudes(int idAfiliado);
        public Task<List<Solicitud>> BuscarOfertaSolicitudes(string idOferta);
    }
}

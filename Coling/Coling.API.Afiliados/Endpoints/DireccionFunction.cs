using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Coling.API.Afiliados.Endpoints
{
    public class DireccionFunction
    {
        private readonly ILogger<DireccionFunction> _logger;
        private readonly IDireccionLogic _direccionLogic;

        public DireccionFunction(ILogger<DireccionFunction> logger,IDireccionLogic direccionLogic)
        {
            _logger = logger;
            this._direccionLogic = direccionLogic;
        }

        [Function("ListarDirecciones")]
        public async Task<HttpResponseData> ListarDirecciones([HttpTrigger(AuthorizationLevel.Function, "get", Route ="listardirecciones")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Direcciones");
            try
            {
                var listaDirecciones = _direccionLogic.ListarDireccionTodas();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaDirecciones.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("ObtenerDireccionById")]
        public async Task<HttpResponseData> ObtenerDireccionById([HttpTrigger(AuthorizationLevel.Function,"get",Route = "obtenerDireccionById/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener una Direccion");
            try
            {
                var direccion = _direccionLogic.ObtenerDireccionById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(direccion.Result);
                return respuesta;

            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }                
        }
        [Function("InsertarDireccion")]
        public async Task<HttpResponseData> InsertarDireccion([HttpTrigger(AuthorizationLevel.Function,"post",Route = "insertarDireccion")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar una Direccion");
            try
            {
                var direccion = await req.ReadFromJsonAsync<Direccion>() ?? throw new Exception("Debe ingresar una Direccion con todos sus datos");
                bool seGuardo = await _direccionLogic.InsertarDireccion(direccion);
                if (seGuardo)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("ModificarDireccion")]
        public async Task<HttpResponseData> ModificarDireccion([HttpTrigger(AuthorizationLevel.Function,"put",Route = "modificarDireccion/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar una Direccion");
            try
            {
                var direccion = await req.ReadFromJsonAsync<Direccion>()?? throw new Exception("Debe ingresar una Direccion con todos sus datos");
                bool seModifico = await _direccionLogic.ModificarDireccion(direccion,id);
                if (seModifico)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {

                var error=req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("EliminarDireccion")]
        public async Task<HttpResponseData> EliminarDireccion([HttpTrigger(AuthorizationLevel.Function,"delete",Route = "eliminarDireccion/{id}")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar una Direccion");
            try
            {
                bool seElimino = await _direccionLogic.EliminarDireccion(id);
                if (seElimino)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;   
                }
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {

                var error=req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
    }
}

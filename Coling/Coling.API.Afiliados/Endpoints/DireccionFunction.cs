using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.DTO;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
        [OpenApiOperation("Listarspec", "ListarDirecciones", Description = "Sirve para listar todos las Direcciones")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<DireccionDTO>), Description = "Mostrara una Lista de Direcciones")]
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
        [OpenApiOperation("Obtenerspec", "ObtenerDireccionById", Description = "Sirve para obtener una Direccion")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DireccionDTO), Description = "Mostrara una Direccion")]
        public async Task<HttpResponseData> ObtenerDireccionById([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "obtenerDireccionById/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener una Direccion");
            try
            {
                var direccion = _direccionLogic.ObtenerDireccionById(id);
                if (direccion.Result != null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(direccion.Result);
                    return respuesta;
                }
                else
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.NotFound);
                    return respuesta;
                }
               

            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }                
        }
        [Function("BuscarDirecciones")]
        [OpenApiOperation("Buscarspec", "BuscarDirecciones", Description = "Sirve para listar todos las Direcciones de una Persona")]
        [OpenApiParameter(name: "idPersona", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<DireccionDTO>), Description = "Mostrara una Lista de Direcciones de una Persona")]
        public async Task<HttpResponseData> BuscarDirecciones([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "BuscarDirecciones/{idPersona}")] HttpRequestData req,int idPersona)
        {
            
            try
            {
                var listaDirecciones = _direccionLogic.BuscarPersonaDirecciones(idPersona);
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
        [Function("InsertarDireccion")]
        [OpenApiOperation("Insertarspec", "InsertarDireccion", Description = "Sirve para Insertar una Direccion")]
        [OpenApiRequestBody("application/json", typeof(DireccionDTO), Description = "Direccion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DireccionDTO), Description = "Mostrara la Direccion Creada")]
        public async Task<HttpResponseData> InsertarDireccion([HttpTrigger(AuthorizationLevel.Anonymous, "post",Route = "insertarDireccion")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar una Direccion");
            try
            {
                var direccion = await req.ReadFromJsonAsync<DireccionDTO>() ?? throw new Exception("Debe ingresar una Direccion con todos sus datos");               
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
        [OpenApiOperation("Modificarspec", "ModificarDireccion", Description = "Sirve para Modificar una Direccion")]
        [OpenApiRequestBody("application/json", typeof(DireccionDTO), Description = "Direccion modelo")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DireccionDTO), Description = "Mostrara la Direccion Modificada")]
        public async Task<HttpResponseData> ModificarDireccion([HttpTrigger(AuthorizationLevel.Anonymous, "put",Route = "modificarDireccion/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar una Direccion");
            try
            {
                var direccion = await req.ReadFromJsonAsync<DireccionDTO>()?? throw new Exception("Debe ingresar una Direccion con todos sus datos");
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
        [OpenApiOperation("Eliminarspec", "EliminarDireccion", Description = "Sirve para Eliminar una Direccion")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarDireccion([HttpTrigger(AuthorizationLevel.Anonymous, "delete",Route = "eliminarDireccion/{id}")] HttpRequestData req, int id) 
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

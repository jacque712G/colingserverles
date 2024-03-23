using Coling.API.Bolsatrabajo.Contratos;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Coling.API.Bolsatrabajo.EndPoints
{
    public class SolicitudFunction
    {
        private readonly ILogger<SolicitudFunction> _logger;
        private readonly ISolicitudLogic repos;

        public SolicitudFunction(ILogger<SolicitudFunction> logger,ISolicitudLogic repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("ListarSolicitudes")]
        [OpenApiOperation("Listarspec", "ListarSolicitudes", Description = "Sirve para listar todas las Solicitudes")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Solicitud>), Description = "Mostrara una Lista de Solicitudes")]
        public async Task<HttpResponseData> ListarSolicitudes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarSolicitudes")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var listaSolicitudes = repos.ListarSolicitudTodas();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaSolicitudes.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                await respuesta.WriteAsJsonAsync(e.Message);
                return respuesta;
            }
        }

        [Function("ObtenerSolicitudById")]
        [OpenApiOperation("Obtenerspec", "ObtenerSolicitudById", Description = "Sirve para obtener una Solicitud")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Solicitud), Description = "Mostrara una Solicitud")]
        public async Task<HttpResponseData> ObtenerSolicitudById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ObtenerSolicitudById/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                var solicitud = repos.ObtenerSolicitudById(id);
                if (solicitud.Result != null)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(solicitud.Result);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.NotFound);
                    return respuesta;
                }

            }
            catch (Exception e)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                await respuesta.WriteAsJsonAsync(e.Message);
                return respuesta;
            }
        }
        [Function("BuscarAfiliadoSolicitudes")]
        [OpenApiOperation("Buscarspec", "BuscarAfiliadoSolicitudes", Description = "Sirve para listar todas las Solicitudes de un Afiliado")]
        [OpenApiParameter(name: "idAfiliado", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Solicitud>), Description = "Mostrara una Lista de Solicitudes de un Afiliado")]
        public async Task<HttpResponseData> BuscarAfiliadoSolicitudes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "BuscarAfiliadoSolicitudes/{idAfiliado}")] HttpRequestData req, int idAfiliado)
        {
            HttpResponseData respuesta;
            try
            {
                var listaSolicitudes = repos.BuscarAfiliadoSolicitudes(idAfiliado);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaSolicitudes.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                await respuesta.WriteAsJsonAsync(e.Message);
                return respuesta;
            }
        }
        [Function("BuscarOfertaSolicitudes")]
        [OpenApiOperation("BuscarSolspec", "BuscarOfertaSolicitudes", Description = "Sirve para listar todas las Solicitudes de una Oferta")]
        [OpenApiParameter(name: "idOferta", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Solicitud>), Description = "Mostrara una Lista de Solicitudes de una Oferta")]
        public async Task<HttpResponseData> BuscarOfertaSolicitudes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "BuscarOfertaSolicitudes/{idOferta}")] HttpRequestData req, string idOferta)
        {
            HttpResponseData respuesta;
            try
            {
                var listaSolicitudes = repos.BuscarOfertaSolicitudes(idOferta);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaSolicitudes.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                await respuesta.WriteAsJsonAsync(e.Message);
                return respuesta;
            }
        }
        [Function("InsertarSolicitud")]
        [OpenApiOperation("Insertarspec", "InsertarSolicitud", Description = "Sirve para Insertar una Solicitud")]
        [OpenApiRequestBody("application/json", typeof(Solicitud), Description = "Solicitud modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Solicitud), Description = "Mostrara la Solicitud Creada")]
        public async Task<HttpResponseData> InsertarSolicitud([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "InsertarSolicitud")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Solicitud>() ?? throw new Exception("Debe ingresar una Solicitud con todos sus datos");
                bool seGuardo = await repos.InsertarSolicitud(registro);
                if (seGuardo)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception e)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                await respuesta.WriteAsJsonAsync(e.Message);
                return respuesta;
            }
        }

        [Function("ModificarSolicitud")]
        [OpenApiOperation("Modificarspec", "ModificarSolicitud", Description = "Sirve para Modificar una Solicitud")]
        [OpenApiRequestBody("application/json", typeof(Solicitud), Description = "Solicitud modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Solicitud), Description = "Mostrara la Solicitud Modificada")]
        public async Task<HttpResponseData> ModificarSolicitud([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ModificarSolicitud")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var solicitud = await req.ReadFromJsonAsync<Solicitud>() ?? throw new Exception("Debe ingresar una Solicitud con todos sus datos");
                bool seModifico = await repos.ModificarSolicitud(solicitud);
                if (seModifico)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception e)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                await respuesta.WriteAsJsonAsync(e.Message);
                return respuesta;
            }
        }

        [Function("EliminarSolicitud")]
        [OpenApiOperation("Eliminarspec", "EliminarSolicitud", Description = "Sirve para Eliminar una Solicitud")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        public async Task<HttpResponseData> EliminarSolicitud([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "EliminarSolicitud/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                bool seElimino = await repos.EliminarSolicitud(id);
                if (seElimino)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception e)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                await respuesta.WriteAsJsonAsync(e.Message);
                return respuesta;
            }
        }
    }
}

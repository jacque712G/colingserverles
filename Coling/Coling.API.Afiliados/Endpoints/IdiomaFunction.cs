using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Grpc.Core;
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
    public class IdiomaFunction
    {
        private readonly ILogger<IdiomaFunction> _logger;
        private readonly IIdiomaLogic _idiomaLogic;

        public IdiomaFunction(ILogger<IdiomaFunction> logger,IIdiomaLogic idiomaLogic)
        {
            _logger = logger;
            _idiomaLogic = idiomaLogic;
        }

        [Function("ListarIdiomas")]
        [OpenApiOperation("Listarspec", "ListarIdiomas", Description = "Sirve para listar todos los Idiomas")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Idioma>), Description = "Mostrara una Lista de Idiomas")]
        public async Task<HttpResponseData> ListarIdiomas([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="listaridiomas")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Idiomas");
            try
            {
                var listaIdiomas = _idiomaLogic.ListarIdiomaTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaIdiomas.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("ObtenerIdiomaById")]
        [OpenApiOperation("Obtenerspec", "ObtenerIdiomaById", Description = "Sirve para obtener un Idioma")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Idioma), Description = "Mostrara un Idioma")]
        public async Task<HttpResponseData> ObtenerIdiomaById([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "obtenerIdiomaById/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener un Idioma");
            try
            {
                var idioma = _idiomaLogic.ObtenerIdiomaById(id);
                if (idioma.Result!=null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(idioma.Result);
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

                var error= req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("InsertarIdioma")]
        [OpenApiOperation("Insertarspec", "InsertarIdioma", Description = "Sirve para Insertar un Idioma")]
        [OpenApiRequestBody("application/json", typeof(Idioma), Description = "Idioma modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Idioma), Description = "Mostrara el Idioma Creado")]
        public async Task<HttpResponseData> InsertarIdioma([HttpTrigger(AuthorizationLevel.Anonymous, "post",Route = "insertarIdioma")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar un Idioma");
            try
            {
                var idioma = await req.ReadFromJsonAsync<Idioma>()??throw new Exception("Debe ingresar un Idioma con todos sus datos");
                bool seGuardo = await _idiomaLogic.InsertarIdioma(idioma);
                if (seGuardo)
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

        [Function("ModificarIdioma")]
        [OpenApiOperation("Modificarspec", "ModificarIdioma", Description = "Sirve para Modificar un Idioma")]
        [OpenApiRequestBody("application/json", typeof(Idioma), Description = "Idioma modelo")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Idioma), Description = "Mostrara el Idioma Modificado")]
        public async Task<HttpResponseData> ModificarIdioma([HttpTrigger(AuthorizationLevel.Anonymous, "put",Route = "modificarIdioma/{id}")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar un Idioma");
            try
            {
                var idioma = await req.ReadFromJsonAsync<Idioma>() ??throw new Exception("Debe ingresar un Idioma con todos sus datos");
                bool seModifico = await _idiomaLogic.ModificarIdioma(idioma,id);
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
        [Function("EliminarIdioma")]
        [OpenApiOperation("Eliminarspec", "EliminarIdioma", Description = "Sirve para Eliminar un Idioma")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarIdioma([HttpTrigger(AuthorizationLevel.Anonymous, "delete",Route= "eliminarIdioma/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar un Idioma");
            try
            {
                bool seElimino = await _idiomaLogic.EiminarIdioma(id);
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

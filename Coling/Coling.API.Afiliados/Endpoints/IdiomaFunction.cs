using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
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
        public async Task<HttpResponseData> ListarIdiomas([HttpTrigger(AuthorizationLevel.Function, "get", Route ="listaridiomas")] HttpRequestData req)
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
        public async Task<HttpResponseData> ObtenerIdiomaById([HttpTrigger(AuthorizationLevel.Function,"get",Route = "obtenerIdiomaById/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener un Idioma");
            try
            {
                var idioma = _idiomaLogic.ObtenerIdiomaById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(idioma);
                return respuesta;
            }
            catch (Exception e)
            {

                var error= req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("InsertarIdioma")]
        public async Task<HttpResponseData> InsertarIdioma([HttpTrigger(AuthorizationLevel.Function,"post",Route = "insertarIdioma")] HttpRequestData req) 
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
        public async Task<HttpResponseData> ModificarIdioma([HttpTrigger(AuthorizationLevel.Function,"put",Route = "modificarIdioma/{id}")] HttpRequestData req, int id) 
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
        public async Task<HttpResponseData> EliminarIdioma([HttpTrigger(AuthorizationLevel.Function,"delete",Route= "eliminarIdioma/{id}")] HttpRequestData req,int id) 
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

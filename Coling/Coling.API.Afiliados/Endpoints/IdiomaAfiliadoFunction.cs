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
    public class IdiomaAfiliadoFunction
    {
        private readonly ILogger<IdiomaAfiliadoFunction> _logger;
        private readonly IIdiomaAfiliadoLogic _idiomaAfiliadoLogic;

        public IdiomaAfiliadoFunction(ILogger<IdiomaAfiliadoFunction> logger,IIdiomaAfiliadoLogic idiomaAfiliadoLogic)
        {
            _logger = logger;
            _idiomaAfiliadoLogic = idiomaAfiliadoLogic;
        }

        [Function("ListarIdiomasAfiliados")]
        public async Task<HttpResponseData> ListarIdiomasAfiliados([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listaridiomasafiliados")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Idiomas Afiliados");
            try
            {
                var listaIdiomasAfiliados = _idiomaAfiliadoLogic.ListarIdiomaAfiliadoTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaIdiomasAfiliados.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error=req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;   
            }
           
        }
        [Function("ObtenerIdiomaAfiliadoById")]
        public async Task<HttpResponseData> ObtenerIdiomaAfiliadoById([HttpTrigger(AuthorizationLevel.Function,"get",Route = "obtenerIdiomaAfiliadoById")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener un Idioma Afiliado");
            try
            {
                var idiomaAfiliado = _idiomaAfiliadoLogic.ObtenerIdiomaAfiliadoById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(idiomaAfiliado.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error=req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("InsertarIdiomaAfiliado")]
        public async Task<HttpResponseData> InsertarIdiomaAfiliado([HttpTrigger(AuthorizationLevel.Function,"post",Route = "insertarIdiomaAfiliado")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Idioma Afiliado");
            try
            {
                var idiomaAfiliado = await req.ReadFromJsonAsync<IdiomaAfiliado>()??throw new Exception("Debe ingresar un Idioma Afiliado con todos sus datos");
                bool seGuardo=await _idiomaAfiliadoLogic.InsertarIdiomaAfiliado(idiomaAfiliado);
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
        [Function("ModificarIdiomaAfiliado")]
        public async Task<HttpResponseData> ModificarIdiomaAfiliado([HttpTrigger(AuthorizationLevel.Function,"put",Route = "modificarIdiomaAfiliado/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar Idioma Afiliado");
            try
            {
                var idiomaAfiliado = await req.ReadFromJsonAsync<IdiomaAfiliado>() ??throw new Exception("Debe ingresar un Idioma Afiliado con todos sus datos");
                bool seModifico = await _idiomaAfiliadoLogic.ModificarIdiomaAfiliado(idiomaAfiliado,id);
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
        [Function("EliminarIdiomaAfiliado")]
        public async Task<HttpResponseData> EliminarIdiomaAfiliado([HttpTrigger(AuthorizationLevel.Function,"delete", Route = "eliminarIdiomaAfiliado/{id}")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar Idioma Afiliado");
            try
            {
                bool seElimino = await _idiomaAfiliadoLogic.EliminarIdiomaAfiliado(id);
                if (seElimino)
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
    }
}

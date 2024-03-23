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
        [OpenApiOperation("Listarspec", "ListarIdiomasAfiliados", Description = "Sirve para listar todos los Idiomas Afiliados")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<IdiomaAfiliadoDTO>), Description = "Mostrara una Lista de Idiomas Afiliados")]
        public async Task<HttpResponseData> ListarIdiomasAfiliados([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listaridiomasafiliados")] HttpRequestData req)
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
        [OpenApiOperation("Obtenerspec", "ObtenerIdiomaAfiliadoById", Description = "Sirve para obtener un Idioma Afiliado")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IdiomaAfiliadoDTO), Description = "Mostrara un Idioma Afiliado")]
        public async Task<HttpResponseData> ObtenerIdiomaAfiliadoById([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "obtenerIdiomaAfiliadoById/{id}")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener un Idioma Afiliado");
            try
            {
                var idiomaAfiliado = _idiomaAfiliadoLogic.ObtenerIdiomaAfiliadoById(id);
                if (idiomaAfiliado.Result!=null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(idiomaAfiliado.Result);
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

                var error=req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("BuscarIdiomasAfiliados")]
        [OpenApiOperation("Buscarspec", "BuscarIdiomasAfiliados", Description = "Sirve para listar todos los Idiomas de un Afiliado")]
        [OpenApiParameter(name: "idAfiliado", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<IdiomaDTO>), Description = "Mostrara una Lista de Idiomas de un Afiliado")]
        public async Task<HttpResponseData> BuscarIdiomasAfiliados([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "BuscarIdiomasAfiliados/{idAfiliado}")] HttpRequestData req,int idAfiliado)
        {
            
            try
            {
                var listaIdiomasAfiliados = _idiomaAfiliadoLogic.BuscarAfiliadoIdiomas(idAfiliado);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaIdiomasAfiliados.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }
        [Function("InsertarIdiomaAfiliado")]
        [OpenApiOperation("Insertarspec", "InsertarIdiomaAfiliado", Description = "Sirve para Insertar un Idioma Afiliado")]
        [OpenApiRequestBody("application/json", typeof(IdiomaAfiliadoDTO), Description = "Idioma Afiliado modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IdiomaAfiliadoDTO), Description = "Mostrara el Idioma Afiliado Creado")]
        public async Task<HttpResponseData> InsertarIdiomaAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "post",Route = "insertarIdiomaAfiliado")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Idioma Afiliado");
            try
            {
                var idiomaAfiliado = await req.ReadFromJsonAsync<IdiomaAfiliadoDTO>()??throw new Exception("Debe ingresar un Idioma Afiliado con todos sus datos");
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
        [OpenApiOperation("Modificarspec", "ModificarIdiomaAfiliado", Description = "Sirve para Modificar un Idioma Afiliado")]
        [OpenApiRequestBody("application/json", typeof(IdiomaAfiliadoDTO), Description = "Idioma Afiliado modelo")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IdiomaAfiliadoDTO), Description = "Mostrara el Idioma Afiliado Modificado")]
        public async Task<HttpResponseData> ModificarIdiomaAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "put",Route = "modificarIdiomaAfiliado/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar Idioma Afiliado");
            try
            {
                var idiomaAfiliado = await req.ReadFromJsonAsync<IdiomaAfiliadoDTO>() ??throw new Exception("Debe ingresar un Idioma Afiliado con todos sus datos");
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
        [OpenApiOperation("Eliminarspec", "EliminarIdiomaAfiliado", Description = "Sirve para Eliminar un Idioma Afiliado")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarIdiomaAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarIdiomaAfiliado/{id}")] HttpRequestData req, int id) 
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

using Coling.API.Afiliados.Contratos;
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
    public class AfiliadoFunction
    {
        private readonly ILogger<AfiliadoFunction> _logger;
        private readonly IAfiliadoLogic _afiliadoLogic;

        public AfiliadoFunction(ILogger<AfiliadoFunction> logger,IAfiliadoLogic _afiliadoLogic)
        {
            _logger = logger;
            this._afiliadoLogic = _afiliadoLogic;
        }

        [Function("ListarAfiliados")]        
        [OpenApiOperation("Listarspec", "ListarAfiliados", Description = "Sirve para listar todos los Afiliados")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Afiliado>), Description = "Mostrara una Lista de Afiliados")]
        public async Task<HttpResponseData> ListarAfiliados([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="listarafiliados")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Afiliados");
            try
            {
                var listaAfiliados = _afiliadoLogic.ListarAfiliadoTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaAfiliados.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("ObtenerAfiliadoById")]
        [OpenApiOperation("Obtenerspec", "ObtenerAfiliadoById", Description = "Sirve para obtener un Afiliado")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Afiliado), Description = "Mostrara un Afiliado")]
        public async Task<HttpResponseData> ObtenerAfiliadoById([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "obtenerAfiliadoById/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener a un Afiliado");
            try
            {
                var afiliado = _afiliadoLogic.ObtenerAfiliadoById(id);
                if (afiliado.Result!=null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(afiliado.Result);
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
        [Function("InsertarAfiliado")]
        [OpenApiOperation("Insertarspec", "InsertarAfiliado", Description = "Sirve para Insertar un Afiliado")]
        [OpenApiRequestBody("application/json", typeof(Afiliado), Description = "Afiliado modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Afiliado), Description = "Mostrara el Afiliado Creado")]
        public async Task<HttpResponseData> InsertarAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "post",Route = "insertarAfiliado")] HttpRequestData req)         
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar un Afiliado");
            try
            {
                var afiliado = await req.ReadFromJsonAsync<Afiliado>()??throw new Exception("Debe ingresar un afiliado con todos sus datos");
                bool seGuardo = await _afiliadoLogic.InsertarAfiliado(afiliado);
                if (seGuardo)
                {
                    var afiliadoCreado = _afiliadoLogic.ObtenerAfiliadoById(afiliado.Id);
                    var respuesta=req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(afiliadoCreado.Result);                    
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
        [Function("ModificarAfiliado")]
        [OpenApiOperation("Modificarspec", "ModificarAfiliado", Description = "Sirve para Modificar un Afiliado")]
        [OpenApiRequestBody("application/json", typeof(Afiliado), Description = "Afiliado modelo")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Afiliado), Description = "Mostrara el Afiliado Modificado")]
        public async Task<HttpResponseData> ModificarAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "put",Route = "modificarAfiliado/{id}")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar un Afiliado");
            try
            {
                var afiliado = await req.ReadFromJsonAsync<Afiliado>()??throw new Exception("Debe ingresar un afiliado con todos sus datos");
                bool seModifico = await _afiliadoLogic.ModificarAfiliado(afiliado,id);
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

        [Function("EliminarAfiliado")]
        [OpenApiOperation("Eliminarspec", "EliminarAfiliado", Description = "Sirve para Eliminar un Estudio")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "delete",Route = "eliminarAfiliado/{id}")]HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar un Afiliado");
            try
            {
                bool seElimino = await _afiliadoLogic.EliminarAfiliado(id);
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

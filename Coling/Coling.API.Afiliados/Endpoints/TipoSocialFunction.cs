using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.Implementacion;
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
    public class TipoSocialFunction
    {
        private readonly ILogger<TipoSocialFunction> _logger;
        private readonly ITipoSocialLogic _tipoSocialLogic;

        public TipoSocialFunction(ILogger<TipoSocialFunction> logger,ITipoSocialLogic tipoSocialLogic)
        {
            _logger = logger;
            _tipoSocialLogic = tipoSocialLogic;
        }

        [Function("ListarTiposSociales")]
        [OpenApiOperation("Listarspec", "ListarTiposSociales", Description = "Sirve para listar todos los Tipos Sociales")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<TipoSocial>), Description = "Mostrara una Lista de Tipos Sociales")]
        public async Task<HttpResponseData> ListarTiposSociales([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="listartipossociales")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Tipos Sociales");
            try
            {
                var listaTiposSociales = _tipoSocialLogic.ListarTipoSocialTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaTiposSociales.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
           
        }
        [Function("ObtenerTipoSocialById")]
        [OpenApiOperation("Obtenerspec", "ObtenerTipoSocialById", Description = "Sirve para obtener un Tipo Social")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TipoSocial), Description = "Mostrara un Tipo Social")]
        public async Task<HttpResponseData> ObtenerTipoSocialById([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "obtenerTipoSocialById/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener un Tipo Social");
            try
            {
                var tipoSocial = _tipoSocialLogic.ObtenerTipoSocialById(id);
                if (tipoSocial.Result!=null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(tipoSocial.Result);
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
        [Function("InsertarTipoSocial")]
        [OpenApiOperation("Insertarspec", "InsertarTipoSocial", Description = "Sirve para Insertar un Tipo Social")]
        [OpenApiRequestBody("application/json", typeof(TipoSocial), Description = "Tipo Social modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TipoSocial), Description = "Mostrara el Tipo Social Creado")]
        public async Task<HttpResponseData> InsertarTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "post",Route = "insertarTipoSocial")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar un Tipo Social");
            try
            {
                var tipoSocial = await req.ReadFromJsonAsync<TipoSocial>()?? throw new Exception("Debe ingresar un Tipo Social con todos sus datos");
                bool seGuardo=await _tipoSocialLogic.InsertarTipoSocial(tipoSocial);
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
        [Function("ModificarTipoSocial")]
        [OpenApiOperation("Modificarspec", "ModificarTipoSocial", Description = "Sirve para Modificar un Tipo Social")]
        [OpenApiRequestBody("application/json", typeof(TipoSocial), Description = "Tipo Social modelo")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TipoSocial), Description = "Mostrara el Tipo Social Modificado")]
        public async Task<HttpResponseData> ModificarTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarTipoSocial/{id}")] HttpRequestData req,int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar un Tipo Social");
            try
            {
                var tipoSocial = await req.ReadFromJsonAsync<TipoSocial>() ?? throw new Exception("Debe ingresar un Tipo Social con todos sus datos");
                bool seGuardo = await _tipoSocialLogic.ModificarTipoSocial(tipoSocial,id);
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
        [Function("EliminarTipoSocial")]
        [OpenApiOperation("Eliminarspec", "EliminarTipoSocial", Description = "Sirve para Eliminar un Tipo Social")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarTipoSocial/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar un Tipo Social");
            try
            {
                bool seElimino = await _tipoSocialLogic.EliminarTipoSocial(id);
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

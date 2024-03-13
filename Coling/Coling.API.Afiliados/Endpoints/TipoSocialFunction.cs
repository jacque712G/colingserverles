using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.Implementacion;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
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
        public async Task<HttpResponseData> ListarTiposSociales([HttpTrigger(AuthorizationLevel.Function, "get", Route ="listartipossociales")] HttpRequestData req)
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
        public async Task<HttpResponseData> ObtenerTipoSocialById([HttpTrigger(AuthorizationLevel.Function,"get",Route = "obtenerTipoSocialById")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener un Tipo Social");
            try
            {
                var tipoSocial = _tipoSocialLogic.ObtenerTipoSocialById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(tipoSocial.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error=req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
            
        }
        [Function("InsertarTipoSocial")]
        public async Task<HttpResponseData> InsertarTipoSocial([HttpTrigger(AuthorizationLevel.Function,"post",Route = "insertarTipoSocial")] HttpRequestData req) 
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
        public async Task<HttpResponseData> ModificarTipoSocial([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarTipoSocial/{id}")] HttpRequestData req,int id)
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
        public async Task<HttpResponseData> EliminarTipoSocial([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarTipoSocial/{id}")] HttpRequestData req, int id)
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

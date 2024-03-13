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
    public class PersonaTipoSocialFunction
    {
        private readonly ILogger<PersonaTipoSocialFunction> _logger;
        private readonly IPersonaTipoSocialLogic _personaTipoSocialLogic;

        public PersonaTipoSocialFunction(ILogger<PersonaTipoSocialFunction> logger,IPersonaTipoSocialLogic personaTipoSocialLogic)
        {
            _logger = logger;
            _personaTipoSocialLogic= personaTipoSocialLogic;
        }

        [Function("ListarPersonaTiposSociales")]
        public async Task<HttpResponseData> ListarPersonaTiposSociales([HttpTrigger(AuthorizationLevel.Function, "get", Route ="listarpersonatipossociales")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Personas Tipos Sociales");
            try
            {
                var listaPersonaTiposSociales = _personaTipoSocialLogic.ListarPersonaTipoSocialTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaPersonaTiposSociales.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("ObtenerPersonaTipoSocialById")]
        public async Task<HttpResponseData> ObtenerPersonaTipoSocialById([HttpTrigger(AuthorizationLevel.Function,"get",Route = "obtenerPersonaTipoSocialById")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener una Persona Tipo Social");
            try
            {
                var personaTipoSocial = _personaTipoSocialLogic.ObtenerPersonaTipoSocialById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(personaTipoSocial.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("InsertarPersonaTipoSocial")]
        public async Task<HttpResponseData> InsertarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Function,"post",Route = "insertarPersonaTipoSocial")]HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Persona Tipo Social");
            try
            {
                var personaTipoSocial = await req.ReadFromJsonAsync<PersonaTipoSocial>()??throw new Exception("Debe ingresar una Persona Tipo Social con todos sus datos");
                bool seGuardo = await _personaTipoSocialLogic.InsertarPersonaTipoSocial(personaTipoSocial);
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

        [Function("ModificarPersonaTipoSocial")]
        public async Task<HttpResponseData> ModificarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarPersonaTipoSocial/{id}")] HttpRequestData req,int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar Persona Tipo Social");
            try
            {
                var personaTipoSocial = await req.ReadFromJsonAsync<PersonaTipoSocial>() ?? throw new Exception("Debe ingresar una Persona Tipo Social con todos sus datos");
                bool seModifico = await _personaTipoSocialLogic.ModificarPersonaTipoSocial(personaTipoSocial,id);
                if (seModifico)
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
        [Function("EliminarPersonaTipoSocial")]
        public async Task<HttpResponseData> EliminarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarPersonaTipoSocial/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar Persona Tipo Social");
            try
            {
                bool seElimino = await _personaTipoSocialLogic.EliminarPersonaTipoSocial(id);
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

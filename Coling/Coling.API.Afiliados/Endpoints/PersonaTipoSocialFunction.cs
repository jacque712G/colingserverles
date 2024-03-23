using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.DTO;
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
        [OpenApiOperation("Listarspec", "ListarPersonaTiposSociales", Description = "Sirve para listar todos las Personas Tipos Sociales")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<PersonaTipoSocialDTO>), Description = "Mostrara una Lista de Personas Tipos Sociales")]
        public async Task<HttpResponseData> ListarPersonaTiposSociales([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="listarpersonatipossociales")] HttpRequestData req)
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
        [OpenApiOperation("Obtenerspec", "ObtenerPersonaTipoSocialById", Description = "Sirve para obtener una Persona Tipo Social")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PersonaTipoSocialDTO), Description = "Mostrara una Persona Tipo Social")]
        public async Task<HttpResponseData> ObtenerPersonaTipoSocialById([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "obtenerPersonaTipoSocialById/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener una Persona Tipo Social");
            try
            {
                var personaTipoSocial = _personaTipoSocialLogic.ObtenerPersonaTipoSocialById(id);
                if (personaTipoSocial.Result!=null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(personaTipoSocial.Result);
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

        [Function("BuscarPersonaTiposSociales")]
        [OpenApiOperation("Buscarspec", "BuscarPersonaTiposSociales", Description = "Sirve para listar todos los Tipos Sociales de una Persona")]
        [OpenApiParameter(name: "idPersona", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<TipoSocialDTO>), Description = "Mostrara una Lista de Tipos Sociales de una Persona")]
        public async Task<HttpResponseData> BuscarPersonaTiposSociales([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "BuscarPersonaTiposSociales/{idPersona}")] HttpRequestData req,int idPersona)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Personas Tipos Sociales");
            try
            {
                var listaPersonaTiposSociales = _personaTipoSocialLogic.BuscarPersonaTiposSociales(idPersona);
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
        [Function("InsertarPersonaTipoSocial")]
        [OpenApiOperation("Insertarspec", "InsertarPersonaTipoSocial", Description = "Sirve para Insertar una Persona Tipo Social")]
        [OpenApiRequestBody("application/json", typeof(PersonaTipoSocialDTO), Description = "Persona Tipo Social modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PersonaTipoSocialDTO), Description = "Mostrara la Persona Tipo Social Creada")]
        public async Task<HttpResponseData> InsertarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "post",Route = "insertarPersonaTipoSocial")]HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Persona Tipo Social");
            try
            {
                var personaTipoSocial = await req.ReadFromJsonAsync<PersonaTipoSocialDTO>()??throw new Exception("Debe ingresar una Persona Tipo Social con todos sus datos");
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
        [OpenApiOperation("Modificarspec", "ModificarPersonaTipoSocial", Description = "Sirve para Modificar una Persona Tipo Social")]
        [OpenApiRequestBody("application/json", typeof(PersonaTipoSocialDTO), Description = "Persona Tipo Social modelo")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PersonaTipoSocialDTO), Description = "Mostrara la Persona Tipo Social Modificada")]
        public async Task<HttpResponseData> ModificarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarPersonaTipoSocial/{id}")] HttpRequestData req,int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar Persona Tipo Social");
            try
            {
                var personaTipoSocial = await req.ReadFromJsonAsync<PersonaTipoSocialDTO>() ?? throw new Exception("Debe ingresar una Persona Tipo Social con todos sus datos");
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
        [OpenApiOperation("Eliminarspec", "EliminarPersonaTipoSocial", Description = "Sirve para Eliminar una Persona Tipo Social")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarPersonaTipoSocial([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarPersonaTipoSocial/{id}")] HttpRequestData req, int id)
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

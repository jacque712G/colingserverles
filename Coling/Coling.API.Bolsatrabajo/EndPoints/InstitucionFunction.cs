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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Coling.API.Bolsatrabajo.EndPoints
{
    public class InstitucionFunction
    {
        private readonly ILogger<InstitucionFunction> _logger;
        private readonly IInstitucionLogic repos;

        public InstitucionFunction(ILogger<InstitucionFunction> logger,IInstitucionLogic repos)
        {
            _logger = logger;
            this.repos = repos;
        }
        [Function("ListarInstituciones")]
        [OpenApiOperation("Listarspec", "ListarInstituciones", Description = "Sirve para listar todas las Instituciones")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Institucion>), Description = "Mostrara una Lista de Instituciones")]
        public async Task<HttpResponseData> ListarInstituciones([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarInstituciones")] HttpRequestData req)
        {           
            try
            {
                var listaInstituciones = repos.ListarInstitucionTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaInstituciones.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("ObtenerInstitucionById")]
        [OpenApiOperation("Obtenerspec", "ObtenerInstitucionById", Description = "Sirve para obtener una Institucion")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Institucion), Description = "Mostrara una Institucion")]
        public async Task<HttpResponseData> ObtenerInstitucionById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ObtenerInstitucionById/{id}")] HttpRequestData req, string id)
        {
           
            try
            {
                var institucion = repos.ObtenerInstitucionById(id);
                if (institucion.Result != null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(institucion.Result);
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
        [Function("InsertarInstitucion")]
        [OpenApiOperation("Insertarspec", "InsertarInstitucion", Description = "Sirve para Insertar una Institucion")]
        [OpenApiRequestBody("application/json", typeof(Institucion), Description = "Institucion modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Institucion), Description = "Mostrara la Institucion Creada")]
        public async Task<HttpResponseData> InsertarInstitucion([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "InsertarInstitucion")] HttpRequestData req)
        {
            HttpResponseData? respuesta = null;
            try
            {
                var institucion = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("Debe ingresar una Institucion con todos sus datos");
                bool seGuardo=await repos.InsertarInstitucion(institucion);
                if (seGuardo)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta=req.CreateResponse(HttpStatusCode.BadRequest);
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
        [Function("ModificarInstitucion")]
        [OpenApiOperation("Modificarspec", "ModificarInstitucion", Description = "Sirve para Modificar una Institucion")]
        [OpenApiRequestBody("application/json", typeof(Institucion), Description = "Institucion modelo")]       
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Institucion), Description = "Mostrara la Institucion Modificada")]
        public async Task<HttpResponseData> ModificarInstitucion([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ModificarInstitucion")] HttpRequestData req)
        {           
            try
            {
                var institucion = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("Debe ingresar una Institucion con todos sus datos");
                bool seModifico = await repos.ModificarInstitucion(institucion);
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
        [Function("EliminarInstitucion")]
        [OpenApiOperation("Eliminarspec", "EliminarInstitucion", Description = "Sirve para Eliminar una Institucion")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        public async Task<HttpResponseData> EliminarInstitucion([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "EliminarInstitucion/{id}")] HttpRequestData req, string id)
        {
           
            try
            {
                bool seElimino = await repos.EliminarInstitucion(id);
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

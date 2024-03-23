using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.DTO;
using Coling.API.Afiliados.Implementacion;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Coling.API.Afiliados.Endpoints
{
    public class ProfesionAfiliadoFunction
    {
        private readonly ILogger<ProfesionAfiliadoFunction> _logger;
        private readonly IProfesionAfiliadoLogic _profesionAfiliado;

        public ProfesionAfiliadoFunction(ILogger<ProfesionAfiliadoFunction> logger,IProfesionAfiliadoLogic profesionAfiliado)
        {
            _logger = logger;
            _profesionAfiliado = profesionAfiliado;
        }

        [Function("ListarProfesionAfiliado")]
        [OpenApiOperation("Listarspec", "ListarProfesionAfiliado", Description = "Sirve para listar todos las Profesiones Afiliados")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<ProfesionAfiliadoDTO>), Description = "Mostrara una Lista de Profesiones Afiliados")]
        public async Task<HttpResponseData> ListarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="listarprofesionafiliado")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Profesion Afiliado");
            try
            {
                var listaProfesionAfiliado = _profesionAfiliado.ListarProfesionAfliadoTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaProfesionAfiliado.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
            
        }

        [Function("ObtenerProfesionAfiliadoById")]
        [OpenApiOperation("Obtenerspec", "ObtenerProfesionAfiliadoById", Description = "Sirve para obtener una Profesion Afiliado")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ProfesionAfiliadoDTO), Description = "Mostrara una Profesion Afiliado")]
        public async Task<HttpResponseData> ObtenerProfesionAfiliadoById([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "obtenerProfesionAfiliadoById/{id}")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener una Profesion Afiliado");
            try
            {
                var profesionAfiliado = _profesionAfiliado.ObtenerProfesionAfiliadoById(id);
                if (profesionAfiliado.Result!=null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(profesionAfiliado.Result);
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
        [Function("BuscarProfesionesAfiliados")]
        [OpenApiOperation("Listarspec", "BuscarProfesionesAfiliados", Description = "Sirve para listar todos las Profesiones de un Afiliado")]
        [OpenApiParameter(name: "idAfiliado", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<ProfesionAfiliadoDTO>), Description = "Mostrara una Lista de Profesiones de un Afiliado")]
        public async Task<HttpResponseData> BuscarProfesionesAfiliados([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "BuscarProfesionesAfiliados/{idAfiliado}")] HttpRequestData req,int idAfiliado)
        {
            
            try
            {
                var listaProfesionAfiliado = _profesionAfiliado.BuscarAfiliadoProfesiones(idAfiliado);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaProfesionAfiliado.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("InsertarProfesionAfiliado")]
        [OpenApiOperation("Insertarspec", "InsertarProfesionAfiliado", Description = "Sirve para Insertar una Profesion Afiliado")]
        [OpenApiRequestBody("application/json", typeof(ProfesionAfiliadoDTO), Description = "Profesion Afiliado modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ProfesionAfiliadoDTO), Description = "Mostrara la Profesion Afiliado Creada")]
        public async Task<HttpResponseData> InsertarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "post",Route = "insertarProfesionAfiliado")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Profesion Afiliado");
            try
            {
                var profesionAfiliado = await req.ReadFromJsonAsync<ProfesionAfiliadoDTO>()??throw new Exception("Debe ingresar una Profesion Afiliado con todos sus datos");
                bool seGuardo = await _profesionAfiliado.InsertarProfesionAfiliado(profesionAfiliado);
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

        [Function("ModificarProfesionAfiliado")]
        [OpenApiOperation("Modificarspec", "ModificarProfesionAfiliado", Description = "Sirve para Modificar una Profesion Afiliado")]
        [OpenApiRequestBody("application/json", typeof(ProfesionAfiliadoDTO), Description = "Profesion Afiliado modelo")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ProfesionAfiliadoDTO), Description = "Mostrara la Profesion Afiliado Modificada")]
        public async Task<HttpResponseData> ModificarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarProfesionAfiliado/{id}")] HttpRequestData req,int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar Profesion Afiliado");
            try
            {
                var profesionAfiliado = await req.ReadFromJsonAsync<ProfesionAfiliadoDTO>() ?? throw new Exception("Debe ingresar una Profesion Afiliado con todos sus datos");
                bool seModifico = await _profesionAfiliado.ModificarProfesionAfiliado(profesionAfiliado,id);
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
        [Function("EliminarProfesionAfiliado")]
        [OpenApiOperation("Eliminarspec", "EliminarProfesionAfiliado", Description = "Sirve para Eliminar una Profesion Afiliado")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarProfesionAfiliado/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar Profesion Afiliado");
            try
            {
                bool seElimino = await _profesionAfiliado.EliminarProfesionAfiliado(id);
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

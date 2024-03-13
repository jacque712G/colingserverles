using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.Implementacion;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
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
        public async Task<HttpResponseData> ListarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Function, "get", Route ="listarprofesionafiliado")] HttpRequestData req)
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
        public async Task<HttpResponseData> ObtenerProfesionAfiliadoById([HttpTrigger(AuthorizationLevel.Function,"get",Route = "obtenerProfesionAfiliadoById")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener una Profesion Afiliado");
            try
            {
                var profesionAfiliado = _profesionAfiliado.ObtenerProfesionAfiliadoById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(profesionAfiliado.Result);
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
        public async Task<HttpResponseData> InsertarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Function,"post",Route = "insertarProfesionAfiliado")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Profesion Afiliado");
            try
            {
                var profesionAfiliado = await req.ReadFromJsonAsync<ProfesionAfiliado>()??throw new Exception("Debe ingresar una Profesion Afiliado con todos sus datos");
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
        public async Task<HttpResponseData> ModificarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarProfesionAfiliado/{id}")] HttpRequestData req,int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar Profesion Afiliado");
            try
            {
                var profesionAfiliado = await req.ReadFromJsonAsync<ProfesionAfiliado>() ?? throw new Exception("Debe ingresar una Profesion Afiliado con todos sus datos");
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
        public async Task<HttpResponseData> EliminarProfesionAfiliado([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarProfesionAfiliado/{id}")] HttpRequestData req, int id)
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

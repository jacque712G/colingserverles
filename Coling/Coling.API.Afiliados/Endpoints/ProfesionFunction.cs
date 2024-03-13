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
    public class ProfesionFunction
    {
        private readonly ILogger<ProfesionFunction> _logger;
        private readonly IProfesionLogic _profesionLogic;

        public ProfesionFunction(ILogger<ProfesionFunction> logger,IProfesionLogic profesionLogic)
        {
            _logger = logger;
            _profesionLogic = profesionLogic;
        }

        [Function("ListarProfesiones")]
        public async Task<HttpResponseData> ListarProfesiones([HttpTrigger(AuthorizationLevel.Function, "get", Route="listarprofesiones")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Profesiones");
            try
            {
                var listaProfesiones = _profesionLogic.ListarProfesionTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaProfesiones.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("ObtenerProfesionById")]
        public async Task<HttpResponseData> ObtenerProfesionById([HttpTrigger(AuthorizationLevel.Function,"get",Route = "obtenerProfesionById")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener una Profesion");
            try
            {
                var profesion = _profesionLogic.ObtenerProfesionById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(profesion.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("InsertarProfesion")]
        public async Task<HttpResponseData> InsertarProfesion([HttpTrigger(AuthorizationLevel.Function,"post",Route = "InsertarProfesion")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Profesion");
            try
            {
                var profesion = await req.ReadFromJsonAsync<Profesion>() ?? throw new Exception("Debe ingresar una Profesion con todos sus datos");
                bool seGuardo = await _profesionLogic.InsertarProfesion(profesion);
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

        [Function("ModificarProfesion")]
        public async Task<HttpResponseData> ModificarProfesion([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarProfesion/{id}")] HttpRequestData req,int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar una Profesion");
            try
            {
                var profesion = await req.ReadFromJsonAsync<Profesion>() ?? throw new Exception("Debe ingresar una Profesion con todos sus datos");
                bool seModifico = await _profesionLogic.ModificarProfesion(profesion,id);
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
        [Function("EliminarProfesion")]
        public async Task<HttpResponseData> EliminarProfesion([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarProfesion/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar una Profesion");
            try
            {
                bool seElimino = await _profesionLogic.EliminarProfesion(id);
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

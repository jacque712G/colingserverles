using Coling.API.Afiliados.Contratos;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Coling.API.Afiliados.Endpoints
{
    public class GradoAcademicoFunction
    {
        private readonly ILogger<GradoAcademicoFunction> _logger;
        private readonly IGradoAcademicoLogic _gradoLogic;

        public GradoAcademicoFunction(ILogger<GradoAcademicoFunction> logger,IGradoAcademicoLogic gradoLogic)
        {
            _logger = logger;
            _gradoLogic = gradoLogic;
        }

        [Function("ListarGrados")]
        public async Task<HttpResponseData> ListarGrados([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listargrados")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Grados");
            try
            {
                var listagrados = _gradoLogic.ListarGradoTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listagrados.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("")]
        public async Task<HttpResponseData> ObtenerGradoById([HttpTrigger(AuthorizationLevel.Function,"get",Route = "obtenerGradoById/{id}")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener un Grado Academico");
            try
            {
                var grado = _gradoLogic.ObtenerGradoById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(grado.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;   
            }
        }
        [Function("InsertarGrado")]
        public async Task<HttpResponseData> InsertarGrado([HttpTrigger(AuthorizationLevel.Function,"post",Route = "insertarGrado")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar Grado Academico");
            try
            {
                var grado = await req.ReadFromJsonAsync<GradoAcademico>()??throw new Exception("Debe ingresar un Grado Academico con todos sus datos");
                bool seGuardo = await _gradoLogic.InsertarGradoAcademico(grado);
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
        [Function("ModificarGrado")]
        public async Task<HttpResponseData> ModificarGrado([HttpTrigger(AuthorizationLevel.Function,"put",Route = "modificarGrado/{id}")]HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar un Grado Academico");
            try
            {
                var grado = await req.ReadFromJsonAsync<GradoAcademico>()?? throw new Exception("Debe ingresar un Grado Academico con todos sus datos");
                bool seModifico = await _gradoLogic.ModificarGradoAcademico(grado,id);
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
        [Function("EliminarGrado")]
        public async Task<HttpResponseData> EliminarGrado([HttpTrigger(AuthorizationLevel.Function,"delete",Route = "eliminarGrado/{id}")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar un Grado Academico");
            try
            {
                bool seElimino = await _gradoLogic.EliminarGradoAcademico(id);
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

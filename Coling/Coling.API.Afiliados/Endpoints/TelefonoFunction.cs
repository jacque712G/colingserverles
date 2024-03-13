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
    public class TelefonoFunction
    {
        private readonly ILogger<TelefonoFunction> _logger;
        private readonly ITelefonoLogic _telefonoLogic;

        public TelefonoFunction(ILogger<TelefonoFunction> logger,ITelefonoLogic telefonoLogic)
        {
            _logger = logger;
            _telefonoLogic = telefonoLogic;
        }

        [Function("ListarTelefonos")]
        public async Task<HttpResponseData> ListarTelefonos([HttpTrigger(AuthorizationLevel.Function, "get", Route ="listartelefonos")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Telefonos");
            try
            {
                var listaTelefonos = _telefonoLogic.ListarTelefonoTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaTelefonos.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
            
        }
        [Function("ObtenerTelefonoById")]
        public async Task<HttpResponseData> ObtenerTelefonoById([HttpTrigger(AuthorizationLevel.Function,"get",Route = "obtenerTelefonoById")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener un Telefono");
            try
            {
                var telefono = _telefonoLogic.ObtenerTelefonoById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(telefono.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("InsertarTelefono")]
        public async Task<HttpResponseData> InsertarTelefono([HttpTrigger(AuthorizationLevel.Function,"post",Route = "insertarTelefono")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar un Telefono");
            try
            {
                var telefono = await req.ReadFromJsonAsync<Telefono>()?? throw new Exception("Debe ingresar un Telefono con todos sus datos");
                bool seGuardo= await _telefonoLogic.InsertarTelefono(telefono);
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

        [Function("ModificarTelefono")]
        public async Task<HttpResponseData> ModificarTelefono([HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificarTelefono/{id}")] HttpRequestData req,int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar un Telefono");
            try
            {
                var telefono = await req.ReadFromJsonAsync<Telefono>() ?? throw new Exception("Debe ingresar un Telefono con todos sus datos");
                bool seModifico = await _telefonoLogic.ModificarTelefono(telefono,id);
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
        [Function("EliminarTelefono")]
        public async Task<HttpResponseData> EliminarTelefono([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarTelefono/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar un Telefono");
            try
            {
                bool seElimino = await _telefonoLogic.EliminarTelefono(id);
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

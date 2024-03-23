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
        [OpenApiOperation("Listarspec", "ListarTelefonos", Description = "Sirve para listar todos los Telefonos")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<TelefonoDTO>), Description = "Mostrara una Lista de Telefonos")]
        public async Task<HttpResponseData> ListarTelefonos([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="listartelefonos")] HttpRequestData req)
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
        [OpenApiOperation("Obtenerspec", "ObtenerTelefonoById", Description = "Sirve para obtener un Telefono")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TelefonoDTO), Description = "Mostrara un Telefono")]
        public async Task<HttpResponseData> ObtenerTelefonoById([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "obtenerTelefonoById/{id}")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener un Telefono");
            try
            {
                var telefono = _telefonoLogic.ObtenerTelefonoById(id);
                if (telefono.Result!=null)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(telefono.Result);
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
        [Function("BuscarTelefonos")]
        [OpenApiOperation("Buscarspec", "BuscarTelefonos", Description = "Sirve para listar todos los Telefonos de una Persona")]
        [OpenApiParameter(name: "idPersona", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<TelefonoDTO>), Description = "Mostrara una Lista de Telefonos de una Persona")]
        public async Task<HttpResponseData> BuscarTelefonos([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "BuscarTelefonos/{idPersona}")] HttpRequestData req,int idPersona)
        {
           
            try
            {
                var listaTelefonos = _telefonoLogic.BuscarPersonaTelefonos(idPersona);
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
        [Function("InsertarTelefono")]
        [OpenApiOperation("Insertarspec", "InsertarTelefono", Description = "Sirve para Insertar un Telefono")]
        [OpenApiRequestBody("application/json", typeof(TelefonoDTO), Description = "Telefono modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TelefonoDTO), Description = "Mostrara el Telefono Creado")]
        public async Task<HttpResponseData> InsertarTelefono([HttpTrigger(AuthorizationLevel.Anonymous, "post",Route = "insertarTelefono")] HttpRequestData req) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar un Telefono");
            try
            {
                var telefono = await req.ReadFromJsonAsync<TelefonoDTO>()?? throw new Exception("Debe ingresar un Telefono con todos sus datos");
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
        [OpenApiOperation("Modificarspec", "ModificarTelefono", Description = "Sirve para Modificar un Telefono")]
        [OpenApiRequestBody("application/json", typeof(TelefonoDTO), Description = "Telefono modelo")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TelefonoDTO), Description = "Mostrara el Telefono Modificado")]
        public async Task<HttpResponseData> ModificarTelefono([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarTelefono/{id}")] HttpRequestData req,int id)
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar un Telefono");
            try
            {
                var telefono = await req.ReadFromJsonAsync<TelefonoDTO>() ?? throw new Exception("Debe ingresar un Telefono con todos sus datos");
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
        [OpenApiOperation("Eliminarspec", "EliminarTelefono", Description = "Sirve para Eliminar un Telefono")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> EliminarTelefono([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarTelefono/{id}")] HttpRequestData req, int id)
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

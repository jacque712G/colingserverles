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
    public class AfiliadoFunction
    {
        private readonly ILogger<AfiliadoFunction> _logger;
        private readonly IAfiliadoLogic _afiliadoLogic;

        public AfiliadoFunction(ILogger<AfiliadoFunction> logger,IAfiliadoLogic _afiliadoLogic)
        {
            _logger = logger;
            this._afiliadoLogic = _afiliadoLogic;
        }

        [Function("ListarAfiliados")]
        public async Task<HttpResponseData> ListarAfiliados([HttpTrigger(AuthorizationLevel.Function, "get", Route ="listarafiliados")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando Azure Function para Listar Afiliados");
            try
            {
                var listaAfiliados = _afiliadoLogic.ListarAfiliadoTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaAfiliados.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("ObtenerAfiliadoById")]
        public async Task<HttpResponseData> ObtenerAfiliadoById([HttpTrigger(AuthorizationLevel.Function,"get",Route = "obtenerAfiliadoById/{id}")] HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Obtener a un Afiliado");
            try
            {
                var afiliado = _afiliadoLogic.ObtenerAfiliadoById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(afiliado.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                var error=req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
        [Function("InsertarAfiliado")]
        public async Task<HttpResponseData> InsertarAfiliado([HttpTrigger(AuthorizationLevel.Function,"post",Route = "insertarAfiliado")] HttpRequestData req)         
        {
            _logger.LogInformation("Ejecutando Azure Function para Insertar un Afiliado");
            try
            {
                var afiliado = await req.ReadFromJsonAsync<Afiliado>()??throw new Exception("Debe ingresar un afiliado con todos sus datos");
                bool seGuardo = await _afiliadoLogic.InsertarAfiliado(afiliado);
                if (seGuardo)
                {
                    var respuesta=req.CreateResponse(HttpStatusCode.OK);
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
        [Function("ModificarAfiliado")]
        public async Task<HttpResponseData> ModificarAfiliado([HttpTrigger(AuthorizationLevel.Function,"put",Route = "modificarAfiliado/{id}")] HttpRequestData req, int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Modificar un Afiliado");
            try
            {
                var afiliado = await req.ReadFromJsonAsync<Afiliado>()??throw new Exception("Debe ingresar un afiliado con todos sus datos");
                bool seModifico = await _afiliadoLogic.ModificarAfiliado(afiliado,id);
                if (seModifico) 
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

        [Function("EliminarAfiliado")]
        public async Task<HttpResponseData> EliminarAfiliado([HttpTrigger(AuthorizationLevel.Function,"delete",Route = "eliminarAfiliado/{id}")]HttpRequestData req,int id) 
        {
            _logger.LogInformation("Ejecutando Azure Function para Eliminar un Afiliado");
            try
            {
                bool seElimino = await _afiliadoLogic.EliminarAfiliado(id);
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

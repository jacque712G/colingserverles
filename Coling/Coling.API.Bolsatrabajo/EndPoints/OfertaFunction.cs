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

namespace Coling.API.Bolsatrabajo.EndPoints
{
    public class OfertaFunction
    {
        private readonly ILogger<OfertaFunction> _logger;
        private readonly IOfertaLogic repos;

        public OfertaFunction(ILogger<OfertaFunction> logger,IOfertaLogic repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("ListarOfertas")]
        [OpenApiOperation("Listarspec", "ListarOfertas", Description = "Sirve para listar todas las Ofertas")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<OfertaLaboral>), Description = "Mostrara una Lista de Ofertas")]
        public async Task<HttpResponseData> ListarOfertas([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarOfertas")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var listaOfertas = repos.ListarOfertaTodas();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaOfertas.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                respuesta=req.CreateResponse(HttpStatusCode.InternalServerError);
                await respuesta.WriteAsJsonAsync(e.Message);
                return respuesta;
            }
        }

        [Function("ObtenerOfertaById")]
        [OpenApiOperation("Obtenerspec", "ObtenerOfertaById", Description = "Sirve para obtener una Oferta")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OfertaLaboral), Description = "Mostrara una Oferta")]
        public async Task<HttpResponseData> ObtenerOfertaById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ObtenerOfertaById/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                var oferta = repos.ObtenerOfertaById(id);
                if (oferta.Result!=null)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    await respuesta.WriteAsJsonAsync(oferta.Result);
                    return respuesta;
                }
                else
                {
                   respuesta= req.CreateResponse(HttpStatusCode.NotFound);
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
        [Function("BuscarInstitucionOfertas")]
        [OpenApiOperation("Buscarspec", "BuscarInstitucionOfertas", Description = "Sirve para listar todas las Ofertas de una Institucion")]
        [OpenApiParameter(name: "idInstitucion", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<OfertaLaboral>), Description = "Mostrara una Lista de Ofertas de una Institucion")]
        public async Task<HttpResponseData> BuscarInstitucionOfertas([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "BuscarInstitucionOfertas/{idInstitucion}")] HttpRequestData req,string idInstitucion)
        {
            HttpResponseData respuesta;
            try
            {
                var listaOfertas = repos.BuscarInstitucionOfertas(idInstitucion);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaOfertas.Result);
                return respuesta;
            }
            catch (Exception e)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                await respuesta.WriteAsJsonAsync(e.Message);
                return respuesta;
            }
        }
        [Function("InsertarOferta")]
        [OpenApiOperation("Insertarspec", "InsertarOferta", Description = "Sirve para Insertar una Oferta")]
        [OpenApiRequestBody("application/json", typeof(OfertaLaboral), Description = "Oferta modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OfertaLaboral), Description = "Mostrara la Oferta Creada")]
        public async Task<HttpResponseData> InsertarOferta([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "InsertarOferta")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<OfertaLaboral>()?? throw new Exception("Debe ingresar una Oferta con todos sus datos");
                bool seGuardo = await repos.InsertarOferta(registro);
                if (seGuardo)
                {
                    respuesta=req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
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
        [Function("ModificarOferta")]
        [OpenApiOperation("Modificarspec", "ModificarOferta", Description = "Sirve para Modificar una Oferta")]
        [OpenApiRequestBody("application/json", typeof(OfertaLaboral), Description = "Oferta modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OfertaLaboral), Description = "Mostrara la Oferta Modificada")]
        public async Task<HttpResponseData> ModificarOferta([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ModificarOferta")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var oferta = await req.ReadFromJsonAsync<OfertaLaboral>() ?? throw new Exception("Debe ingresar una Oferta con todos sus datos");
                bool seModifico = await repos.ModificarOferta(oferta);
                if (seModifico)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
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

        [Function("EliminarOferta")]
        [OpenApiOperation("Eliminarspec", "EliminarOferta", Description = "Sirve para Eliminar una Oferta")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        public async Task<HttpResponseData> EliminarOferta([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "EliminarOferta/{id}")] HttpRequestData req, string id)
        {
            HttpResponseData respuesta;
            try
            {
                bool seElimino = await repos.EliminarOferta(id);
                if (seElimino)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
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
     }
}

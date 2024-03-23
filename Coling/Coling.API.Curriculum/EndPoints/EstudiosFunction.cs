using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Coling.API.Curriculum.EndPoints
{
    public class EstudiosFunction
    {
        private readonly ILogger<EstudiosFunction> _logger;
        private readonly IEstudiosRepositorio repos;

        public EstudiosFunction(ILogger<EstudiosFunction> logger,IEstudiosRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("ListarEstudios")]
        [OpenApiOperation("Listarspec", "ListarEstudios", Description = "Sirve para listar todos los Estudios")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Estudios>), Description = "Mostrara una Lista de Estudios")]
        public async Task<HttpResponseData> ListarEstudios([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "ListarEstudios")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repos.GetAll();
                respuesta=req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(lista.Result);
                return respuesta;
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }

        [Function("ObtenerEstudioById")]
        [OpenApiOperation("Obtenerspec", "ObtenerEstudioById", Description = "Sirve para obtener un Estudio")]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Estudios), Description = "Mostrara un Estudio")]
        public async Task<HttpResponseData> ObtenerEstudioById([HttpTrigger(AuthorizationLevel.Anonymous,"get",Route = "ObtenerEstudioById/{rowkey}")] HttpRequestData req,string rowkey) 
        {
            HttpResponseData respuesta;
            try
            {
                var estudio=repos.GetById(rowkey);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(estudio.Result);
                return respuesta;
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("BuscarAfiliadoEstudios")]
        [OpenApiOperation("Buscarspec", "BuscarAfiliadoEstudios", Description = "Sirve para listar todos los Estudios de un Afiliado")]
        [OpenApiParameter(name: "idAfiliado", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Estudios>), Description = "Mostrara una Lista de Estudios de un Afiliado")]
        public async Task<HttpResponseData> BuscarAfiliadoEstudios([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "BuscarAfiliadoEstudios/{idAfiliado}")] HttpRequestData req,int idAfiliado)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repos.BuscarAfiliadoEstudios(idAfiliado);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(lista.Result);
                return respuesta;
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("InsertarEstudio")]
        [OpenApiOperation("Insertarspec", "InsertarEstudio", Description = "Sirve para Insertar un Estudio")]
        [OpenApiRequestBody("application/json", typeof(Estudios), Description = "Estudio modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Estudios), Description = "Mostrara el Estudio Creado")]
        public async Task<HttpResponseData> InsertarEstudio([HttpTrigger(AuthorizationLevel.Anonymous,"post",Route = "InsertarEstudio")] HttpRequestData req) 
        {
            HttpResponseData respuesta;
            try
            {
                var registro =await req.ReadFromJsonAsync<Estudios>()??throw new Exception("Debe ingresar todos los datos de un Estudio");
                registro.RowKey=Guid.NewGuid().ToString();
                registro.Timestamp=DateTime.UtcNow;
                bool seGuardo = await repos.Create(registro);
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
            catch (Exception)
            {

                respuesta=req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("ModificarEstudio")]
        [OpenApiOperation("Modificarspec", "ModificarEstudio", Description = "Sirve para Modificar un Estudio")]
        [OpenApiRequestBody("application/json", typeof(Estudios), Description = "Estudio modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Estudios), Description = "Mostrara el Estudio Modificado")]
        public async Task<HttpResponseData> ModificarEstudio([HttpTrigger(AuthorizationLevel.Anonymous,"put",Route = "ModificarEstudio")] HttpRequestData req) 
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Estudios>() ?? throw new Exception("Debe ingresar todos los datos del Estudio");
                bool seModifico=await repos.Update(registro);
                if (seModifico) 
                {
                    respuesta=req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta= req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {

                respuesta=req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("EliminarEstudio")]
        [OpenApiOperation("Eliminarspec", "EliminarEstudio", Description = "Sirve para Eliminar un Estudio")]
        [OpenApiParameter(name: "partitionkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        public async Task<HttpResponseData> EliminarEstudio([HttpTrigger(AuthorizationLevel.Anonymous,"delete",Route = "EliminarEstudio/{partitionkey}/{rowkey}")] HttpRequestData req,string partitionkey,string rowkey) 
        {
            HttpResponseData respuesta;
            try
            {
                bool seElimino = await repos.Delete(partitionkey,rowkey);
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
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
    }
}

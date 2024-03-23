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
    public class ExperienciaLaboralFunction
    {
        private readonly ILogger<ExperienciaLaboralFunction> _logger;
        private readonly IExperienciaLaboralRepositorio repos;

        public ExperienciaLaboralFunction(ILogger<ExperienciaLaboralFunction> logger,IExperienciaLaboralRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("ListarExperiencia")]
        [OpenApiOperation("Listarspec", "ListarExperiencia", Description = "Sirve para listar todas las Experiencias Laborales")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<ExperienciaLaboral>), Description = "Mostrara una Lista de Experiencias Laborales")]
        public async Task<HttpResponseData> ListarExperiencia([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarExperiencia")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repos.GetAll();
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

        [Function("ObtenerExperienciaById")]
        [OpenApiOperation("Obtenerspec", "ObtenerExperienciaById", Description = "Sirve para obtener una Experiencia Laboral")]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ExperienciaLaboral), Description = "Mostrara una Experiencia Laboral")]
        public async Task<HttpResponseData> ObtenerExperienciaById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ObtenerExperienciaById/{rowkey}")] HttpRequestData req, string rowkey)
        {
            HttpResponseData respuesta;
            try
            {
                var experiencia = repos.GetById(rowkey);
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(experiencia.Result);
                return respuesta;
            }
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("BuscarAfiliadoExperiencia")]
        [OpenApiOperation("Buscarspec", "BuscarAfiliadoExperiencia", Description = "Sirve para listar todas las Experiencias Laborales de un Afiliado")]
        [OpenApiParameter(name: "idAfiliado", In = ParameterLocation.Path, Required = true, Type = typeof(int))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<ExperienciaLaboral>), Description = "Mostrara una Lista de Experiencias Laborales de un Afiliado")]
        public async Task<HttpResponseData> BuscarAfiliadoExperiencia([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "BuscarAfiliadoExperiencia/{idAfiliado}")] HttpRequestData req,int idAfiliado)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repos.BuscarAfiliadoExperiencia(idAfiliado);
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
        [Function("InsertarExperiencia")]
        [OpenApiOperation("Insertarspec", "InsertarExperiencia", Description = "Sirve para Insertar una Experiencia Laboral")]
        [OpenApiRequestBody("application/json", typeof(ExperienciaLaboral), Description = "Experiencia Laboral modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ExperienciaLaboral), Description = "Mostrara la Experiencia Laboral Creada")]
        public async Task<HttpResponseData> InsertarExperiencia([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "InsertarExperiencia")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<ExperienciaLaboral>() ?? throw new Exception("Debe ingresar todos los datos de una Experiencia Laboral");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                bool seGuardo = await repos.Create(registro);
                if (seGuardo)
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
        [Function("ModificarExperiencia")]
        [OpenApiOperation("Modificarspec", "ModificarExperiencia", Description = "Sirve para Modificar una Experiencia Laboral")]
        [OpenApiRequestBody("application/json", typeof(ExperienciaLaboral), Description = "Experiencia Laboral modelo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ExperienciaLaboral), Description = "Mostrara la Experiencia Laboral Modificada")]
        public async Task<HttpResponseData> ModificarExperiencia([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "ModificarExperiencia")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<ExperienciaLaboral>() ?? throw new Exception("Debe ingresar todos los datos de la Experiencia Laboral");
                bool seModifico = await repos.Update(registro);
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
            catch (Exception)
            {

                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }
        }
        [Function("EliminarExperiencia")]
        [OpenApiOperation("Eliminarspec", "EliminarExperiencia", Description = "Sirve para Eliminar una Experiencia Laboral")]
        [OpenApiParameter(name: "partitionkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "rowkey", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        public async Task<HttpResponseData> EliminarExperiencia([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "EliminarExperiencia/{partitionkey}/{rowkey}")] HttpRequestData req, string partitionkey, string rowkey)
        {
            HttpResponseData respuesta;
            try
            {
                bool seElimino = await repos.Delete(partitionkey, rowkey);
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

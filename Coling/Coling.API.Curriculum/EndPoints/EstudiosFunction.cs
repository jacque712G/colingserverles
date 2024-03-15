using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
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
        public async Task<HttpResponseData> ListarEstudios([HttpTrigger(AuthorizationLevel.Function, "get",Route = "ListarEstudios")] HttpRequestData req)
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
        public async Task<HttpResponseData> ObtenerEstudioById([HttpTrigger(AuthorizationLevel.Function,"get",Route = "ObtenerEstudioById/{rowkey}")] HttpRequestData req,string rowkey) 
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
        [Function("InsertarEstudio")]
        public async Task<HttpResponseData> InsertarEstudio([HttpTrigger(AuthorizationLevel.Function,"post",Route = "InsertarEstudio")] HttpRequestData req) 
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
        public async Task<HttpResponseData> ModificarEstudio([HttpTrigger(AuthorizationLevel.Function,"put",Route = "ModificarEstudio")] HttpRequestData req) 
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
        public async Task<HttpResponseData> EliminarEstudio([HttpTrigger(AuthorizationLevel.Function,"delete",Route = "EliminarEstudio/{partitionkey}/{rowkey}")] HttpRequestData req,string partitionkey,string rowkey) 
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

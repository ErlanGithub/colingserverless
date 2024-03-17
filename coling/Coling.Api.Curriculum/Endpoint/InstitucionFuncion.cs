using Coling.Api.Curriculum.Contratos.Repositorios;
using Coling.Api.Curriculum.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Net;
using System.Text.Json;

namespace Coling.Api.Curriculum.Endpoint
{
    public class InstitucionFuncion
    {
        private readonly ILogger<InstitucionFuncion> _logger;
        private readonly IInstitucionRepositorio repos;

        public InstitucionFuncion(ILogger<InstitucionFuncion> logger, IInstitucionRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }
        /*----------------------Insertar--------------*/
        [Function("InsertarInstitucionFuncion")]
        public async Task<HttpResponseData> InsertarInstitucion([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<Institucion>() ?? throw new Exception("debe ingresar una institucion con todos los datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                bool sw = await repos.Create(registro);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else { 
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
        /*----------------------Listar todos--------------*/
        [Function("ListarTodosInstitucionFuncion")]
        [OpenApiOperation("ListarSpec", "InstitucionListarTodos",Description = "este endpoint sirve para listar todas las instituciones")]
        [OpenApiResponseWithBody(statusCode:HttpStatusCode.OK, contentType:"application/json",bodyType: typeof(List<Institucion>),
            Description = "mostrara una lista de instituciones")]
        public async Task<HttpResponseData> ListarTodosInstitucion([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
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
        /*----------------------Eliminar--------------*/
        [Function("EliminarInstitucionFuncion")]
        public async Task<HttpResponseData> EliminarInstitucion(
    [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminar/{partitionkey}/{rowkey}")] HttpRequestData req,
    string partitionkey,
    string rowkey)
        {
            HttpResponseData respuesta;
            string etag = "";
            bool sw = await repos.Delete(partitionkey, rowkey,etag);
            if (sw)
            {
                respuesta = req.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
            }

            return respuesta;
        }
        /*----------------------Listar un registro--------------*/
        [Function("ListarUnoInstitucionFuncion")]
        public async Task<HttpResponseData> ListarUnoInstitucion(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerpersona/{id}")] HttpRequestData req,
         string id)
        {
            HttpResponseData respuesta;
            var registro = await repos.get(id);

            if (registro != null)
            {
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                respuesta.Headers.Add("Content-Type", "application/json");
                await respuesta.WriteStringAsync(JsonSerializer.Serialize(registro));
            }
            else
            {
                respuesta = req.CreateResponse(HttpStatusCode.NotFound);
            }

            return respuesta;
        }
        /*----------------------Editar-----------------------*/
        [Function("EditarInstitucionFuncion")]
        public async Task<HttpResponseData> EditarInstitucion(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "editar/institucion")] HttpRequestData req)
        {
            HttpResponseData respuesta;

            try
            {
                string requestBody = await req.ReadAsStringAsync();
                Institucion institucion = JsonSerializer.Deserialize<Institucion>(requestBody);
                bool success = await repos.Update(institucion);

                if (success)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return respuesta;
        }


    }
}

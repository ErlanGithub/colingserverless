using Coling.Api.Curriculum.Contratos.Repositorios;
using Coling.Api.Curriculum.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Coling.Api.Curriculum.Endpoint
{
    public class ExperienciaFunction
    {
        private readonly ILogger<ExperienciaFunction> _logger;
        private readonly IExperienciaRepositorio repos;

        public ExperienciaFunction(ILogger<ExperienciaFunction> logger, IExperienciaRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        /*----------------------Insertar--------------*/
        [Function("ExperienciaInsertarFuncion")]
        public async Task<HttpResponseData> InsertarExperiencia([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<ExperienciaModel>() ?? throw new Exception("debe ingresar un nuevo registro con todos los datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                bool sw = await repos.Creates(registro);
                if (sw)
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
        /*----------------------Listar todos--------------*/
        [Function("ExperienciaListarTodosFuncion")]
        public async Task<HttpResponseData> ListarTodosExperiencia([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
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
        /*----------------------Listar un registro--------------*/
        [Function("ExperienciaListarUnoFuncion")]
        public async Task<HttpResponseData> ListarUnoEstudio(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerexperiencia/{id}")] HttpRequestData req,
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
        [Function("ExperienciaEditarFuncion")]
        public async Task<HttpResponseData> EditarExperiencia(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "editarExperiencia/estudios")] HttpRequestData req)
        {
            HttpResponseData respuesta;

            try
            {
                string requestBody = await req.ReadAsStringAsync();
                ExperienciaModel experiencia = JsonSerializer.Deserialize<ExperienciaModel>(requestBody);
                bool success = await repos.Update(experiencia);

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
        /*----------------------Eliminar--------------*/
        [Function("ExperienciaEliminarFuncion")]
        public async Task<HttpResponseData> EliminarExperiencia(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarexperiencia/{partitionkey}/{rowkey}")] HttpRequestData req,
                                string partitionkey,
                                string rowkey)
        {
            HttpResponseData respuesta;
            string etag = "";
            bool sw = await repos.Delete(partitionkey, rowkey, etag);
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
    }
}

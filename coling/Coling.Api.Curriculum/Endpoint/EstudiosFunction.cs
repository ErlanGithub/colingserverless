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
    public class EstudiosFunction
    {
        private readonly ILogger<EstudiosFunction> _logger;
        private readonly IEstudiosRepositorio repos;

        public EstudiosFunction(ILogger<EstudiosFunction> logger, IEstudiosRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        /*----------------------Insertar--------------*/
        [Function("EstudiosInsertarFuncion")]
        public async Task<HttpResponseData> InsertarEstudios([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<EstudiosModel>() ?? throw new Exception("debe ingresar un nuevos estudios con todos los datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                bool sw = await repos.Create(registro);
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
        [Function("EstudiosListarTodosFuncion")]
        public async Task<HttpResponseData> ListarTodosEstudios([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
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
        [Function("EstudiosListarUnoFuncion")]
        public async Task<HttpResponseData> ListarUnoEstudio(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerestudio/{id}")] HttpRequestData req,
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
        [Function("EstudiosEditarFuncion")]
        public async Task<HttpResponseData> EditarEstudios(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "modificar/estudios")] HttpRequestData req)
        {
            HttpResponseData respuesta;

            try
            {
                string requestBody = await req.ReadAsStringAsync();
                EstudiosModel estudios = JsonSerializer.Deserialize<EstudiosModel>(requestBody);
                bool success = await repos.Update(estudios);

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
        [Function("EstudiosEliminarFuncion")]
        public async Task<HttpResponseData> EliminarEstudios(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarestudios/{partitionkey}/{rowkey}")] HttpRequestData req,
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

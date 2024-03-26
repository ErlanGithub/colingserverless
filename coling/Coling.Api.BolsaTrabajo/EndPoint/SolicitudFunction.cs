using Coling.Api.BolsaTrabajo.Model;
using Coling.Api.BolsaTrabajo.Repositorios;
using Coling.Api.BolsaTrabajo.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.IO;
using System.Net;
using System.Text;

namespace Coling.Api.BolsaTrabajo.EndPoint
{
    public class SolicitudFunction
    {
        private readonly ILogger<SolicitudFunction> _logger;
        private readonly ISolicitudRepositorios repos;

        public SolicitudFunction(ILogger<SolicitudFunction> logger, ISolicitudRepositorios repos)
        {
            _logger = logger;
            this.repos = repos;
        }

      
        /*----------------------Insertar--------------*/
        [Function("SolicitudInsertarFuncion")]
        [OpenApiOperation("Listarspec", "InsertarSolicitud", Description = "Sirve para insertar una nueva solicitud")]
        [OpenApiRequestBody("application/json", typeof(SolicitudModel),
           Description = "ingresar una nueva solicitud")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(SolicitudModel),
            Description = "Muestra la solicitud insertada")]
        public async Task<HttpResponseData> Insertar([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<SolicitudModel>() ?? throw new Exception("debe ingresar un nuevo registro con todos los datos");
                Guid aleatorio = Guid.NewGuid();
                registro._id = aleatorio.ToString();
                registro.fechaPublicacion = DateTime.UtcNow.Date.ToString("dd/MM/yyyy");
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
        /*----------------------Eliminar--------------*/
        [Function("SolicitudEliminarFuncion")]
        [OpenApiOperation("Listarspec", "Eliminar Solicitud", Description = "Sirve para eliminar una solicitud")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(void),
            Description = " eliminar solicitud")]
        [OpenApiParameter(name: "_id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "El id de la solicitud")]
        public async Task<HttpResponseData> Eliminar(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminarsolicitud")] HttpRequestData req,
        FunctionContext context)
        {
            HttpResponseData respuesta;
            string id = req.Query["_id"];
            bool sw = await repos.Delete(id);

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

        /*----------------------obtener----------------------------*/
        [Function("SolicitudObtenerFuncion")]
        [OpenApiOperation("Listarspec", "Obtener Solicitud", Description = "Sirve para obtener una solicitud buscada")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(SolicitudModel),
            Description = "muestra una solicitud")]
        [OpenApiParameter(name: "_id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "id")]
        public async Task<HttpResponseData> Obtener(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obtenersolicitud")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            string id = req.Query["_id"];
            try
            {
                var lista = repos.get(id);
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
        /*----------------------Listar------------------*/
        [Function("SolicitudListarFuncion")]
        [OpenApiOperation("Listarspec", "ListarSolicitud", Description = "lista todos las solicitudes")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(List<SolicitudModel>),
            Description = "Mostrará una lista de solicitudes")]
        public async Task<HttpResponseData> GetAll(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarsolicitudes")] HttpRequestData req,
        FunctionContext context)
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
        /*----------------------Editar------------------*/
        [Function("SolicitudEditar")]
        [OpenApiOperation("Listarspec", "Editar Solicitud", Description = "modifica una solicitud")]
        [OpenApiRequestBody("application/json", typeof(SolicitudModel),
           Description = "Solicitud Modelo")]
        [OpenApiParameter(name: "_id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "El id de la solicitud")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(SolicitudModel),
            Description = "muestra la solicitud modificada")]
        public async Task<HttpResponseData> Editar([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "editarsolicitud")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            string id = req.Query["_id"];
            try
            {
                var registro = await req.ReadFromJsonAsync<SolicitudModel>() ?? throw new Exception("Debe ingresar una registro con todos sus datos");              
                bool sw = await repos.Update(registro,id);
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
    }
}


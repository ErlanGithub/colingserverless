using Coling.Api.BolsaTrabajo.Model;
using Coling.Api.BolsaTrabajo.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Coling.Api.BolsaTrabajo.EndPoint
{
    public class OfertaLaboralFunction
    {
        private readonly ILogger<OfertaLaboralFunction> _logger;
        private readonly IOfertaLabRepositorios repos;

        public OfertaLaboralFunction(ILogger<OfertaLaboralFunction> logger, IOfertaLabRepositorios repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        /*----------------------Insertar--------------*/
        [Function("OfertaLaboralInsertarFuncion")]
        [OpenApiOperation("Listarspec", "Insertar Oferta Laboral", Description = "Sirve para insertar una nueva oferta Laboral")]
        [OpenApiRequestBody("application/json", typeof(OfertaLaboralModel),
           Description = "oferta laboral a ingresar")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(OfertaLaboralModel),
            Description = "Muesta la oferta Laboral Ingresada")]
        public async Task<HttpResponseData> Insertar([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<OfertaLaboralModel>() ?? throw new Exception("debe ingresar un nuevo registro con todos los datos");
                Guid aleatorio = Guid.NewGuid();
                registro._id = aleatorio.ToString();
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
        /*----------------------Listar------------------*/
        [Function("OfertaLaboralListarFuncion")]
        [OpenApiOperation("Listarspec", "ListarOfertasLaborales", Description = "lista todos las ofertas laborales")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(List<SolicitudModel>),
            Description = "Mostrara una lista de ofertas laborales")]
        public async Task<HttpResponseData> GetAll(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarofertas")] HttpRequestData req,
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
        /*----------------------obtener----------------------------*/
        [Function("OfertaLaboralObtenerFuncion")]
        [OpenApiOperation("Listarspec", "Obtener Oferta Laboral", Description = "Sirve para obtener una oferta laboral por el id")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(OfertaLaboralModel),
            Description = "Muestra la oferta laboral buscada")]
        [OpenApiParameter(name: "_id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "id")]
        public async Task<HttpResponseData> Obtener(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "obteneroferta")] HttpRequestData req)
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
        /*----------------------Eliminar--------------*/
        [Function("OfertaLaboralEliminarFuncion")]
        [OpenApiOperation("Listarspec", "Eliminar Oferta Laboral", Description = "Sirve para eliminar una oferta laboral")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(void),
            Description = " eliminar oferta laboral")]
        [OpenApiParameter(name: "_id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "El id de la oferta laboral")]
        public async Task<HttpResponseData> Eliminar(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "eliminaroferta")] HttpRequestData req,
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
        /*----------------------Editar------------------*/
        [Function("OfertaLaboralEditarFuncion")]
        [OpenApiOperation("Listarspec", "Editar Oferta Laboral", Description = "modifica una oferta laboral")]
        [OpenApiRequestBody("application/json", typeof(OfertaLaboralModel),
           Description = "Oferta Laboral Modelo")]
        [OpenApiParameter(name: "_id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "El id de la oferta laboral")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(OfertaLaboralModel),
            Description = "muestra la oferta laboral modificada")]
        public async Task<HttpResponseData> Editar([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "editar oferta")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            string id = req.Query["_id"];
            try
            {
                var registro = await req.ReadFromJsonAsync<OfertaLaboralModel>() ?? throw new Exception("Debe ingresar una registro con todos sus datos");
                bool sw = await repos.Update(registro, id);
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

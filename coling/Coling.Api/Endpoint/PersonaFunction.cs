using Coling.Api.Afiliados.Contratos;
using Coling.Api.Afiliados.Implementacion;
using Coling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Coling.Api.Afiliados.Endpoint
{
    public class PersonaFunction
    {
        private readonly ILogger<PersonaFunction> _logger;
        private readonly IPersonaLogic personaLogic;
        public PersonaFunction(ILogger<PersonaFunction> logger, IPersonaLogic personaLogic)
        {
            _logger = logger;
            this.personaLogic = personaLogic;
        }

        [Function("ListarPersonas")]
        public async Task<HttpResponseData> ListarPersonas([HttpTrigger(AuthorizationLevel.Function, "get", Route = "listarpersonas")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para isnertar personas.");
            try
            {
                var listaPersonas = personaLogic.ListarPersonaTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaPersonas.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("InsertarPersona")]
        public async Task<HttpResponseData> InsertarPersona([HttpTrigger(AuthorizationLevel.Function, "post", Route = "insertarpersona")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar personas.");
            try
            {
                var per = await req.ReadFromJsonAsync<Persona>() ?? throw new Exception("Debe ingresar una persona con todos sus datos");
                bool seGuardo = await personaLogic.InsertarPersona(per);
                if (seGuardo)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
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

        [Function("ObtenerPersona")]
        public async Task<HttpResponseData> ObtenerPersonas([HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerpersonas/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando azure function para obtener personas.");
            try
            {
                var obtenerPersona = personaLogic.ObtnerPersonaById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(obtenerPersona.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("EliminarPersona")]
        public async Task<HttpResponseData> EliminarPersonas([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarpersonas/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando azure function para eliminar Personas.");
            try
            {
                var obtenerPersona = personaLogic.EliminarPersona(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(obtenerPersona.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("EditarPersona")]
        public async Task<HttpResponseData> EditarPersona(
    [HttpTrigger(AuthorizationLevel.Function, "put", Route = "editarpersona/{id}")] HttpRequestData req,
    int id)
        {
            _logger.LogInformation($"Ejecutando azure function para editar la persona con ID {id}.");

            try
            {
                // Leer el cuerpo de la solicitud para obtener los datos de la persona a editar
                string requestBody = await req.ReadAsStringAsync();
                var persona = JsonSerializer.Deserialize<Persona>(requestBody);

                // Realizar la lógica de actualización de la persona con el ID especificado
                var resultado = await personaLogic.ModificarPersona(persona,id);

                // Crear la respuesta con el resultado de la operación de edición
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(resultado);
                return respuesta;
            }
            catch (Exception e)
            {
                // En caso de error, devolver una respuesta de error
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }

    }
}

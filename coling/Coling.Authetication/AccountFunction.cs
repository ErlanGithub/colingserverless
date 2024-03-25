using Coling.Authetication.Model;
using ColingRepositorioAutenticacion.Contratos;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Coling.Authetication
{
    public class AccountFunction
    {
        private readonly ILogger<AccountFunction> _logger;
        private readonly IUsuarioRepositorio usuarioRepositorio;

        public AccountFunction(ILogger<AccountFunction> logger, IUsuarioRepositorio usuarioRepositorio)
        {
            _logger = logger;
            this.usuarioRepositorio = usuarioRepositorio;
        }

        [Function("Login")]
        [OpenApiOperation("Accountspec", "Account", Description = "Se obtiene el token si las credenciales son validas")]
        [OpenApiRequestBody("application/json", typeof(Credenciales), Description = "Introduzca los datos de credenciales model")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ITokenData), Description = "El token es")]
        public async Task<HttpResponseData> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            HttpResponseData? respuesta = null;
            var login = await req.ReadFromJsonAsync<Credenciales>() ?? throw new ValidationException("Sus credenciales deben ser completas");
            var tokenFinal = await usuarioRepositorio.VerficarCredenciales(login.UserName, login.Password);
            if (tokenFinal != null)
            {
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteStringAsync(tokenFinal.Token);
            }
            else { respuesta = req.CreateResponse(HttpStatusCode.Unauthorized); }

            return respuesta;
        }
    }
}

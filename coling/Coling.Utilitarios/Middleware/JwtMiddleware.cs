using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Utilitarios.Middleware
{
    public class JwtMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly IConfiguration configuration;

        public JwtMiddleware(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var request = await context.GetHttpRequestDataAsync();

            if (!EsTokenValido(request.Headers))
            {
                throw new InvalidOperationException("El token es invalido");
            }

            await next(context);
        }

        private bool EsTokenValido(IEnumerable<KeyValuePair<string, IEnumerable<string>>> cabeceras)
        {
            bool sw = false;
            string? token = null;

            var cabeceraAutorizacion = cabeceras.FirstOrDefault(h => h.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase) ||
                                                                h.Key.Equals("Bearer", StringComparison.OrdinalIgnoreCase)).Value;
            token = ExtraerToken(cabeceraAutorizacion.FirstOrDefault());

            var LlaveSecreta = configuration["LlaveSecreta"];
            if (string.IsNullOrWhiteSpace(LlaveSecreta))
            {
                throw new InvalidOperationException("NO esta configurada la llave secreta");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var validarParametros = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(LlaveSecreta)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validarParametros, out _);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string ExtraerToken(string tokenCabecera)
        {
            const string prefijoBearer = "Bearer ";
            //if(tokenCabecera.StartsWith(prefijoBearer, StringComparison.OrdinalIgnoreCase))
            //{
            return tokenCabecera.Substring(prefijoBearer.Length);
            //}

            //throw new InvalidOperationException("Operacion invalida");
        }
    }
}

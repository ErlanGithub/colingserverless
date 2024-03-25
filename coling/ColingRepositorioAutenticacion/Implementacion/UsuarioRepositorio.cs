using ColingRepositorioAutenticacion.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;



namespace ColingRepositorioAutenticacion.Implementacion
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly IConfiguration configuration;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<TokenData> ConstruirToken(string usuarioname, string password)
        {
            var claims = new List<Claim>()
            {
                new Claim("usuario", usuarioname),
                new Claim("rol", "Admin"),
                new Claim("estado", "Activo")
            };

            var SecretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["LlaveSecreta"] ?? ""));
            var creds = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(5);

            var tokenSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expires, signingCredentials: creds);

            TokenData respuestaToken = new TokenData();
            respuestaToken.Token = new JwtSecurityTokenHandler().WriteToken(tokenSeguridad);
            respuestaToken.Expira = expires;

            return respuestaToken;
        }

        public async Task<string> EncriptarPassword(string password)
        {
            string Encriptado = "";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                Encriptado = Convert.ToBase64String(bytes);
            }
            return Encriptado;
        }

        public Task<bool> ValidarToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<TokenData> VerficarCredenciales(string usuariox, string passwordx)
        {
            TokenData tokenDevolver = new TokenData();
            string passEncriptado = await EncriptarPassword(passwordx);
            string consulta = "select count(idusuario) from usuario where nombreUser='" + usuariox + "' and password='" + passEncriptado + "'";
            int Existe = conexion.EjecutarEscalar(consulta);
            if (Existe > 0)
            {
                tokenDevolver = await ConstruirToken(usuariox, passwordx);
            }
            return tokenDevolver;
        }
    }
}

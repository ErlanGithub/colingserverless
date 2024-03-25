using ColingRepositorioAutenticacion.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColingRepositorioAutenticacion.Implementacion
{
    public class TokenData : ITokenData
    {
        public DateTime Expira { get; set; }
        public string Token { get; set; }
    }
}
